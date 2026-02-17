using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.Model.Security;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification
{
    public class NotificationRepository
    {
        private readonly EccsaContext _context;
        private readonly UserAdministrationRepository _userRepository;
        public NotificationRepository(EccsaContext context, UserAdministrationRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }



        public async Task<List<RecipientSource>> Recipients(ECCSAModuleFlag? subSystem
            , Guid sectorId
            , StakeholderTypeEnum stakeholderType = StakeholderTypeEnum.NotSet
             )
        {

            var recipients = await _context.Recipients
               .Where(x =>
               (subSystem == null ||
               x.Owner.HasFlag(subSystem)) &&
               (stakeholderType == StakeholderTypeEnum.NotSet ||
               x.StakeholderType == stakeholderType) &&
               (sectorId == Guid.Empty || (x.SectorId.HasValue && x.SectorId.Value == sectorId))).ToListAsync();



            return recipients;
        }

        public async Task ImportPublicContacts(ECCSAModuleFlag module)
        {
            var users = await _userRepository.PublicUsers();
            users = users.Where(u => u.AssociationId == Guid.Empty && u.DefaultModule == 0).ToList();
            if (users.Count <= 0)
            {
                throw new InvalidOperationException("No data to import");
            }
            var tmp = await _context.Recipients.Where(r => r.StakeholderType == StakeholderTypeEnum.Public).ToListAsync();
            _context.Recipients.RemoveRange(tmp);
            await _context.SaveChangesAsync();
            foreach (var user in users)
            {
                var recipient = new RecipientSource
                {
                    Id = user.Id,
                    CompanyName = "ECCSA",
                    CreatedBy = Guid.Empty,
                    CreateStamp = DateTime.UtcNow,
                    Email = user.Email,
                    MobileNo = string.IsNullOrEmpty(user.MobileNo) ? " " : user.MobileNo,
                    Name = user.FullName,
                    Owner = user.DefaultModule,
                    StakeholderType = StakeholderTypeEnum.Public,
                    UpdatedBy = Guid.Empty,
                    UpdateStamp = DateTime.UtcNow
                };
                _context.Recipients.Add(recipient);
            }
            await _context.SaveChangesAsync();
        }
        public async Task ImportEccsaStaff(ECCSAModuleFlag module)
        {
            var users = await _userRepository.BackOfficeUsers();
            users = users.Where(u => u.AssociationId == Constants.ECCSA/* && u.DefaultModule == module*/).ToList();
            if (users.Count <= 0)
            {
                throw new InvalidOperationException("No data to import");
            }
            var tmp = await _context.Recipients.Where(r => r.StakeholderType == StakeholderTypeEnum.Management
            /*&& r.Owner.HasFlag(module)*/).ToListAsync();
            _context.Recipients.RemoveRange(tmp);
            await _context.SaveChangesAsync();
            foreach (var user in users)
            {
                var recipient = new RecipientSource
                {
                    Id = user.Id,
                    CompanyName = "ECCSA",
                    CreatedBy = Guid.Empty,
                    CreateStamp = DateTime.UtcNow,
                    Email = user.Email,
                    MobileNo = string.IsNullOrEmpty(user.MobileNo) ? " " : user.MobileNo,
                    Name = user.FullName,
                    Owner = user.DefaultModule,
                    StakeholderType = StakeholderTypeEnum.Management,
                    UpdatedBy = Guid.Empty,
                    UpdateStamp = DateTime.UtcNow
                };
                _context.Recipients.Add(recipient);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ImportAssociations()
        {
            var associations = await _context.Associations.Include(a => a.Stakeholder)
                .ThenInclude(a => a.Contacts).Include(a => a.Employees).ThenInclude(e => e.PositionType)
                .Where(a => a.Stakeholder.Status == StatusEnum.Active).ToListAsync();
            if (associations.Count <= 0)
            {
                throw new InvalidOperationException("No data to import");
            }
            var tmp = await _context.Recipients.Where(r => r.StakeholderType == StakeholderTypeEnum.Association).ToListAsync();
            _context.Recipients.RemoveRange(tmp);
            await _context.SaveChangesAsync();
            foreach (var user in associations)
            {
                var email = user.Stakeholder.Contacts.FirstOrDefault(c => c.ContactType == ContactTypeEnum.Email)?.Address ?? String.Empty;
                var mobile = user.Stakeholder.Contacts.FirstOrDefault(c => c.ContactType == ContactTypeEnum.Mobile)?.Address ?? String.Empty;
                if (!string.IsNullOrEmpty(email + mobile))
                {
                    var recipient = new RecipientSource
                    {
                        Id = user.Id,
                        CompanyName = user.NameEng,
                        CreatedBy = Guid.Empty,
                        CreateStamp = DateTime.UtcNow,
                        Email = email,
                        MobileNo = mobile,
                        Name = user.NameEng,
                        Owner = ECCSAModuleFlag.Membership,
                        StakeholderType = StakeholderTypeEnum.Association,
                        UpdatedBy = Guid.Empty,
                        UpdateStamp = DateTime.UtcNow
                    };
                    _context.Recipients.Add(recipient);
                }
                foreach (var emp in user.Employees)
                {
                    var recipient = new RecipientSource
                    {
                        Id = emp.Id,
                        CompanyName = user.NameEng,
                        CreatedBy = Guid.Empty,
                        CreateStamp = DateTime.UtcNow,
                        Email = emp.Email,
                        MobileNo = emp.Mobile,
                        Name = (emp.Title?.Length > 0 ? emp.Title + " " : "") + emp.Name,
                        Position = emp.PositionType.Name,
                        Owner = ECCSAModuleFlag.Membership,
                        StakeholderType = StakeholderTypeEnum.Association,
                        UpdatedBy = Guid.Empty,
                        UpdateStamp = DateTime.UtcNow
                    };
                    _context.Recipients.Add(recipient);
                }
            }
            await _context.SaveChangesAsync();
        }

       
        public async Task ImportMembers()
        {
            var individuals = await _context.Individuals.Include(a => a.Stakeholder)
                 .ThenInclude(a => a.Contacts).Include(a => a.Employees).ThenInclude(e => e.PositionType)
                 .Where(a => a.Stakeholder.Status == StatusEnum.Active).ToListAsync();
            if (individuals.Count <= 0)
            {
                throw new InvalidOperationException("No data to import");
            }
            var tmp = await _context.Recipients.Where(r => r.StakeholderType == StakeholderTypeEnum.Individual).ToListAsync();
            _context.Recipients.RemoveRange(tmp);
            await _context.SaveChangesAsync();
            foreach (var user in individuals)
            {
                var email = user.Stakeholder.Contacts.FirstOrDefault(c => c.ContactType == ContactTypeEnum.Email)?.Address ?? String.Empty;
                var mobile = user.Stakeholder.Contacts.FirstOrDefault(c => c.ContactType == ContactTypeEnum.Mobile)?.Address ?? String.Empty;
                if (!string.IsNullOrEmpty(email + mobile))
                {
                    var recipient = new RecipientSource
                    {
                        Id = user.Id,
                        CompanyName = String.IsNullOrWhiteSpace(user.NameEng) ? user.NameLocal : user.NameEng,
                        CreatedBy = Guid.Empty,
                        CreateStamp = DateTime.UtcNow,
                        Email = email,
                        MobileNo = mobile,
                        Name = String.IsNullOrWhiteSpace(user.NameEng) ? user.NameLocal : user.NameEng,
                        Owner = ECCSAModuleFlag.Membership,
                        SectorId = user.SectorId,
                        StakeholderType = StakeholderTypeEnum.Individual,
                        UpdatedBy = Guid.Empty,
                        UpdateStamp = DateTime.UtcNow
                    };
                    _context.Recipients.Add(recipient);
                }
                foreach (var emp in user.Employees)
                {
                    var recipient = new RecipientSource
                    {
                        Id = emp.Id,
                        CompanyName = String.IsNullOrWhiteSpace(user.NameEng) ? user.NameLocal : user.NameEng,
                        CreatedBy = Guid.Empty,
                        CreateStamp = DateTime.UtcNow,
                        Email = emp.Email,
                        MobileNo = emp.Mobile,
                        Name = (emp.Title?.Length > 0 ? emp.Title + " " : "") + emp.Name,
                        Position = emp.PositionType.Name,
                        Owner = ECCSAModuleFlag.Membership,
                        StakeholderType = StakeholderTypeEnum.Association,
                        UpdatedBy = Guid.Empty,
                        UpdateStamp = DateTime.UtcNow
                    };
                    _context.Recipients.Add(recipient);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task<RecipientSource> RecipientById(Guid id)
        {
            var request = await _context.Recipients
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }

        public async Task Delete(RecipientSource data)
        {
            var tmp = await _context.Recipients.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("RecipientSource  not found");
            _context.Recipients.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Save(RecipientSource data, Guid userId)
        {
            if (_context.Recipients.Any(r => r.Email == data.Email && data.Id != r.Id))
            {
                throw new InvalidOperationException($"Recipient with Email {data.Email} already registered!");
            }
            if (_context.Recipients.Any(r => r.MobileNo == data.MobileNo && data.Id != r.Id))
            {
                throw new InvalidOperationException($"Recipient with Mobile No {data.MobileNo} already registered!");
            }
            var tmp = await _context.Recipients.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp != null && tmp.MobileNo != data.MobileNo && tmp.Email != data.Email)
            {
                throw new InvalidOperationException($"Cannot change email & mobile no at the same time!");
            }
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.Recipients.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Save(List<RecipientSource> dataList, Guid userId)
        {
            foreach (var data in dataList)
            {
                if (_context.Recipients.Any(r => data.Email.Trim() != string.Empty && r.Email == data.Email && data.Id != r.Id))
                {
                    throw new InvalidOperationException($"Recipient with Email {data.Email} already registered!");
                }
                if (_context.Recipients.Any(r => data.MobileNo.Trim() != string.Empty && r.MobileNo == data.MobileNo && data.Id != r.Id))
                {
                    throw new InvalidOperationException($"Recipient with Mobile No {data.MobileNo} already registered!");
                }
                var tmp = await _context.Recipients.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp != null && tmp.MobileNo != data.MobileNo && tmp.Email != data.Email)
                {
                    throw new InvalidOperationException($"Cannot change email & mobile no at the same time!");
                }
                data.UpdatedBy = userId;
                data.UpdateStamp = DateTime.UtcNow;
                if (data.Id == Guid.Empty || tmp == null)
                {
                    data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                    data.CreatedBy = userId;
                    data.CreateStamp = DateTime.UtcNow;
                    _context.Recipients.Add(data);
                }
                else
                {
                    _context.Entry(tmp).CurrentValues.SetValues(data);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task AddRecipient(Guid id, string email, string label, CcGroup ccGroup, Guid userId)
        {
            var tmp = await EmailMessageById(id);
            if (tmp == null)
            {
                throw new KeyNotFoundException($"Email {id} not found");
            }
            if (!Validation.IsEmail(email))
            {
                throw new InvalidOperationException($"Email {email} not valid");
            }
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            var rec = tmp.Recipients.FirstOrDefault(r => r.Address.ToLowerInvariant() == email.ToLowerInvariant());
            if (rec != null)
            {
                rec.CCGroup = ccGroup;
            }
            else
            {
                tmp.Recipients.Add(new MessageDelivery(email, label, ccGroup));
            }
            _context.EmailMessages.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveRecipient(Guid id, string email, Guid userId)
        {
            var tmp = await EmailMessageById(id);
            if (tmp == null)
            {
                throw new KeyNotFoundException($"Email {id} not found");
            }
            if (!Validation.IsEmail(email))
            {
                throw new InvalidOperationException($"Email {email} not valid");
            }
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            var rec = tmp.Recipients.FirstOrDefault(r => r.Address.ToLowerInvariant() == email.ToLowerInvariant());
            if (rec != null)
            {
                tmp.Recipients.Remove(rec);
            }
            _context.EmailMessages.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task SaveValue(RecipientSource data, Guid userId)
        {
            var tmp = await _context.Recipients.FirstOrDefaultAsync(x =>
            x.Name == data.Name &&
            x.CompanyName == data.CompanyName &&
            x.Owner.HasFlag(data.Owner) &&
            x.StakeholderType == data.StakeholderType &&
            (x.MobileNo == data.MobileNo || x.Email == data.Email));
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.Recipients.Add(data);
            }
            else
            {
                data.Id = tmp.Id;
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmailTemplate>> EmailTemplates(ECCSAModuleFlag subSystem)
        {
            return await _context.EmailTemplates
               .Where(x => x.Owner.HasFlag(subSystem)).ToListAsync();

        }

        public async Task<EmailTemplate> EmailTemplateById(Guid id)
        {
            var request = await _context.EmailTemplates
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }

        public async Task Delete(EmailTemplate data)
        {
            var tmp = await _context.EmailTemplates.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Email Template  not found");
            _context.EmailTemplates.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Save(EmailTemplate data, Guid userId)
        {
            var tmp = await _context.EmailTemplates.FirstOrDefaultAsync(x => x.Id == data.Id);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.EmailTemplates.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<EmailMessage>> EmailMessages(ECCSAModuleFlag subSystem, MessageStatus status = MessageStatus.Draft)
        {
            return await _context.EmailMessages
               .Where(x => x.Owner.HasFlag(subSystem) && x.Status == status).ToListAsync();

        }

        public async Task<EmailMessage> EmailMessageById(Guid id)
        {
            var request = await _context.EmailMessages
                    .FirstOrDefaultAsync(i => i.Id == id);
            if (request != null)
                request.Template = await _context.EmailTemplates.FirstOrDefaultAsync(t => t.Id == request.TemplateId);
            return request;
        }

        public async Task Delete(EmailMessage data)
        {
            var tmp = await _context.EmailMessages.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Email Template  not found");
            _context.EmailMessages.Remove(tmp);
            await _context.SaveChangesAsync();
        }


        public async Task Save(EmailMessage data, Guid userId)
        {
            var tmp = await _context.EmailMessages.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.Status = data.Status;
                data.Owner = data.Owner;
            }
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;

            if (data.Id == Guid.Empty || tmp == null)
            {
                _context.EmailMessages.Add(data);
            }
            else
            {
                _context.EmailMessages.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> CopyEmail(Guid id, Guid userId)
        {
            var org = await _context.EmailMessages.FirstOrDefaultAsync(x => x.Id == id);
            if (org == null)
            {
                throw new Exception("Source email not found!");
            }

            var tmp = new EmailMessage
            {
                CreatedBy = userId,
                CreateStamp = DateTime.UtcNow,
                Status = MessageStatus.Draft,
                Owner = org.Owner,
                Subject = "Copy of " + org.Subject,
                Body = org.Body,
                Attachment = org.Attachment,
                TemplateId = org.TemplateId,
                Recipients = org.Recipients.ToList(),
                UpdatedBy = userId,
                ReferenceId = org.ReferenceId,
                UpdateStamp = DateTime.UtcNow,

                Id = Guid.NewGuid()
            };
            tmp.Recipients.ForEach(s => s.Status = DeliveryStatus.NotSet);
            _context.EmailMessages.Add(tmp);
            await _context.SaveChangesAsync();
            return tmp.Id;
        }
        public async Task ChangeStatus(Guid id, MessageStatus status, Guid userId)
        {
            var tmp = await _context.EmailMessages.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null)
            {
                return;
            }
            else
            {
                if (status == MessageStatus.Submitted &&
                     (string.IsNullOrWhiteSpace(tmp.Body) || string.IsNullOrWhiteSpace(tmp.Subject)
                     || tmp.Recipients == null || tmp.Recipients.Count <= 0))
                {
                    throw new InvalidOperationException("Cannot submit message with no body or no subject or no recipient");
                }
                if (status == MessageStatus.Processed && tmp.Status != MessageStatus.Submitted)
                {
                    throw new InvalidOperationException("Only submitted messaged can be set to processed");
                }
                if (status == MessageStatus.Canceled && tmp.Status != MessageStatus.Submitted)
                {
                    throw new InvalidOperationException("Only submitted messaged can be set to canceled");
                }
                if (status == MessageStatus.Submitted)
                {
                    tmp.Recipients.ForEach(r => r.Status = DeliveryStatus.Planned);
                }
                tmp.UpdatedBy = userId;
                tmp.UpdateStamp = DateTime.UtcNow;
                tmp.Status = status;
                _context.EmailMessages.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
        public async Task ChangeSMSStatus(Guid id, MessageStatus status, Guid userId)
        {
            var tmp = await _context.SMSMessages.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null)
            {
                return;
            }
            else
            {
                if (status == MessageStatus.Submitted &&
                     (tmp.Message.Trim() == string.Empty || tmp.Recipients == null || tmp.Recipients.Count <= 0))
                {
                    throw new InvalidOperationException("Cannot submit message with no body or recipient");
                }
                if (status == MessageStatus.Processed && tmp.Status != MessageStatus.Submitted)
                {
                    throw new InvalidOperationException("Only submitted messaged can be set to processed");
                }
                tmp.UpdatedBy = userId;
                tmp.UpdateStamp = DateTime.UtcNow;
                tmp.Status = status;
                _context.SMSMessages.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Submit(EmailMessage data, Guid userId)
        {
            var tmp = await _context.EmailMessages.FirstOrDefaultAsync(x => x.Id == data.Id && x.Status == MessageStatus.Draft);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (tmp == null)
            {
                return;
            }
            else
            {
                tmp.Status = MessageStatus.Submitted;
                _context.EmailMessages.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<SMSMessage>> SMSMessages(ECCSAModuleFlag subSystem, MessageStatus status = MessageStatus.Draft)
        {
            return await _context.SMSMessages
               .Where(x => x.Owner.HasFlag(subSystem) && x.Status == status).ToListAsync();

        }

        public async Task<SMSMessage> SMSMessageById(Guid id)
        {
            var request = await _context.SMSMessages
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }

        public async Task Delete(SMSMessage data)
        {
            var tmp = await _context.SMSMessages.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("SMS Message  not found");
            _context.SMSMessages.Remove(tmp);
            await _context.SaveChangesAsync();
        }


        public async Task Save(SMSMessage data, Guid userId)
        {
            var tmp = await _context.SMSMessages.FirstOrDefaultAsync(x => x.Id == data.Id);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.Status = MessageStatus.Draft;
                _context.SMSMessages.Add(data);
            }
            else
            {
                tmp.Message = data.Message;
                tmp.Recipients = data.Recipients;
                //Owner and Status are intentionally ommitted
                _context.SMSMessages.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Submit(SMSMessage data, Guid userId)
        {
            var tmp = await _context.SMSMessages.FirstOrDefaultAsync(x => x.Id == data.Id && x.Status == MessageStatus.Draft);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (tmp == null)
            {
                return;
            }
            else
            {
                tmp.Status = MessageStatus.Submitted;
                _context.SMSMessages.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }


        public async Task SaveTempReceipients(List<TempRecipientSource> data, Guid userId)
        {
            _context.TempRecipients.RemoveRange(_context.TempRecipients);
            _context.TempRecipients.AddRange(data);
            await _context.SaveChangesAsync();
        }

        public async Task SaveSectorMap(ImportRecipientSectorMap data, Guid userId)
        {
            if (data == null) return;
            if (_context.ImportRecipientSectorMap.Any(s => s.ImportSectorName.ToLower() == data.ImportSectorName.ToLower()))
            {
                var temp = _context.ImportRecipientSectorMap.FirstOrDefault(s => s.ImportSectorName.ToLower() == data.ImportSectorName.ToLower());
                temp.SectorId = data.SectorId;
            }
            else
            {
                _context.ImportRecipientSectorMap.Add(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task SaveSectorMap(List<ImportRecipientSectorMap> data, Guid userId)
        {
            if (data == null) return;
            foreach (var item in data)
            {
                if (_context.ImportRecipientSectorMap.Any(s => s.ImportSectorName.ToLower() == item.ImportSectorName.ToLower()))
                {
                    var temp = _context.ImportRecipientSectorMap.FirstOrDefault(s => s.ImportSectorName.ToLower() == item.ImportSectorName.ToLower());
                    temp.SectorId = item.SectorId;
                }
                else
                {
                    _context.ImportRecipientSectorMap.Add(item);
                }
            }
            await _context.SaveChangesAsync();
        }
        public async Task<List<ImportRecipientSectorMap>> GetSectorMaps()
        {
            return await _context.ImportRecipientSectorMap.ToListAsync();
        }

        public async Task<List<TempRecipientSource>> GetTempRecipient(ECCSAModuleFlag module)
        {
            return await _context.TempRecipients.Where(s => s.Owner == module).ToListAsync();
        }
    }
}