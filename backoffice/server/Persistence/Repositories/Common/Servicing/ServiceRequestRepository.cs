using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Model.Trade;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing
{
    public class ServiceRequestRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        private readonly NotificationRepository _repository;
        private readonly InvoiceRepository _invoiceRepository;
        public ServiceRequestRepository(EccsaContext context
            , EccsaUserContext userContext
            , NotificationRepository repository
            , InvoiceRepository invoiceRepository)
        {
            _context = context;
            _userContext = userContext;
            _repository = repository;
            _invoiceRepository = invoiceRepository;
        }

        public IQueryable<ServiceRequest> ServiceRequests(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ServiceRequests.Include(x => x.ServiceType)
;
        }
        public List<ServiceRequest> UserServiceRequests(Guid userId, int status)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var list = _context.ServiceRequests.Include(x => x.ServiceType).
                Include(x => x.Responses)
                .Where(r => (r.RequestingUserId == userId || (r.ServiceType.IsArbitration && r.OpponentUserId == userId))
                && !r.ServiceType.IsResourceServiceType &&
                (status == -1 ||
                (status == 1 && r.RequestStatus != WorkflowStateEnum.Closed) ||
                (status == 0 && r.RequestStatus == WorkflowStateEnum.Closed)
                )).ToList();
            list.ForEach(s => s.ServiceType = new ServiceType
            {
                NameEng = s.ServiceType.NameEng,
                NameLocal = s.ServiceType.NameLocal,
                IsArbitration = s.ServiceType.IsArbitration
            });

            foreach (var req in list)
            {
                if (!req.ServiceType.IsArbitration) continue;
                var rqs = _context.ServiceRequirements.Where(r => r.ServiceTypeId == req.ServiceTypeId).ToList();
                foreach (var res in req.Responses)
                {
                    var rq = rqs.FirstOrDefault(r => r.Id == res.RequirementId);
                    res.Requirement = new ServiceRequirement { IsForOpponent = rq.IsForOpponent };
                }
                req.IsOpponent = req.RequestingUserId != userId;
                req.Responses = req.Responses.Where(s => s.Requirement.IsForOpponent == (req.RequestingUserId != userId)).ToList();
            }

            var cerOfOriginServiceType = _context.ServiceTypes.FirstOrDefault(s => s.Id == CertificateOfOrigin.CertificateOfOriginId);
            var userCerOfOrigins = _context.CertificateOfOrigins.Where(s => s.TraderTIN == user.TIN &&
            (status == -1 ||
                (status == 1 && s.Status != WorkflowStateEnum.Closed) ||
                (status == 0 && s.Status == WorkflowStateEnum.Closed))).ToList();
            try
            {
                list.AddRange(userCerOfOrigins.Select(s => new ServiceRequest
                {
                    Id = Guid.NewGuid(),
                    CerOfOriginId = s.Id,
                    ServiceTypeId = CertificateOfOrigin.CertificateOfOriginId,
                    RequestStatus = s.Status,
                    RequestDate = s.RegDate.Length == 8 ?
                        new DateTime(int.Parse(s.RegDate.Substring(0, 4)),
                        int.Parse(s.RegDate.Substring(4, 2)),
                        int.Parse(s.RegDate.Substring(6))) : DateTime.MinValue,
                    ServiceType = new ServiceType
                    {
                        NameEng = cerOfOriginServiceType.NameEng,
                        NameLocal = cerOfOriginServiceType.NameLocal,
                    }
                }).ToList());
            }
            catch { }

            return list;
        }

        public List<ServiceRequest> UserResourceRequests(Guid userId, int status)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var list = _context.ServiceRequests.Include(x => x.ServiceType).
                Include(x => x.Responses)
                .Where(r => r.RequestingUserId == userId && r.ServiceType.IsResourceServiceType &&
                (status == -1 ||
                (status == 1 && r.RequestStatus != WorkflowStateEnum.Closed) ||
                (status == 0 && r.RequestStatus == WorkflowStateEnum.Closed)
                )).ToList();
            list.ForEach(s => s.ServiceType = new ServiceType
            {
                NameEng = s.ServiceType.NameEng,
                NameLocal = s.ServiceType.NameLocal,
            });

            return list;
        }
        public async Task<ServiceRequirementResponse> UserResponseById(Guid userId, Guid id)
        {
            var response = await _context.ServiceRequirementResponses.FirstOrDefaultAsync(r => r.Id == id &&
            (r.ServiceRequest.RequestingUserId == userId || (r.ServiceRequest.ServiceType.IsArbitration && r.ServiceRequest.OpponentUserId == userId)));
            return response;
        }

        public async Task<ServiceRequest> UserServiceRequestForNotification(Guid userId, Guid id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.ServiceType)
                .FirstOrDefaultAsync(i => i.Id == id &&
                                          (i.RequestingUserId == userId || (i.ServiceType.IsArbitration && i.OpponentUserId == userId)));
            var users = await _userContext.Users.Where(u => u.AssociationId == Constants.ECCSA && u.AssignedModules > 0).ToListAsync();
            request.ServiceType.WorkerDetails = new List<ServiceTypeWorkerDetail>();
            foreach (var worker in request.ServiceType.WorkerList)
            {
                var user = users.FirstOrDefault(u => u.Id == worker.UserId);
                if (user == null) continue;
                request.ServiceType.WorkerDetails.Add(new ServiceTypeWorkerDetail
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    MobileNo = string.IsNullOrEmpty(user.MobileNo) ? user.PhoneNumber : user.MobileNo,
                });
            }
            return request;
        }

        public async Task<ServiceRequest> UserServiceRequest(Guid userId, Guid id)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            var request = await _context.ServiceRequests
                    .Include(x => x.Responses)
                    .ThenInclude(x => x.Requirement)
                    .Include(x => x.Followups)
                    .Include(x => x.Attachments)
                    .FirstOrDefaultAsync(i => i.Id == id &&
                    (i.RequestingUserId == userId || (i.ServiceType.IsArbitration && i.OpponentUserId == userId)));
            if (request != null)
            {
                request.IsOpponent = request.RequestingUserId != userId;
                var invoice = await _invoiceRepository.InvoiceByNo(request.InvoiceNo, request.Id);
                if (invoice != null)
                {
                    request.Invoice = new Invoice
                    {
                        TelePayUrl = invoice.TelePayUrl,
                        OutTradeNo = invoice.OutTradeNo,
                        Settlments = invoice.Settlments.Select(s => new Settlment
                        {
                            Id = s.Id,
                            ReceiptNo = s.ReceiptNo,
                            AmountConfirmed = s.AmountConfirmed,
                            Extension = s.Extension,
                            Status = s.Status
                        }).ToList()
                    };
                    request.RemainingAmount = invoice.DueRemaining;
                }
                foreach (var res in request.Responses.Where(r => r.Requirement.RequirementType == RequirementTypeEnum.Invoice))
                {
                    var resInvoice = await _invoiceRepository.InvoiceByNo(res.InvoiceNo, res.Id);
                    if (resInvoice != null)
                    {
                        res.Invoice = new Invoice
                        {
                            TelePayUrl = resInvoice.TelePayUrl,
                            OutTradeNo = resInvoice.OutTradeNo,
                            Settlments = resInvoice.Settlments.Select(s => new Settlment
                            {
                                Id = s.Id,
                                ReceiptNo = s.ReceiptNo,
                                AmountConfirmed = s.AmountConfirmed,
                                Extension = s.Extension,
                                Status = s.Status
                            }).ToList()
                        };
                        res.RemainingAmount = resInvoice.DueRemaining;
                    }
                }
                ///
                if (_context.ServiceTypes.Any(s => s.Id == request.ServiceTypeId && s.IsResourceServiceType))
                {
                    request.Resource = await _context.Resources.FirstOrDefaultAsync(r => r.Id == request.ServiceTypeId);
                    if (request.Resource != null && request.Resource.DigitalCost.HasValue && request.Resource.DigitalCost.Value > 0.01m)
                    {
                        _context.Entry(request.Resource).State = EntityState.Detached;
                        request.Resource.Extension = "";
                        request.Resource.HasUrlForUserRequest = !string.IsNullOrWhiteSpace(request.Resource.Url);
                        request.Resource.Url = "";
                    }
                }
                ///
                if (_context.ServiceTypes.Any(s => s.Id == request.ServiceTypeId && s.IsArbitration))
                {
                    var isOpponenet = request.RequestingUserId != userId;
                    request.Responses = request.Responses.Where(s => s.Requirement.IsForOpponent == isOpponenet).ToList();
                    request.Attachments = request.Attachments.Where(a => a.IsOpponent == isOpponenet).ToList();
                }
            }

            return request;
        }
        public async Task<ServiceRequest> UserServiceRequestForInvoice(Guid userId, Guid id)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            var request = await _context.ServiceRequests
                    .FirstOrDefaultAsync(i => i.Id == id && i.RequestingUserId == userId);

            return request;
        }
        public async Task<CertificateOfOrigin> UserCertificateOfOrigin(Guid userId, long id)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            var request = await _context.CertificateOfOrigins
                    .Include(x => x.CommodityList)
                    .FirstOrDefaultAsync(i => i.Id == id && i.TraderTIN == user.TIN);

            return request;
        }
        public async Task RecommendApprove(Guid requestId, string extension, string code, Guid userId)
        {
            var request = await _context.ServiceRequests
                    .FirstOrDefaultAsync(i => i.Id == requestId);
            if (request.RequestStatus != WorkflowStateEnum.UnderReview)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.DraftPrepared;
            request.Extension = extension;
            request.Signature = code;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            request.AuthorizedDate = DateTime.UtcNow;
            _context.ServiceRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAttachment(Guid requestId, string extension, string code, Guid userId)
        {
            var request = await _context.ServiceRequests
                    .FirstOrDefaultAsync(i => i.Id == requestId);
            request.Extension = extension;
            request.Signature = code;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task RecommendRejection(Guid requestId, String remark, Guid userId)
        {
            var request = await _context.ServiceRequests
                    .FirstOrDefaultAsync(i => i.Id == requestId);
            if (request.RequestStatus != WorkflowStateEnum.UnderReview)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.RecommendRejection;
            request.Remark = remark;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            request.AuthorizedDate = DateTime.UtcNow;
            _context.ServiceRequests.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task ApproveAcceptRequest(Guid requestId, Guid userId)
        {
            var approver = await _userContext.Users.FirstOrDefaultAsync(f => f.Id == userId);
            var request = await _context.ServiceRequests.Include(r => r.ServiceType)
                    .FirstOrDefaultAsync(i => i.Id == requestId);
            if (request.RequestStatus != WorkflowStateEnum.DraftPrepared)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.Accepted;
            request.ApproverName = approver.FullName;
            request.ApproverTitle = approver.Title ?? "";
            request.ApprovedDate = DateTime.UtcNow;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(request);
            ///
            if (request.ServiceType.ServiceOwner == ECCSAModuleFlag.Academy)
            {
                var training = _context.Trainings.FirstOrDefault(t => t.ServiceTypeId == request.ServiceTypeId);
                if (training != null)
                {
                    await CreateRegistration(request, training, userId);
                }
            }
            ///
            await _context.SaveChangesAsync();
        }
        private async Task CreateRegistration(ServiceRequest request, Training training, Guid userId)
        {
            Registration data = new()
            {
                Id = Guid.NewGuid()
            ,
                TrainingId = training.Id
            ,
                LoginId = request.RequestingUserId
            //,
            //TrainingSponsorId = 
            ,
                FullName = request.FullName
            ,
                EmailAddress = request.EmailAddress
            ,
                MobileNo = request.MobileNo
            ,
                RegistrationDate = request.RequestDate
            ,
                RegistrationStatus = RegistrationStatus.Accepted
            ,
                DiscountCategory = 0
            ,
                CreatedBy = userId
            ,
                CreateStamp = DateTime.UtcNow
            ,
                UpdatedBy = userId
            ,
                UpdateStamp = DateTime.UtcNow
            ,
                RequestId = request.Id

            };
            await _context.Registrations.AddAsync(data);
        }
        public async Task DeclineAcceptRequest(Guid requestId, Guid userId, String remark)
        {
            var request = await _context.ServiceRequests
                    .FirstOrDefaultAsync(i => i.Id == requestId);
            if (request.RequestStatus != WorkflowStateEnum.DraftPrepared)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.UnderReview;
            request.Remark = remark;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task ApproveRejectionRequest(Guid requestId, Guid userId, String remark)
        {
            var approver = await _userContext.Users.FirstOrDefaultAsync(f => f.Id == userId);
            var request = await _context.ServiceRequests
                    .FirstOrDefaultAsync(i => i.Id == requestId);
            if (request.RequestStatus != WorkflowStateEnum.RecommendRejection)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.Rejected;
            request.Remark = remark;
            request.ApproverName = approver.FullName;
            request.ApproverTitle = approver.Title ?? "";
            request.ApprovedDate = DateTime.UtcNow;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeclineRejectionRequest(Guid requestId, Guid userId, String remark)
        {
            var request = await _context.ServiceRequests
                    .FirstOrDefaultAsync(i => i.Id == requestId);
            if (request.RequestStatus != WorkflowStateEnum.RecommendRejection)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.UnderReview;
            request.Remark = remark;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(request);
            await _context.SaveChangesAsync();
        }
        public IQueryable<ServiceRequirementResponse> Responses(Guid userId, Guid id)
        {
            return _context.ServiceRequirementResponses.
                    Where(r =>
                    (r.ServiceRequest.RequestingUserId == userId ||
                    (r.ServiceRequest.ServiceType.IsArbitration && r.ServiceRequest.OpponentUserId == userId))
                    && r.ServiceRequestId == id);
        }

        public ServiceRequirementResponse Response(Guid userId, Guid id)
        {
            return _context.ServiceRequirementResponses.Include(r => r.Requirement).
                    FirstOrDefault(r =>
                    (r.ServiceRequest.RequestingUserId == userId ||
                    r.Requirement.IsForOpponent && r.ServiceRequest.OpponentUserId == userId)
                    && r.Id == id);
        }

        public async Task<List<ServiceFollowup>> AppointmentList(ECCSAModuleFlag module, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return await _context.ServiceFollowups.Include(s => s.ServiceRequest.ServiceType).Where(f =>
                    f.ServiceRequest.ServiceType.ServiceOwner.HasFlag(module) && f.StartDate.Date >= fromDate.Date &&
                    f.StartDate.Date <= toDate.Date).ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<ServiceFollowup>();
            }
        }
        public async Task<List<ServiceRequest>> Services(ECCSAModuleFlag subSystem)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ServiceRequests
                .Include(x => x.ServiceType)
                .Where(x =>
                 //(//x.RequestStatus != WorkflowStateEnum.NotSet &&
                 //x.RequestStatus != WorkflowStateEnum.Accepted) &&
                 x.ServiceType.ServiceOwner == subSystem).ToListAsync();

        }

        public async Task<List<ServiceRequest>> ServicesByServiceType(ECCSAModuleFlag subSystem, Guid serviceTypeId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ServiceRequests
                .Include(x => x.ServiceType)
                .Where(x =>
                (x.ServiceTypeId == serviceTypeId) &&
                 x.ServiceType.ServiceOwner == subSystem).ToListAsync();

        }

        public async Task<List<ServiceRequest>> Services(ECCSAModuleFlag subSystem, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return await _context.ServiceRequests
                        .Include(x => x.ServiceType)
                        .Where(x => x.ServiceType.ServiceOwner == subSystem).ToListAsync();
                }

                return await _context.ServiceRequests
                    .Include(x => x.ServiceType)
                    .Where(x => (x.TIN.Contains(key) || x.TrackingNo.Contains(key) || x.InvoiceNo.Contains(key)) &&

                                x.ServiceType.ServiceOwner == subSystem).ToListAsync();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw;
            }

        }

        public User User(Guid id)
        {
            return _userContext.Users.FirstOrDefault(u => u.Id == id);
        }
        public async Task<List<User>> Users(ECCSAModuleFlag subSystem)
        {
            var roles = Role.GetRoles(subSystem);
            var tmp = await _userContext.Users.Include(u => u.UserRoles).Where(u => u.AssignedModules > 0 && u.AssignedModules.HasFlag(subSystem)).ToListAsync();
            var users = new List<User>();
            foreach (var role in roles)
            {
                foreach (var user in tmp)
                {
                    if (user.UserRoles.Any(r => r.RoleId == role.Id))
                    {
                        users.Add(user);
                    }

                }
            }
            return users;


        }

        public async Task<List<ServiceRequest>> Services(ECCSAModuleFlag subSystem, WorkflowStateEnum status)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ServiceRequests
                .Include(x => x.ServiceType)
                .Where(x => x.RequestStatus == status &&
                 x.ServiceType.ServiceOwner == subSystem).ToListAsync();

        }
        public async Task<ServiceRequest> UserServiceRequestByServiceTypeId(Guid id, Guid userId)
        {
            var request = await _context.ServiceRequests.FirstOrDefaultAsync(s => s.ServiceTypeId == id && s.RequestingUserId == userId);
            return request;
        }
        public async Task<ServiceRequest> ServiceRequestForPrint(Guid id, Guid userId)
        {
            var request = await _context.ServiceRequests
                    .Include(x => x.Responses)
                    .ThenInclude(x => x.Requirement)
                    .Include(x => x.ServiceType)
                    .ThenInclude(x => x.ServiceRequirements)
                    .Include("ServiceType.ServiceTypeReports").AsNoTracking()
                    .FirstOrDefaultAsync(i => i.Id == id && i.RequestingUserId == userId);
            var tabularRequirements = request.ServiceType.ServiceRequirements.
                Where(s => s.InputMethod == ServiceInputTypeEnum.Tabular);
            if (tabularRequirements.Any())
            {
                //request.ServiceType.TabularRequirements = (await _context.TabularDataSources.
                //    IncludeFilter(l => l.Details.Where(v => v.Status == StatusEnum.Active))
                //    .ToListAsync())
                //    .Where(l => tabularRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();
                request.ServiceType.TabularRequirements = (await _context.TabularDataSources.
                    Include(l => l.Details)
                    .ToListAsync())
                    .Where(l => tabularRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();
                foreach (var item in request.ServiceType.TabularRequirements)
                {
                    item.Details = item.Details.Where(d => d.Status == StatusEnum.Active).ToList();
                }
            }
            return request;
        }
        public async Task<ServiceRequest> ServiceRequestForPreview(Guid id)
        {
            var request = await _context.ServiceRequests
                    .Include(x => x.Responses)
                    .Include(x => x.ServiceType)
                    .ThenInclude(x => x.ServiceRequirements)
                    .Include("ServiceType.ServiceTypeReports")
                    .FirstOrDefaultAsync(i => i.Id == id);
            var tabularRequirements = request.ServiceType.ServiceRequirements.
                Where(s => s.InputMethod == ServiceInputTypeEnum.Tabular);
            if (tabularRequirements.Any())
            {
                //request.ServiceType.TabularRequirements = (await _context.TabularDataSources.
                //    IncludeFilter(l => l.Details.Where(v => v.Status == StatusEnum.Active))
                //    .ToListAsync())
                //    .Where(l => tabularRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();

                request.ServiceType.TabularRequirements = (await _context.TabularDataSources.
                    Include(l => l.Details)
                    .ToListAsync())
                    .Where(l => tabularRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();
                foreach (var item in request.ServiceType.TabularRequirements)
                {
                    item.Details = item.Details.Where(d => d.Status == StatusEnum.Active).ToList();
                }
            }
            return request;
        }
        public async Task<ServiceRequest> ServiceRequestById(Guid id, Guid userId)
        {
            var request = await _context.ServiceRequests
                    .Include(x => x.Responses)
                    .Include(x => x.Followups)
                    .Include(x => x.Attachments)
                    .FirstOrDefaultAsync(i => i.Id == id);
            if (request != null)
            {
                var serviceType = await _context.ServiceTypes
                                .Include(x => x.ServiceRequirements)
                                .Include(x => x.ServiceTypeReports)
                                .FirstOrDefaultAsync(x => x.Id == request.ServiceTypeId);
                request.ServiceType = serviceType;
                foreach (var res in request.Responses)
                {
                    res.Requirement = serviceType.ServiceRequirements.First(r => r.Id == res.RequirementId);
                    if (res.Requirement.RequirementType == RequirementTypeEnum.Invoice)
                    {
                        if (!string.IsNullOrEmpty(res.InvoiceNo))
                        {
                            var invoice = await _invoiceRepository.InvoiceByNo(res.InvoiceNo, res.Id);
                            //res.ReceiptAmount = String.Empty;
                            //res.InvoiceAmount = String.Empty;

                            if (invoice != null)
                            {
                                res.Invoice = invoice;
                                //if (!string.IsNullOrEmpty(res.ReceiptNo))
                                //{
                                //    res.ReceiptAmount = $"{invoice.TotalPaid.ToString("#,##0.00")}Birr";
                                //}
                                //res.InvoiceAmount = $"{invoice.TotalAmount.ToString("#,##0.00")}Birr";
                            }
                        }
                    }
                }

                request.Responses = request.Responses.OrderBy(r => r.Requirement.OrderNo).ToList();
                request.Followups = request.Followups.OrderBy(f => f.StartDateTime).ToList();
                if (!string.IsNullOrEmpty(request.InvoiceNo))
                {
                    var invoice = await _invoiceRepository.InvoiceByNo(request.InvoiceNo, request.Id);
                    request.ReceiptAmount = String.Empty;
                    request.InvoiceAmount = String.Empty;

                    if (invoice != null)
                    {
                        request.Invoice = invoice;
                        if (!string.IsNullOrEmpty(request.ReceiptNo))
                        {
                            request.ReceiptAmount = $"{invoice.TotalPaid.ToString("#,##0.00")}Birr";

                        }
                        request.InvoiceAmount = $"{invoice.TotalAmount.ToString("#,##0.00")}Birr";
                    }
                }

                var tabularRequirements = request.ServiceType.ServiceRequirements.
                Where(s => s.InputMethod == ServiceInputTypeEnum.Tabular);
                if (tabularRequirements.Any())
                {
                    //request.ServiceType.TabularRequirements = (await _context.TabularDataSources.
                    //    IncludeFilter(l => l.Details.Where(v => v.Status == StatusEnum.Active))
                    //    .ToListAsync())
                    //    .Where(l => tabularRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();
                    request.ServiceType.TabularRequirements = (await _context.TabularDataSources.
                    Include(l => l.Details)
                    .ToListAsync())
                    .Where(l => tabularRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();
                    foreach (var item in request.ServiceType.TabularRequirements)
                    {
                        item.Details = item.Details.Where(d => d.Status == StatusEnum.Active).ToList();
                    }
                }
            }
            return request;
        }

        public async Task<ServiceRequest> ServiceRequestByInvoice(string invoiceNo, Guid id)
        {
            var request = await _context.ServiceRequests
                    .FirstOrDefaultAsync(i => i.InvoiceNo == invoiceNo && i.Id == id);
            return request;
        }

        public async Task<ServiceRequirementResponse> ServiceRequestResponseByInvoice(string invoiceNo, Guid id)
        {
            var response = await _context.ServiceRequirementResponses
                    .FirstOrDefaultAsync(i => i.InvoiceNo == invoiceNo && i.Id == id);
            return response;
        }
        public async Task Delete(ServiceRequest data)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.ServiceRequests.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        private string GenerateTrackingNo(int count, int moduleId)
        {
            var formattedCount = $"0000{count + 1}";
            var random = $"00{(new Random()).Next(1, 99)}";
            var mod = $"000{moduleId}";
            mod = mod[^3..];
            formattedCount = formattedCount[^5..];
            random = random[^2..];
            long l = long.Parse($"{DateTime.UtcNow:yyMMdd}{formattedCount}{mod}{random}");
            var no = BaseConverter.GetCode(l);
            return no;
        }
        public async Task RecordInvoice(Guid requestId, string invoiceNo, decimal amount, YesNoEnum asMember, Guid userId)
        {

            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            tmp.InvoiceNo = invoiceNo;
            tmp.UpdatedBy = userId;
            tmp.Amount = 0;
            tmp.AsMember = asMember;
            if (invoiceNo == Invoice.BlankInovice.InvoiceNo)
            {
                tmp.ReceiptNo = Invoice.BlankInovice.InvoiceNo;
                tmp.RemainingAmount = decimal.Zero;
                tmp.PaymentStatus = PaymentStatusEnum.Accepted;
                tmp.InvoiceAmount = "0.00Birr";
                tmp.ReceiptAmount = "0.00Birr";
                tmp.PrintReceipt = true;
            }
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task RecordRequirementInvoice(Guid requestId
            , Guid responseId
            , string invoiceNo
            , decimal amount
            , YesNoEnum asMember
            , Guid userId)
        {

            var tmp = await _context.ServiceRequirementResponses
                .FirstOrDefaultAsync(r => r.ServiceRequestId == requestId && r.Id == responseId);
            if (tmp == null)
                return;
            tmp.InvoiceNo = invoiceNo;
            tmp.InvoiceAmount = amount;
            tmp.InvoiceDate = DateTime.UtcNow;
            _context.ServiceRequirementResponses.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task RecordRequirementUserPayment(Guid requestId
            , Guid requirementId
            , string invoiceNo
            , string receiptNo
            , PaymentMethodEnum paymentMethod
            , string remark
            , Guid userId)
        {
            var tmp = await _context.ServiceRequirementResponses
                .FirstOrDefaultAsync(r => r.ServiceRequestId == requestId && r.RequirementId == requirementId);
            if (tmp == null)
                return;
            if (tmp.InvoiceNo != invoiceNo)
                return;
            tmp.ReceiptNo = receiptNo;
            // tmp.AmountPaid = amount;
            tmp.PaymentMethod = paymentMethod;
            tmp.DatePaid = DateTime.UtcNow;
            tmp.Remark = String.IsNullOrEmpty(remark) ? string.Empty : remark;
            tmp.ReviewStatus = DocumentReviewStatusEnum.Submitted;
            tmp.PaymentStatus = PaymentStatusEnum.NotSet;
            _context.ServiceRequirementResponses.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task RecordPayment(Guid requestId, string invoiceNo, string receiptNo, PaymentMethodEnum paymentMethod, string remark, Guid userId)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (tmp.InvoiceNo != invoiceNo)
                return;
            tmp.ReceiptNo = receiptNo;
            tmp.PaymentMethod = paymentMethod;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            tmp.Remark = String.IsNullOrEmpty(remark) ? string.Empty : remark;
            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task RecordUserPayment(Guid requestId, string invoiceNo, string receiptNo, PaymentMethodEnum paymentMethod, string remark, Guid userId)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (tmp.InvoiceNo != invoiceNo)
                return;
            tmp.ReceiptNo = receiptNo;
            tmp.PaymentMethod = paymentMethod;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            tmp.PaymentStatus = PaymentStatusEnum.NotSet;
            tmp.Remark = String.IsNullOrEmpty(remark) ? string.Empty : remark;
            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task ApproveReceiptPrint(Guid requestId, decimal totalPaid,
            PaymentStatusEnum paymentStatus, Guid userId)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            //if (invoiceAmount - confirmedAmount > 0.01M)
            //    return;
            tmp.PrintReceipt = paymentStatus == PaymentStatusEnum.Accepted;
            tmp.PaymentStatus = paymentStatus;
            tmp.UpdatedBy = userId;
            tmp.Amount = totalPaid;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveRequirementReceiptPrint(Guid requestId
            , Guid responseId, decimal totalPaid, PaymentStatusEnum paymentStatus, Guid userId)
        {
            var tmp = await _context.ServiceRequirementResponses
                .FirstOrDefaultAsync(r => r.ServiceRequestId == requestId && r.Id == responseId);
            if (tmp == null)
                return;
            //if (invoiceAmount - confirmedAmount > 0.01M)
            //    return;
            tmp.PrintReceipt = paymentStatus == PaymentStatusEnum.Accepted;
            tmp.PaymentStatus = paymentStatus;
            tmp.AmountPaid = totalPaid;
            _context.ServiceRequirementResponses.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task StartProcessing(Guid id, Guid userId)
        {
            var theUser = await _userContext.Users.FirstOrDefaultAsync(o => o.Id == userId);
            if (theUser == null)
                return;
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null)
                return;
            if (!tmp.UnderReview)
                return;
            var operators = await _context.OperatorHistory.Where(o => o.ServiceRequestId == id && !o.EndDate.HasValue).ToListAsync();
            tmp.OperatorName = theUser.FullName;
            tmp.OperatorTitle = theUser.Title ?? "";
            foreach (var op in operators.Where(h => !h.EndDate.HasValue))
            {
                op.EndDate = DateTime.UtcNow;
                _context.OperatorHistory.Update(op);
            }
            _context.OperatorHistory.Add(new OperatorHistory
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                OperatorName = tmp.OperatorName,
                ServiceRequestId = id,
                UserId = userId
            });
            tmp.RequestStatus = WorkflowStateEnum.UnderReview;
            var lst = await _context.ServiceRequirementResponses.Where(r => r.ServiceRequestId == id && r.ReviewStatus == DocumentReviewStatusEnum.NotSet).ToListAsync();
            foreach (var res in lst)
            {
                res.ReviewStatus = DocumentReviewStatusEnum.Rejected;
                _context.ServiceRequirementResponses.Update(res);
            }
            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task AssignUser(Guid requestId, Guid operatorId, Guid userId)
        {
            var theUser = await _userContext.Users.FirstOrDefaultAsync(o => o.Id == operatorId);
            if (theUser == null)
                return;
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (!tmp.UnderReview)
                return;
            var operators = await _context.OperatorHistory.Where(o => o.ServiceRequestId == requestId && !o.EndDate.HasValue).ToListAsync();
            tmp.OperatorName = theUser.FullName;
            tmp.OperatorTitle = theUser.Title ?? "";
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            foreach (var op in operators.Where(h => !h.EndDate.HasValue))
            {
                op.EndDate = DateTime.UtcNow;
                _context.OperatorHistory.Update(op);
            }
            _context.OperatorHistory.Add(new OperatorHistory
            {
                Id = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                OperatorName = tmp.OperatorName,
                ServiceRequestId = requestId,
                UserId = userId
            });

            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task ReviewRequirement(Guid id, Guid requestId, DocumentReviewStatusEnum status, string remark, Guid userId)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (tmp.RequestStatus == WorkflowStateEnum.NotSet)
                return;
            var response = await _context.ServiceRequirementResponses.Include(r => r.Requirement).FirstOrDefaultAsync(r => r.Id == id && r.ServiceRequestId == requestId);
            response.ReviewStatus = status;
            response.ReviewedBy = userId;
            response.DateReviewed = DateTime.UtcNow;
            response.Remark = remark ?? string.Empty;
            _context.ServiceRequirementResponses.Update(response);
            await _context.SaveChangesAsync();
        }
        public async Task SubmitRequest(Guid requestId, Guid userId)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (tmp.RequestStatus != WorkflowStateEnum.NotSet)
                return;
            tmp.RequestStatus = WorkflowStateEnum.Submitted;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(tmp);
            var lst = await _context.ServiceRequirementResponses.Where(r => r.ServiceRequestId == requestId).ToListAsync();
            foreach (var res in lst)
            {
                if (res.ReviewStatus != DocumentReviewStatusEnum.NotSet) continue;
                if (res.ReviewStatus != DocumentReviewStatusEnum.NotSet) continue;
                if (res.Requirement.InputMethod == ServiceInputTypeEnum.Payment) continue;
                if (IsNullOrEmpty(res))
                    res.ReviewStatus = DocumentReviewStatusEnum.Submitted;
                else
                    res.ReviewStatus = DocumentReviewStatusEnum.NotSet;
                _context.ServiceRequirementResponses.Update(res);
            }
            await _context.SaveChangesAsync();
        }
        public async Task SetOpponentMember(Guid requestId, Guid opponentUserId, Guid userId)
        {
            var tmp = await _context.ServiceRequests.Include(s => s.ServiceType).FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (!tmp.ServiceType.IsArbitration)
                return;
            if (tmp.RequestStatus != WorkflowStateEnum.Submitted && tmp.RequestStatus != WorkflowStateEnum.UnderReview)
                return;
            var opponentUser = await _userContext.Users.FirstOrDefaultAsync(s => s.Id == opponentUserId && s.AssignedModules == 0);
            if (opponentUser == null)
            {
                throw new Exception("Opponent member Not found!");
            }
            tmp.OpponentUserId = opponentUserId;
            tmp.OpponentFullName = opponentUser.FullName;
            tmp.OpponentEmail = opponentUser.Email;
            tmp.OpponentMobileNo = opponentUser.MobileNo;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }
        private static bool IsNullOrEmpty(ServiceRequirementResponse response)
        {
            if (response.DecimalResponse.HasValue) return false;
            if (response.DateResponse.HasValue) return false;
            if (response.IntResponse.HasValue) return false;
            if (response.LookupValueId.HasValue) return false;
            if (response.TabularResponse != null && response.TabularResponse.Count > 0) return false;
            if (!string.IsNullOrWhiteSpace(response.TextResponse)) return false;
            return true;
        }
        public async Task SubmitUserRequest(Guid requestId, Guid userId)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId && r.RequestingUserId == userId);
            if (tmp == null)
                return;
            if (tmp.RequestStatus != WorkflowStateEnum.NotSet)
                return;
            tmp.RequestStatus = WorkflowStateEnum.Submitted;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            var lst = await _context.ServiceRequirementResponses.Include(r => r.Requirement).Where(r => r.ServiceRequestId == requestId).ToListAsync();
            foreach (var res in lst)
            {
                if (res.Requirement.InputMethod == ServiceInputTypeEnum.TradeAgreement && res.LookupValueId.HasValue)
                {
                    var c = await _context.AgreementCommodities.Include(i => i.Agreement).FirstOrDefaultAsync(i => i.Id == res.LookupValueId.Value);
                    var a = c.Agreement;
                    res.TextResponse = $"Agreement No: {a.AgreementNo}; LicenseNo: {a.LicenseNo}; Consignee: {a.Consignee}; Country: {a.Country}; City:{a.City}; Address: {a.Address}; \r\n";
                    res.TextResponse += $"Type of Carriage: {a.TypeOfCarriage}; Country of Destination: {a.CountyOfDestination};  Port of Destination: {a.PortOfDestination};  \r\nPort of Departure: {a.PortOfDeparture} ";
                    res.TextResponse += $" Name of Carrier: {a.NameOfCarrier}; Term of Delivery: {a.TermOfDelivery};\r\n Commodity Name: {c.Name}; HS Code: {c.HSCode}";
                }
                if (res.ReviewStatus != DocumentReviewStatusEnum.NotSet) continue;
                if (res.Requirement.InputMethod == ServiceInputTypeEnum.Payment) continue;
                if (!IsNullOrEmpty(res))
                {
                    if (res.Requirement.ApproveOnSubmit)
                    {
                        res.ReviewStatus = DocumentReviewStatusEnum.Accepted;
                    }
                    else
                    {
                        res.ReviewStatus = DocumentReviewStatusEnum.Submitted;
                    }
                }
                else
                    res.ReviewStatus = DocumentReviewStatusEnum.NotSet;
                _context.ServiceRequirementResponses.Update(res);
            }
            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task CloseUserRequest(Guid requestId, Guid userId)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId && r.RequestingUserId == userId);
            if (tmp == null)
                return;
            if (tmp.RequestStatus != WorkflowStateEnum.Accepted && tmp.RequestStatus != WorkflowStateEnum.Rejected)
                return;
            tmp.RequestStatus = WorkflowStateEnum.Closed;
            _context.ServiceRequests.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task CloseUserCertificateOfOrigin(long id, Guid userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            var tmp = await _context.CertificateOfOrigins
                    .FirstOrDefaultAsync(i => i.Id == id && i.TraderTIN == user.TIN);
            if (tmp == null)
                return;
            if (tmp.Status != WorkflowStateEnum.Accepted)
                return;
            tmp.Status = WorkflowStateEnum.Closed;
            _context.CertificateOfOrigins.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task SubmitUserRequestResponse(Guid responseId, Guid userId)
        {
            var res = await _context.ServiceRequirementResponses.FirstOrDefaultAsync(r => r.Id == responseId
            && (r.ServiceRequest.RequestingUserId == userId ||
            (r.ServiceRequest.ServiceType.IsArbitration && r.ServiceRequest.OpponentUserId == userId && r.Requirement.IsForOpponent)));
            if (res.ReviewStatus != DocumentReviewStatusEnum.NotSet && res.ReviewStatus != DocumentReviewStatusEnum.Rejected) return;
            res.ReviewStatus = DocumentReviewStatusEnum.Submitted;
            _context.ServiceRequirementResponses.Update(res);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeRequestStatus(Guid requestId
            , WorkflowStateEnum status
            , string code
            , string extension
            , String Remark
            , Guid userId)
        {
            var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;

            tmp.RequestStatus = status;
            tmp.Signature = string.IsNullOrEmpty(code) ? string.Empty : code;
            tmp.Remark = string.IsNullOrEmpty(Remark) ? string.Empty : Remark;
            tmp.Extension = string.IsNullOrEmpty(extension) ? string.Empty : extension;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceRequests.Update(tmp);

            await _context.SaveChangesAsync();
        }
        public async Task ChangeFollowupStatus(ServiceFollowup data
            , Guid userId)
        {
            var tmp = await _context.ServiceFollowups.FirstOrDefaultAsync(r => r.Id == data.Id);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (tmp == null)
            {
                return;
            }
            tmp.Status = data.Status;
            await _context.SaveChangesAsync();
        }

        public async Task Save(ServiceFollowup data
            , Guid userId)
        {
            var tmp = await _context.ServiceFollowups.FirstOrDefaultAsync(r => r.Id == data.Id);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (tmp == null)
            {
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.ServiceFollowups.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFollowup(Guid id, Guid serviceId, Guid userId, string filePath)
        {
            var tmp = await _context.ServiceFollowups.FirstOrDefaultAsync(r => r.Id == id && r.ServiceId == serviceId);

            if (tmp == null)
                return;
            _context.ServiceFollowups.Remove(tmp);
            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(tmp.Extension))
            {
                System.IO.File.Delete(filePath + tmp.Extension);
            }
        }

        public async Task<ServiceFollowup> Followup(Guid id)
        {
            return await _context.ServiceFollowups.FirstOrDefaultAsync(r => r.Id == id);

        }
        public async Task Save(ServiceRequest data, Guid userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(t => t.Id == data.ServiceTypeId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            if (serviceType == null)
            {
                throw new InvalidOperationException("Service Type not found");
            }
            var reqs = await _context.ServiceRequirements
                .Where(x => x.Status == StatusEnum.Active && x.ServiceTypeId == data.ServiceTypeId).ToListAsync();


            var Cnt = await _context.ServiceRequests.CountAsync(r => r.RequestDate == data.RequestDate.Date);

            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();

                data.TIN = user.TIN;
                data.Address = user.Address ?? "";
                data.EmailAddress = user.Email;
                data.FullName = String.IsNullOrEmpty(user.CompanyName) ? user.FullName : user.CompanyName;
                data.MobileNo = user.PhoneNumber ?? "";
                data.TrackingNo = GenerateTrackingNo(Cnt, (int)serviceType.ServiceOwner);
                data.RequestDate = DateTime.Today;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.UpdatedBy = userId;
                data.UpdateStamp = DateTime.UtcNow;
                foreach (var res in data.Responses)
                {
                    res.ServiceRequestId = data.Id;
                    res.ReviewStatus = DocumentReviewStatusEnum.NotSet;
                    res.Remark = string.Empty;
                    _context.ServiceRequirementResponses.Add(res);
                }
                foreach (var att in data.Attachments)
                {
                    att.ServiceRequestId = data.Id;
                    _context.RequestAttachments.Add(att);
                }
                _context.ServiceRequests.Add(data);
                _context.ServiceRequirementResponses.AddRange(data.Responses);
            }
            else
            {
                var tmp = await _context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Service Request Not found");
                }
                tmp.InvoiceNo = data.InvoiceNo;
                tmp.ReceiptNo = data.ReceiptNo;
                tmp.PaymentMethod = data.PaymentMethod;
                tmp.RequestStatus = data.RequestStatus;
                tmp.Description = data.Description;
                tmp.PreviousStep = data.PreviousStep;
                tmp.CurrentStep = data.CurrentStep;
                tmp.UpdatedBy = userId;
                tmp.UpdateStamp = DateTime.UtcNow;

                tmp.MobileNo = user.MobileNo;
                tmp.EmailAddress = user.Email;
                _context.ServiceRequests.Update(tmp);
            }

            await _context.SaveChangesAsync();
            await AutoGenerateInvoice(data, serviceType);
        }
        private async Task AutoGenerateInvoice(ServiceRequest request, ServiceType serviceType)
        {
            if (serviceType.RateBasis == RateBasisEnum.NotSet)
            {
                serviceType.RateBasis = RateBasisEnum.Flat;
            }
            if (serviceType.RateBasis != RateBasisEnum.Flat) return;
            if (!string.IsNullOrEmpty(request.InvoiceNo)) return;
            var simpleInvoice = new Invoice()
            {
                SubSystem = request.ServiceType.ServiceOwner,
                ReferenceId = request.Id,
                BasePrice = decimal.Round(serviceType.NonMemberServiceCharge / 1.15m, 2, MidpointRounding.ToEven),
                Discount = 0,
                TotalAmount = serviceType.NonMemberServiceCharge,
                CompanyName = request.FullName,
                TIN = request.TIN,
                Address = request.Address,
                PaymentReason = request.ServiceType.NameEng
            };
            var invoice = await _invoiceRepository.GetOrCreate(simpleInvoice, request.CreatedBy);
            await RecordInvoice(request.Id, invoice.InvoiceNo, serviceType.NonMemberServiceCharge, YesNoEnum.NotSet, request.CreatedBy);
        }
        public async Task UpdateResponse(ServiceRequest data, Guid userId)
        {
            var reqs = await _context.ServiceRequirements
                .Where(x => x.Status == StatusEnum.Active && x.ServiceTypeId == data.ServiceTypeId).ToListAsync();

            var request = await _context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == data.Id);
            var serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(s => s.Id == data.ServiceTypeId);
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            foreach (var res in data.Responses)
            {
                var savedRes = _context.ServiceRequirementResponses.Include(r => r.Requirement).
                    FirstOrDefault(r => r.ServiceRequestId == request.Id && r.RequirementId == res.RequirementId);
                if (savedRes == null || (savedRes.ReviewStatus != DocumentReviewStatusEnum.Rejected
                    && savedRes.ReviewStatus != DocumentReviewStatusEnum.NotSet)) { continue; }
                if (savedRes.Requirement.InputMethod != ServiceInputTypeEnum.Upload || !string.IsNullOrEmpty(res.TextResponse))
                {
                    savedRes.TextResponse = res.TextResponse;
                }
                savedRes.IntResponse = res.IntResponse;
                savedRes.DateResponse = res.DateResponse;
                savedRes.DecimalResponse = res.DecimalResponse;
                savedRes.TabularResponse = res.TabularResponse;
                savedRes.LookupValueId = res.LookupValueId;

                _context.ServiceRequirementResponses.Update(savedRes);
            }
            _context.ServiceRequests.Update(request);
            await _repository.SaveValue(new RecipientSource()
            {
                Id = Guid.Empty,
                Name = request.FullName,
                CompanyName = string.Empty,
                MobileNo = request.MobileNo,
                Email = request.EmailAddress,
                Owner = serviceType.ServiceOwner
            }, userId);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOptionalResponse(ServiceRequest data, Guid userId)
        {
            var request = await _context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == data.Id);
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            request.OptionalEntries = data.OptionalEntries;
            _context.ServiceRequests.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task Save(ServiceRequirementResponse data, Guid userId)
        {

            var tmp = await _context.ServiceRequirementResponses.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Response Not found");
            }
            tmp.Remark = data.Remark;
            tmp.ReviewedBy = userId;
            tmp.DateReviewed = DateTime.UtcNow;
            tmp.ReviewStatus = data.ReviewStatus;
            _context.ServiceRequirementResponses.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<RequestAttachment> RequestAttachment(Guid id)
        {
            return await _context.RequestAttachments.Include(s => s.Request)
                .ThenInclude(s => s.ServiceType).FirstOrDefaultAsync(r => r.Id == id);

        }
        public async Task<Guid> Save(RequestAttachment data
            , Guid userId)
        {
            var tmp = await _context.RequestAttachments.FirstOrDefaultAsync(r => r.Id == data.Id);
            var id = data.Id;
            if (tmp == null)
            {
                var result = _context.RequestAttachments.Add(data);
                id = result.Entity.Id;
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task DeleteAttachment(Guid id, Guid userId, string filePath)
        {
            var tmp = await _context.RequestAttachments.
                FirstOrDefaultAsync(r => r.Id == id && (r.Request.RequestingUserId == userId || (r.IsOpponent && r.Request.OpponentUserId == userId)));

            if (tmp == null)
                return;
            _context.RequestAttachments.Remove(tmp);
            await _context.SaveChangesAsync();
            if (!string.IsNullOrEmpty(tmp.Extension))
            {
                System.IO.File.Delete(filePath);
            }
        }

        public async Task<bool> IsBooked(ServiceFollowup model)
        {
            return await _context.ServiceFollowups.AnyAsync(f => f.ServiceTypeId == model.ServiceTypeId &&
            f.StartDate == model.StartDate && f.StartTime == model.StartTime &&
            f.ServiceId != model.ServiceId && f.Id != model.Id);
        }

        public async Task<ServiceRequirementResponse> GetResponseServiceType(Guid responseId, Guid userId)
        {
            var res = await _context.ServiceRequirementResponses.Include(r => r.ServiceRequest).Include(r => r.Requirement)
                .ThenInclude(r => r.ServiceType).AsNoTracking().FirstOrDefaultAsync(r => r.Id == responseId
            && (r.ServiceRequest.RequestingUserId == userId ||
            (r.ServiceRequest.ServiceType.IsArbitration && r.ServiceRequest.OpponentUserId == userId && r.Requirement.IsForOpponent)));
            if (res.ReviewStatus != DocumentReviewStatusEnum.NotSet && res.ReviewStatus != DocumentReviewStatusEnum.Rejected) return null;
            if (res?.Requirement?.ServiceType != null)
            {
                var users = await _userContext.Users.Where(u => u.AssociationId == Constants.ECCSA && u.AssignedModules > 0).ToListAsync();
                res.Requirement.ServiceType.WorkerDetails = new List<ServiceTypeWorkerDetail>();
                foreach (var worker in res.Requirement.ServiceType.WorkerList)
                {
                    var user = users.FirstOrDefault(u => u.Id == worker.UserId);
                    if (user == null) continue;
                    res.Requirement.ServiceType.WorkerDetails.Add(new ServiceTypeWorkerDetail
                    {
                        FullName = user.FullName,
                        Email = user.Email,
                        MobileNo = string.IsNullOrEmpty(user.MobileNo) ? user.PhoneNumber : user.MobileNo,
                    });
                }
            }
            return res;
        }
    }
}