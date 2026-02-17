using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using iDFEmailService.Model;
using iDFEmailService.Model.Notification;
using iDFEmailService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace iDFEmailService.Persistence
{
    /*
     * eccsasystems@gmail.com	lbjvxnockyazighc
        eccsainfo@gmail.com	mdtdogutehzcrbpo
     * */
    public class Repository
    {
        private readonly Context _context;
        private readonly SymmetricEncryptionService tool = new();
        public Repository(Context context)
        {
            _context = context;
        }


        public async Task<List<MailSettings>> GetSenderEmails()
        {
            var list = await _context.MailSettings
                .Where(d => d.DeactivatedStamp == null).OrderBy(d => d.CreateStamp).AsNoTracking().ToListAsync();
            foreach(var l in list)
            {
                //var enc = tool.Encrypt(l.Password);
                //var encStr = tool.ByteArrayToString(enc);
                var dec = tool.Decrypt(tool.StringToByteArray(l.Password));
                l.Password = dec.Replace("\r\n", "");
            }
            return list;
        }

        public async Task<List<MailSettings>> GetValidSenderEmails()
        {
            var sendersHistory = await _context.DeliveryInfos.OrderByDescending(d => d.Date)
                .Where(d => d.Date >= DateTime.UtcNow.Date).ToListAsync();
            var list = await _context.MailSettings
                .Where(d => d.DeactivatedStamp == null).AsNoTracking().OrderBy(d => d.CreateStamp).ToListAsync();
            var validSenders = new List<MailSettings>();
            var hasAnyAdd = false;
            foreach (var sender in list)
            {
                var history = sendersHistory.FirstOrDefault(s => s.SenderEmail == sender.Mail);
                if (history == null)
                {
                    history = new EmailDeliveryInfo
                    {
                        Id = Guid.NewGuid(),
                        SenderEmail = sender.Mail,
                        Date = DateTime.UtcNow.Date,
                        Count = 0,
                        Details = new List<DeliveryDetail>()
                    };
                    _context.DeliveryInfos.Add(history);
                    hasAnyAdd = true;
                }
                if (history.Count < sender.MaxEmailPerDay)
                {
                    sender.DeliveryInfo = history;
                    validSenders.Add(sender);
                }
            }
            if (hasAnyAdd)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Log.Logger.Warning(string.Format("Unable to new Delivery Infos for senders at time {0}", DateTime.UtcNow.ToString()), ex);
                }
            }
            foreach (var l in validSenders)
            {
                //var enc = tool.Encrypt(l.Password);
                //var encStr = tool.ByteArrayToString(enc);
                var dec = tool.Decrypt(tool.StringToByteArray(l.Password));
                l.Password = dec.Replace("\r\n", "");
            }
            return validSenders;

        }

        public async Task<List<EmailDeliveryInfo>> GetLateEmailDeliveryInfos()
        {
            var tmp = await _context.DeliveryInfos.OrderByDescending(d => d.Date)
                .Where(d => d.Date >= DateTime.UtcNow.Date.AddDays(-1)).ToListAsync();
            if (tmp != null) return tmp;
            return tmp;
        }

        public async Task<List<EmailMessage>> GetProcessedEmails(int updatedBeforeMin)
        {
            return await _context.EmailMessages.OrderBy(d => d.UpdateStamp)
                .Where(d => d.Status == MessageStatus.Processed
                && d.UpdateStamp < DateTime.UtcNow.AddMinutes(-updatedBeforeMin)
                ).ToListAsync();
        }
        public async Task<List<EmailMessage>> GetEmails()
        {
            return await _context.EmailMessages
                .Where(d => d.Status == MessageStatus.Submitted &&
                d.UpdateStamp < DateTime.UtcNow.AddMinutes(5)).OrderBy(d => d.UpdateStamp).ToListAsync();
        }
        public async Task<List<SMSMessage>> GetSMS()
        {
            return await _context.SMSMessages
                .Where(d => d.Status == MessageStatus.Submitted).OrderByDescending(d => d.Priority).ThenBy(d => d.UpdateStamp).ToListAsync();
        }

        public async Task<EmailDeliveryInfo> Save(EmailMessage email, List<MessageDelivery> sentList, EmailDeliveryInfo info)
        {
            if (!(sentList?.Count > 0)) return info;
            var tmp = await _context.EmailMessages.FirstOrDefaultAsync(e => e.Id == email.Id);
            tmp.UpdateStamp = DateTime.UtcNow;
            var deliveryInfo = new DeliveryDetail
            {
                CreateStamp = DateTime.UtcNow,
                EmailId = email.Id,
                Subject = email.Subject,
                SentList = new List<MessageDelivery>()
            };
            foreach (var delivery in sentList)
            {
                var recipient = tmp.Recipients.First(e =>
                    e.Address == delivery.Address && e.Label == delivery.Label && e.Status == delivery.Status);
                recipient.Status = DeliveryStatus.Sent;
                delivery.Status = DeliveryStatus.Sent;
                deliveryInfo.SentList.Add(delivery);
            }

            if (!tmp.Recipients.Any(r => r.Status == DeliveryStatus.Planned))
            {
                tmp.Status = MessageStatus.Processed;
            }
            info.Details.Add(deliveryInfo);
            try
            {
                _context.EmailMessages.Update(tmp);
                _context.DeliveryInfos.Update(info);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // throw ex;
                Log.Logger.Warning(string.Format("Unable to save Email Sent Info status for at time {0}", DateTime.UtcNow.ToString()), ex);
            }
            return info;
        }

        public async Task<bool> Save(SMSMessage data, List<MessageDelivery> sentList)
        {
            if (!(sentList?.Count > 0)) return false;
            var tmp = await _context.SMSMessages.FirstOrDefaultAsync(e => e.Id == data.Id);
            if(tmp == null) return false;
            tmp.UpdateStamp = DateTime.UtcNow;
            foreach (var delivery in sentList)
            {
                var recipient = tmp.Recipients.First(e =>
                    e.Address == delivery.Address && e.Label == delivery.Label && e.Status == delivery.Status);
                recipient.Status = DeliveryStatus.Sent;
                delivery.Status = DeliveryStatus.Sent;
            }

            if (!tmp.Recipients.Any(r => r.Status == DeliveryStatus.Planned))
            {
                tmp.Status = MessageStatus.Processed;
            }
            try
            {
                _context.SMSMessages.Update(tmp);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // throw ex;
                Log.Logger.Warning(string.Format("Unable to save SMS Sent Info status for at time {0}", DateTime.UtcNow.ToString()), ex);
            }
            return true;
        }

        public async Task UpdateDeliveryStatus(Guid id, string[] failedList)
        {
            var msg = await _context.EmailMessages.FirstOrDefaultAsync(e => e.Id == id);
            msg.UpdateStamp = DateTime.UtcNow;
            msg.Status = MessageStatus.Closed;
            if (failedList?.Length > 0)
            {
                var failedRecList = msg.Recipients.Where(r => failedList.Contains(r.Address));
                foreach (var rec in failedRecList)
                {
                    rec.Status = DeliveryStatus.NotDelivered;
                }
            }
            try
            {
                _context.EmailMessages.Update(msg);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Warning(string.Format("Unable to save Email Sent failed delivery status for at time {0}", DateTime.UtcNow.ToString()), ex);
            }
        }
    }
}
