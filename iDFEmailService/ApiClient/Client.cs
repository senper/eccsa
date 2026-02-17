using EmailService.Services;
using iDFEmailService.Model;
using iDFEmailService.Model.Notification;
using iDFEmailService.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace iDFEmailService.MailClient
{
    public class Client
    {
        public IConfiguration Configuration { get; }
        private string connectionString = string.Empty;
        private Repository repository = null;
        private int SendsPerEmail = 0;
        private string AttachmentUrl = "";
        private int MaxEmailPerDay => ValidSenderEmails.Sum(s => s.MaxEmailPerDay);
        private int DeliveryCheckDelayMin = 60;
        private MailService _mailService;
        private List<MailSettings> ValidSenderEmails { get; set; } = new List<MailSettings>();
        public Client(IConfiguration configuration, bool isDemo = false)
        {
            Configuration = configuration;
            SetConfig(isDemo);
            repository = new(new Context(connectionString));
            _mailService = new(configuration);
        }


        public void SetConfig(bool isDemo = false)
        {
            connectionString = Configuration.GetConnectionString(isDemo ? "DefaultConnectionDemo" : "DefaultConnection");
            try
            {
                SendsPerEmail = Configuration.GetSection("SendsPerEmail").Get<int>();
                AttachmentUrl = Configuration.GetSection("AttachmentBaseUrl").Get<string>();
                if (!string.IsNullOrWhiteSpace(AttachmentUrl))
                {
                    AttachmentUrl += "/EmailAttachments";
                }
                DeliveryCheckDelayMin = Configuration.GetSection("TimerConfig").GetSection("DeliveryCheckDelay").Get<int>();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Invalid configuration for SendsPerEmail and/or MaxEmailPerDay {0}.", DateTime.UtcNow));
            }
        }
        private string GetSignature(Guid id) => $"(Signature: {Guid2String(id)})";
        public async Task<bool> CheckDelivery()
        {
            Log.Logger.Information(string.Format("Emails delivery check started {0}", DateTime.UtcNow));
            var emails = await repository.GetProcessedEmails(DeliveryCheckDelayMin);
            Log.Logger.Information(string.Format("Found {0} processed emails {1} updated before {2} min", emails.Count, DateTime.UtcNow, DeliveryCheckDelayMin));
            if (emails.Count <= 0) return true;
            var deliveryInfos = await repository.GetLateEmailDeliveryInfos();
            if (deliveryInfos.Count <= 0 || !deliveryInfos.Any(s => s.Details.Count > 0)) return true;
            var senders = await repository.GetSenderEmails();
            foreach (var msg in emails)
            {
                var request = new MailRequest
                {
                    Body = msg.Body,
                    Subject = msg.Subject,
                    ToEmail = new List<string>()
                };
                var curEmailSenders = senders.Where(s => deliveryInfos.Any(d => d.SenderEmail == s.Mail && d.Details.Any(m => m.EmailId == msg.Id))).ToList();
                var signature = GetSignature(msg.Id);
                var failedList = await _mailService.CheckDeliveryStatus(signature, request, curEmailSenders);
                await repository.UpdateDeliveryStatus(msg.Id, failedList);
                Log.Logger.Information(string.Format("Updated delivery status for email with failed {0} at time {1}", failedList.Length, DateTime.UtcNow));
            }
            Log.Logger.Information(string.Format("Emails delivery check ended {0}", DateTime.UtcNow));
            return true;
        }
        protected string Guid2String(Guid id) => id.ToString()[^15..].Replace("-", "");
        private MailSettings PickSender(long indentityId)
        {
            if (!int.TryParse($"{indentityId % ValidSenderEmails.Count}", out var mod)) return null;

            return ValidSenderEmails[mod];
        }
        public async Task<int> SendEmail()
        {
            ValidSenderEmails = await repository.GetValidSenderEmails();
            if (ValidSenderEmails.Count <= 0)
            {
                Log.Logger.Warning(string.Format("No active sender email(s) with remaining send for day found at {0}. Email sending aborted", DateTime.UtcNow));
                return MaxEmailPerDay;
            }

            var remainingForDay = MaxEmailPerDay - ValidSenderEmails.Sum(s => s.DeliveryInfo.Count);
            if (remainingForDay <= 0) return 0;
            Log.Logger.Information(string.Format("Sending email started at {0}", DateTime.UtcNow));

            var emails = await repository.GetEmails();
            foreach (var msg in emails)
            {
                if (remainingForDay <= 0) break;
                var sender = PickSender(msg.IdentityId);
                if (sender == null)
                {
                    Log.Logger.Error(string.Format("No active sender email(s) with remaining send for today found at {0}. Email sending aborted", DateTime.UtcNow));
                    break;
                }

                var remainingForSender = sender.MaxEmailPerDay - sender.DeliveryInfo.Count;
                var noOfRecToTake = Math.Min(remainingForSender, SendsPerEmail);

                var selectedReceipents = msg.Recipients.Where(r => r.Status == Model.Notification.DeliveryStatus.Planned).Take(noOfRecToTake);
                var selectedAdresses = selectedReceipents.Select(s => s.Address).Distinct().ToList();

                if (selectedAdresses.Count <= 0)
                {
                    Log.Logger.Warning(string.Format("No recipient found for the email at {0}. Email sending aborted", DateTime.UtcNow));
                    continue;
                }
                var successEmailSend = await PrepareAndSendEmail(msg, sender, selectedAdresses);

                if (successEmailSend)
                {
                    sender.DeliveryInfo.Count += selectedAdresses.Count;
                    await UpdateEmailSendInfo(msg, sender, selectedReceipents);
                }

                ValidSenderEmails = ValidSenderEmails.Where(s => s.DeliveryInfo.Count < s.MaxEmailPerDay).ToList();
                remainingForDay = MaxEmailPerDay - ValidSenderEmails.Sum(s => s.DeliveryInfo.Count);
            }

            Log.Logger.Information(string.Format("Sending email ended at {0}", DateTime.UtcNow));
            return remainingForDay;
        }
        private async Task<bool> PrepareAndSendEmail(EmailMessage msg, MailSettings sender, List<string> selectedAdresses)
        {
            var signature = Guid2String(msg.Id);
            var request = new MailRequest
            {
                //Attachments = file != null ? new List<IFormFile> { file } : new List<IFormFile>(),
                Body = msg.Body,
                Subject = msg.Subject,
                ToEmail = selectedAdresses,
                SenderEmail = sender
            };
            if (!string.IsNullOrWhiteSpace(msg.Attachment) && !string.IsNullOrWhiteSpace(AttachmentUrl))
            {
                request.Body += $"{Environment.NewLine}{Environment.NewLine}<a href='{AttachmentUrl}/{signature}{msg.Attachment}' download='{msg.Subject}{msg.Attachment}' target='_blank'><strong>Click Here to View/Download Attachment!</strong></a>";
            }
            request.Body += $"{Environment.NewLine}{GetSignature(msg.Id)}";
            try
            {
                await _mailService.SendEmailAsync(request);
                Log.Logger.Information(string.Format("Sent {0} email(s) at {1}", selectedAdresses.Count, DateTime.UtcNow));
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Error in sending email list {0}", DateTime.UtcNow));
                return false;
            }
        }
        private async Task UpdateEmailSendInfo(EmailMessage msg, MailSettings sender, IEnumerable<MessageDelivery> selectedReceipents)
        {
            try
            {
                sender.DeliveryInfo = await repository.Save(msg, selectedReceipents.ToList(), sender.DeliveryInfo);

                Log.Logger.Information(string.Format("Updated Recipient status for {0} at {1}", selectedReceipents.Count(), DateTime.UtcNow));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, string.Format("Error in updating recipient status {0}", DateTime.UtcNow));
            }
        }
        public async Task SendSMS()
        {
            var smsList = await repository.GetSMS();
            foreach (var msg in smsList)
            {
                var noOfRecToTake = msg.Priority > 9 ? SendsPerEmail : msg.Recipients.Count;

                var selectedReceipents = msg.Recipients.Where(r => r.Status == Model.Notification.DeliveryStatus.Planned).Take(noOfRecToTake);
                var selectedAdresses = selectedReceipents.Select(s => s.Address).Distinct().ToList();

                if (selectedAdresses.Count <= 0)
                {
                    Log.Logger.Warning(string.Format("No recipient found for the SMS at {0}. SMS sending aborted", DateTime.UtcNow));
                    continue;
                }
                var sendReceipents = new List<MessageDelivery>();
                foreach (var sms in selectedAdresses)
                {
                    try
                    {
                        var successEmailSend = await SMSService.Send(sms, msg.Message);
                        if(successEmailSend && selectedReceipents.Any(d => d.Address == sms))
                        {
                            sendReceipents.Add(selectedReceipents.FirstOrDefault(d => d.Address == sms));
                        }
                        else if (!successEmailSend)
                        {
                            Log.Logger.Error(string.Format("Failed to send SMS for address {0} at {1}", sms, DateTime.UtcNow));
                        }
                    }
                    catch(Exception ex)
                    {
                        Log.Logger.Error(ex, string.Format("Failed to send SMS for address {0} at {1}", sms, DateTime.UtcNow));
                    }
                }
                try
                {
                    await repository.Save(msg, sendReceipents.ToList());

                    Log.Logger.Information(string.Format("Updated SMS Recipient status for {0} at {1}", sendReceipents.Count(), DateTime.UtcNow));
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, string.Format("Error in updating SMS recipient status {0}", DateTime.UtcNow));
                }
            }

            Log.Logger.Information(string.Format("Sending SMS ended at {0}", DateTime.UtcNow));
        }
    }
}
