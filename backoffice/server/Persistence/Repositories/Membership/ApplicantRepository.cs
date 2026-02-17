using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Common.Servicing;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class ApplicantRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        private readonly InvoiceRepository _invoiceRepository;
        public ApplicantRepository(EccsaContext context, EccsaUserContext userContext
            , InvoiceRepository invoiceRepository)
        {
            _context = context;
            _userContext = userContext;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<List<Applicant>> UserApplicants(Guid userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var associationId = user.AssociationId;
            var listQuery = _context.Applicants.
                Where(a => a.AssociatedUserId == userId);
            var list = await listQuery.ToListAsync();
            var associations = await _context.Associations.Where(s => listQuery.Any(a => a.AssociationId == s.Id))
                .ToListAsync();
            var documents = await _context.ApplicantDocuments.Where(s => listQuery.Any(a => a.Id == s.ApplicantId))
                .ToListAsync();
            list.ForEach(l =>
            {
                l.AssociationName = associations.FirstOrDefault(s => s.Id == l.AssociationId)?.NameEng ?? "";
                l.Responses = documents.Where(d => d.ApplicantId == l.Id).ToList();
            });
            return list;
        }
        public IQueryable<Applicant> Applicants(Guid userId)
        {

            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var associationId = user.AssociationId;
            associationId = Guid.Parse("00000000-0000-0000-0000-0000000f424e");
            return _context.Applicants.Where(a => a.AssociationId == associationId)
;
        }

        public async Task<int> ListCount(Guid associationId, Guid userId)
        {

            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            return await _context.Applicants.Where(a => (a.AssociationId == associationId || user.AssociationId == Constants.ECCSA)
            && a.ApplicationStatus != WorkflowStateEnum.Closed).CountAsync();

        }
        public async Task<List<Applicant>> List(Guid associationId, Guid userId)
        {

            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);

            return await _context.Applicants.Include(a => a.Responses).Where(a => a.AssociationId == associationId).ToListAsync();

        }
        public async Task<Applicant> ApplicantById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var request = await _context.Applicants
                                    .Include(a => a.Association)
                                    .ThenInclude(a => a.MembershipRequirements)
                                    .Include(x => x.Responses)
                                    .ThenInclude(a => a.Requirement)
                                    .FirstOrDefaultAsync(i => i.Id == id);
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.InvoiceNo))
                {
                    request.Invoice = await _invoiceRepository.InvoiceByNo(request.InvoiceNo, request.Id);
                }
            }
            return request;
        }
        public async Task<Applicant> UserApplication(Guid userId, Guid id)
        {
            var request = await _context.Applicants
                    .Include(x => x.Responses)
                    .ThenInclude(x => x.Requirement)
                    //.Include(x => x.Followups)
                    //.Include(x => x.Attachments)
                    .FirstOrDefaultAsync(i => i.Id == id && i.AssociatedUserId == userId);
            if (request != null)
            {
                var invoice = await _invoiceRepository.InvoiceByNo(request.InvoiceNo, request.Id);
                if (invoice != null)
                {
                    request.Invoice = new Invoice
                    {
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
            }

            return request;
        }
        public async Task<List<ApplicantDocument>> ApplicantDocuments(Guid id, Guid userId)
        {
            return await _context.ApplicantDocuments.
                       Where(r => r.Applicant.AssociatedUserId == userId && r.ApplicantId == id)
                       .ToListAsync();
        }
        private static bool IsNullOrEmpty(ApplicantDocument document)
        {
            if (document.DecimalResponse.HasValue) return false;
            if (document.DateResponse.HasValue) return false;
            if (document.IntResponse.HasValue) return false;
            if (document.LookupValueId.HasValue) return false;
            if (!string.IsNullOrEmpty(document.TextResponse)) return false;
            return true;
        }
        public async Task SubmitUserApplication(Guid applicationId, Guid userId)
        {
            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == applicationId && r.AssociatedUserId == userId);
            if (tmp == null)
                return;
            if (tmp.ApplicationStatus != WorkflowStateEnum.NotSet)
                return;
            tmp.ApplicationStatus = WorkflowStateEnum.Submitted;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(tmp);
            var lst = await _context.ApplicantDocuments.Include(r => r.Requirement).Where(r => r.ApplicantId == applicationId).ToListAsync();
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
                _context.ApplicantDocuments.Update(res);
            }
            await _context.SaveChangesAsync();
        }

        public async Task SubmitUserApplicationDocument(Guid documentId, Guid userId)
        {
            var res = await _context.ApplicantDocuments.FirstOrDefaultAsync(r => r.Id == documentId
            && r.Applicant.AssociatedUserId == userId);
            if (res.ReviewStatus != DocumentReviewStatusEnum.NotSet && res.ReviewStatus != DocumentReviewStatusEnum.Rejected) return;
            res.ReviewStatus = DocumentReviewStatusEnum.Submitted;
            _context.ApplicantDocuments.Update(res);
            await _context.SaveChangesAsync();
        }
        public async Task RecordUserPayment(Guid applicationId, string invoiceNo, string receiptNo, PaymentMethodEnum paymentMethod, string remark, Guid userId)
        {
            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == applicationId);
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
            _context.Applicants.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateResponse(Applicant data, Guid userId)
        {
            var reqs = await _context.Applicants
                .Where(x => x.AssociationId == data.AssociationId).ToListAsync();

            var request = await _context.Applicants.FirstOrDefaultAsync(x => x.Id == data.Id);
            var association = await _context.Associations.FirstOrDefaultAsync(s => s.Id == data.AssociationId);
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            foreach (var res in data.Responses)
            {
                var savedRes = _context.ApplicantDocuments.Include(r => r.Requirement).
                    FirstOrDefault(r => r.ApplicantId == request.Id && r.RequirementId == res.RequirementId);
                if (savedRes == null || (savedRes.ReviewStatus != DocumentReviewStatusEnum.Rejected
                    && savedRes.ReviewStatus != DocumentReviewStatusEnum.NotSet)) { continue; }
                if (savedRes.Requirement.InputMethod != ServiceInputTypeEnum.Upload || !string.IsNullOrEmpty(res.TextResponse))
                {
                    savedRes.TextResponse = res.TextResponse;
                }
                savedRes.IntResponse = res.IntResponse;
                savedRes.DateResponse = res.DateResponse;
                savedRes.DecimalResponse = res.DecimalResponse;

                _context.ApplicantDocuments.Update(savedRes);
            }
            _context.Applicants.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task<Applicant> Save(Applicant data, Guid userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var association = await _context.Associations.FirstOrDefaultAsync(t => t.Id == data.AssociationId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            if (association == null)
            {
                throw new InvalidOperationException("Association not found");
            }
            //var reqs = await _context.MembershipRequirements
             //   .Where(x => x.Status == StatusEnum.Active && x.AssociationId == data.AssociationId).ToListAsync();


            var Cnt = await _context.Applicants.CountAsync(r => r.ApplicationDate == data.ApplicationDate.Date);

            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();

                data.NameEng = String.IsNullOrEmpty(user.CompanyName) ? user.FullName : user.CompanyName;
                data.ApplicationDate = DateTime.Today;
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.UpdatedBy = userId;
                data.UpdateStamp = DateTime.UtcNow;
                data.TrackingNo = GenerateTrackingNo(Cnt, (int)ECCSAModuleFlag.Membership);
                data.TIN = user.TIN;
                data.EmailAddress = user.Email;
                data.MobileNo = user.MobileNo;
                data.Address = user.Address;
                data.ApplicationType = data.ApplicationType;
                foreach (var res in data.Responses)
                {
                    res.ApplicantId = data.Id;
                    res.ReviewStatus = DocumentReviewStatusEnum.NotSet;
                    res.Remark = string.Empty;
                   // _context.ApplicantDocuments.Add(res);
                }
                _context.Applicants.Add(data);
                //_context.ApplicantDocuments.AddRange(data.Responses);
            }
            else
            {
                var tmp = await _context.Applicants.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Service Request Not found");
                }
                tmp.InvoiceNo = data.InvoiceNo;
                tmp.ReceiptNo = data.ReceiptNo;
                tmp.PaymentMethod = data.PaymentMethod;
                tmp.ApplicationStatus = data.ApplicationStatus;
                data.ApplicationType = data.ApplicationType;
                tmp.Remark = data.Remark;

                //tmp.PreviousStep = data.PreviousStep;
                //tmp.CurrentStep = data.CurrentStep;
                tmp.UpdatedBy = userId;
                tmp.UpdateStamp = DateTime.UtcNow;

                //tmp.MobileNo = user.MobileNo;
                //tmp.EmailAddress = user.Email;
                _context.Applicants.Update(tmp);
            }

            await _context.SaveChangesAsync();
            await AutoGenerateInvoice(data, association, user);
            return await ApplicantById(data.Id, userId);
        }

        public async Task<List<User>> Users()
        {
            var roles = Role.GetRoles(ECCSAModuleFlag.Membership);
            var tmp = await _userContext.Users.Include(u => u.UserRoles).Where(u => u.AssignedModules > 0 && u.AssignedModules.HasFlag(ECCSAModuleFlag.Membership)).ToListAsync();
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
        private async Task AutoGenerateInvoice(Applicant request, Association association, User user)
        {
            //if (association.RateBasis != RateBasisEnum.Flat) return;
            if (!string.IsNullOrEmpty(request.InvoiceNo)) return;
            var simpleInvoice = new Invoice()
            {
                SubSystem = ECCSAModuleFlag.Membership,
                ReferenceId = request.Id,
                BasePrice = decimal.Round(association.RegistrationFee / 1.15m, 2, MidpointRounding.ToEven),
                Discount = 0,
                TotalAmount = association.RegistrationFee,
                CompanyName = request.NameEng,
                TIN = user.TIN,
                Address = user.Address,
                PaymentReason = "Membership Registration Fee"
            };
            var invoice = await _invoiceRepository.GetOrCreate(simpleInvoice, request.CreatedBy);
            await RecordInvoice(request.Id, invoice.InvoiceNo, association.RegistrationFee, request.CreatedBy);
        }
        public async Task RecordInvoice(Guid applicantId, string invoiceNo, decimal amount, Guid userId)
        {

            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == applicantId);
            if (tmp == null)
                return;
            tmp.InvoiceNo = invoiceNo;
            tmp.UpdatedBy = userId;
            tmp.ApplicationFee = amount;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(tmp);
            await _context.SaveChangesAsync();
        }

        private async Task<MembershipEnrollment> UpdateMembership(RenewalHistory renewal)
        {
            var tmp = await _context.MembershipEnrollments.FirstOrDefaultAsync(r => r.Id == renewal.EnrollmentId);

            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            tmp.MembershipStatus = renewal.RenewalStatus == MembershipWorkflowEnum.Accepted ?
                Model.Common.StatusEnum.Active :
                renewal.RenewalStatus == MembershipWorkflowEnum.Rejected ? Model.Common.StatusEnum.Inactive : tmp.MembershipStatus;
            tmp.LastRenewalDate = renewal.RenewalDate;
            tmp.ExpiryDate = renewal.RenewalDate.AddYears(1);
            tmp.AnnualPayment = renewal.Fee;
            tmp.InvoiceNo = renewal.InvoiceNo;
            tmp.BankReceiptNo = renewal.ReceiptNo;
            tmp.Remark = renewal.Remark;
            return tmp;
        }

        public async Task ChangeRequestStatus(Guid applicantId
          , WorkflowStateEnum status
          , string code
          , string extension
          , String Remark
          , Guid userId)
        {
            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == applicantId);
            if (tmp == null)
                return;

            tmp.ApplicationStatus = status;
            tmp.Signature = string.IsNullOrEmpty(code) ? string.Empty : code;
            tmp.Remark = string.IsNullOrEmpty(Remark) ? string.Empty : Remark;
            tmp.Extension = string.IsNullOrEmpty(extension) ? string.Empty : extension;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(tmp);

            await _context.SaveChangesAsync();
        }


        public async Task Delete(Applicant data)
        {
            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Applicants.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicantDocument> ApplicantDocumentById(Guid id)
        {
            return await _context.ApplicantDocuments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task RecommendApprove(Guid applicantId, string extension, string code, Guid userId)
        {
            var request = await _context.Applicants
                    .FirstOrDefaultAsync(i => i.Id == applicantId);
            if (request.RequestStatus != WorkflowStateEnum.UnderReview)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.DraftPrepared;
            request.Extension = extension;
            request.Signature = code;
            request.Code = code;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task RecommendRejection(Guid applicantId, String remark, Guid userId)
        {
            var request = await _context.Applicants
                    .FirstOrDefaultAsync(i => i.Id == applicantId);
            if (request.RequestStatus != WorkflowStateEnum.UnderReview)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.RecommendRejection;
            request.Remark = remark;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task PrepareIdCard(IdCardInfo data)
        {
            var enrollment = await _context.MembershipEnrollments.FirstOrDefaultAsync(a => a.MemberId == data.IndividualId && a.ParentAssociationId == data.AssociationId);
            if (enrollment == null)
                throw new InvalidOperationException("Membership Info not found");
            var individual = await _context.Individuals.Include(i => i.Stakeholder)
                    .FirstOrDefaultAsync(i => i.Id == data.IndividualId);
            if (enrollment.MembershipStatus == StatusEnum.Active && enrollment.ExpiryDate > DateTime.Today)
            {
                enrollment.IdCard = new IdCardInfo();
            }
            else
            {
                enrollment.IdCard = new IdCardInfo()
                {
                    Code = enrollment.Code,
                    ExpiryDate = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd"),
                    DateIssued = DateTime.Today.ToString("yyyy-MM-dd"),
                    CompanyName = individual.NameEng,
                    Sector = individual.Sector,
                    FullName = data.FullName,
                    Email = data.Email,
                    HouseNo = data.HouseNo,
                    LicenseNo = data.LicenseNo,
                    MembershipNo = data.MembershipNo,
                    PhoneNumber = data.PhoneNumber,
                    TitleName = data.TitleName,
                    Woreda = data.Woreda,
                    Zone = data.Zone,
                    ReadyForPrint = true
                };
            }
            _context.MembershipEnrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }
        public async Task<MembershipEnrollment> ApproveAcceptRequest(Guid applicantId, Guid userId)
        {
            var approver = await _userContext.Users.FirstOrDefaultAsync(f => f.Id == userId);
            var request = await _context.Applicants.Include(x => x.Responses).FirstOrDefaultAsync(i => i.Id == applicantId);
            if (request.RequestStatus != WorkflowStateEnum.DraftPrepared)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.Accepted;
            request.ApproverName = approver.FullName;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            Individual individual;
            Guid membershipId;
            MembershipEnrollment enrollment;
            individual = await _context.Individuals
    .Include(x => x.LegalDocuments)
    .Include(x => x.Stakeholder)

    .FirstOrDefaultAsync(i => i.Id == request.IndividualId);

            if (request.IndividualId.HasValue && request.IndividualId != Guid.Empty && individual != null)
            {
                enrollment = await _context.MembershipEnrollments.FirstOrDefaultAsync(a => a.MemberId == individual.Id && a.ParentAssociationId == request.AssociationId);
                if (enrollment != null)
                {
                    _context.RenewalHistory.Add(new RenewalHistory()
                    {
                        Id = Guid.NewGuid(),
                        ApplicationDate = request.ApplicationDate,
                        RenewalDate = enrollment.LastRenewalDate,
                        RenewalStatus = MembershipWorkflowEnum.Accepted,
                        Fee = enrollment.ApplicationFee,
                        InvoiceNo = enrollment.InvoiceNo,
                        InvoiceDate = enrollment.LastRenewalDate,
                        ReceiptNo = enrollment.BankReceiptNo,
                        PaymentMethod = request.PaymentMethod,
                        EnrollmentId = enrollment.Id,
                        Remark = enrollment.Remark
                    });
                    enrollment.ApplicationFee = request.ApplicationFee;
                    enrollment.BankReceiptNo = request.ReceiptNo;
                    enrollment.InvoiceNo = request.InvoiceNo;
                    enrollment.RenewalStatus = MembershipWorkflowEnum.Accepted;
                    enrollment.MembershipStatus = StatusEnum.Active;
                    enrollment.Remark = request.Remark;

                    _context.MembershipEnrollments.Update(enrollment);
                }
                else
                {

                    enrollment = new MembershipEnrollment
                    {
                        Id = Guid.NewGuid(),
                        MemberId = individual.Id,
                        ParentAssociationId = request.AssociationId,
                        Code = request.Code,
                        LastRenewalDate = DateTime.Today,
                        ExpiryDate = DateTime.Today.AddYears(1),
                        ApplicationFee = request.ApplicationFee,
                        BankReceiptNo = request.ReceiptNo,
                        InvoiceNo = request.InvoiceNo,
                        RenewalStatus = MembershipWorkflowEnum.Accepted,
                        MembershipStatus = StatusEnum.Active,
                        Remark = request.Remark,
                        IdCard = new IdCardInfo()
                        {
                            Code = request.Code,
                            ExpiryDate = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd"),
                            DateIssued = DateTime.Today.ToString("yyyy-MM-dd"),
                            CompanyName = individual.NameEng,
                            Sector = individual.Sector,
                        }
                    };

                    _context.MembershipEnrollments.Add(enrollment);
                }
                membershipId = enrollment.Id;
                var documents = individual.LegalDocuments;
                if (documents == null)
                    documents = new List<LegalDocument>();
                foreach (var d in request.Responses)
                {
                    var doc = documents.FirstOrDefault(x => x.RequirementId == d.RequirementId);
                    if (doc == null)
                    {
                        doc = new LegalDocument
                        {
                            Id = d.Id,
                            IndividualId = individual.Id,
                            RequirementId = d.RequirementId,
                            DateResponse = d.DateResponse,
                            DateReviewed = d.DateReviewed,
                            DateSubmitted = d.DateSubmitted,
                            DecimalResponse = d.DecimalResponse,
                            Extension = d.Extension,
                            IntResponse = d.IntResponse,
                            LookupValueId = d.LookupValueId,
                            TextResponse = d.TextResponse,
                            ReviewedBy = d.ReviewedBy,
                            ReviewStatus = d.ReviewStatus
                        };

                        _context.LegalDocuments.Add(doc);
                    }
                    else
                    {
                        doc.DateResponse = d.DateResponse;
                        doc.DateReviewed = d.DateReviewed;
                        doc.DateSubmitted = d.DateSubmitted;
                        doc.DecimalResponse = d.DecimalResponse;
                        doc.Extension = d.Extension;
                        doc.IntResponse = d.IntResponse;
                        doc.TextResponse = d.TextResponse;
                        doc.LookupValueId = d.LookupValueId;

                        doc.ReviewedBy = d.ReviewedBy;
                        doc.ReviewStatus = d.ReviewStatus;
                        _context.LegalDocuments.Update(doc);
                    }
                }
                if (individual == null) throw new InvalidOperationException("Member not found");

            }
            else
            {
                individual = new Individual()
                {
                    Id = request.Id,
                    Address = request.Address,
                    AssociatedUserId = request.AssociatedUserId,
                    LegalDocuments = new List<LegalDocument>(),
                    CreatedBy = userId,
                    CreateStamp = DateTime.UtcNow,
                    UpdatedBy = userId,
                    UpdateStamp = DateTime.UtcNow,
                    MemberSince = DateTime.Today,
                    NameEng = request.NameEng,
                    NameLocal = request.NameLocal,
                    LocationId = Constants.EthiopiaId,
                    Stakeholder = new Stakeholder()
                    {
                        Id = request.Id,
                        StakeholderType = StakeholderTypeEnum.Individual,
                        Status = StatusEnum.Active,
                    }
                };
                var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == request.AssociatedUserId);
                if (user != null)
                {
                    user.AssociationId = individual.Id;
                    _userContext.Users.Update(user);
                    await _userContext.SaveChangesAsync();
                }
                foreach (var d in request.Responses)
                {
                    individual.LegalDocuments.Add(new LegalDocument()
                    {
                        Id = d.Id,
                        DateResponse = d.DateResponse,
                        DateReviewed = d.DateReviewed,
                        DateSubmitted = d.DateSubmitted,
                        DecimalResponse = d.DecimalResponse,
                        IntResponse = d.IntResponse,
                        TextResponse = d.TextResponse,
                        Extension = d.Extension,
                        IndividualId = individual.Id,
                        Remark = d.Remark,
                        RequirementId = d.RequirementId,
                        ReviewedBy = d.ReviewedBy,
                        ReviewStatus = d.ReviewStatus,
                        LookupValueId = d.LookupValueId,
                    });
                }
                individual.Stakeholder.MembershipEnrollments = new List<MembershipEnrollment>();
                enrollment = new MembershipEnrollment()
                {
                    Id = Guid.NewGuid(),
                    MemberId = individual.Id,
                    ParentAssociationId = request.AssociationId,
                    RenewalStatus = MembershipWorkflowEnum.Accepted,
                    MembershipStatus = StatusEnum.Active,
                    DateOfEnrollment = DateTime.Today,
                    ExpiryDate = DateTime.Today.AddYears(1),
                    InvoiceNo = request.InvoiceNo,
                    ApplicationFee = request.ApplicationFee,
                    BankReceiptNo = request.ReceiptNo,
                    Code = request.Signature,
                };
                individual.Stakeholder.MembershipEnrollments.Add(enrollment);

                _context.Stakeholders.Add(individual.Stakeholder);
                _context.MembershipEnrollments.AddRange(individual.Stakeholder.MembershipEnrollments);
                _context.Individuals.Add(individual);
                _context.LegalDocuments.AddRange(individual.LegalDocuments);
            }

            _context.Applicants.Remove(request);
            _context.ApplicantDocuments.RemoveRange(request.Responses);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task ApproveReceiptPrint(Guid applicantId
            , decimal invoiceAmount, PaymentStatusEnum paymentStatus, Guid userId)
        {
            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == applicantId);
            if (tmp == null)
                return;
            //if (invoiceAmount - confirmedAmount > 0.01M)
            //	return;
            tmp.PrintReceipt = paymentStatus == PaymentStatusEnum.Accepted;
            tmp.PaymentStatus = paymentStatus;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task DeclineRejectionRequest(Guid applicantId, Guid userId, String remark)
        {
            var request = await _context.Applicants
                    .FirstOrDefaultAsync(i => i.Id == applicantId);
            if (request.RequestStatus != WorkflowStateEnum.RecommendRejection)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.UnderReview;
            request.Remark = remark;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeclineAcceptRequest(Guid applicantId, Guid userId, String remark)
        {
            var request = await _context.Applicants
                    .FirstOrDefaultAsync(i => i.Id == applicantId);
            if (request.RequestStatus != WorkflowStateEnum.DraftPrepared)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.UnderReview;
            request.Remark = remark;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(request);
            await _context.SaveChangesAsync();
        }



        public async Task ApproveRejectionRequest(Guid applicantId, Guid userId, String remark)
        {
            var approver = await _userContext.Users.FirstOrDefaultAsync(f => f.Id == userId);
            var request = await _context.Applicants
                    .FirstOrDefaultAsync(i => i.Id == applicantId);
            if (request.RequestStatus != WorkflowStateEnum.RecommendRejection)
                throw new InvalidOperationException("Invalid Request Status");
            request.RequestStatus = WorkflowStateEnum.Rejected;
            request.Remark = remark;
            request.ApproverName = approver.FullName;
            request.UpdatedBy = userId;
            request.UpdateStamp = DateTime.UtcNow;
            _context.Applicants.Update(request);
            await _context.SaveChangesAsync();
        }



        public async Task StartProcessing(Guid id, Guid userId)
        {
            var theUser = await _userContext.Users.FirstOrDefaultAsync(o => o.Id == userId);
            if (theUser == null)
                return;
            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null)
                return;
            if (!tmp.UnderReview)
                return;
            var operators = await _context.OperatorHistory.Where(o => o.ServiceRequestId == id && !o.EndDate.HasValue).ToListAsync();
            tmp.OperatorName = theUser.FullName;
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
            _context.Applicants.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task AssignUser(Guid requestId, Guid operatorId, Guid userId)
        {
            var theUser = await _userContext.Users.FirstOrDefaultAsync(o => o.Id == operatorId);
            if (theUser == null)
                return;
            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (!tmp.UnderReview)
                return;
            var operators = await _context.OperatorHistory.Where(o => o.ServiceRequestId == requestId && !o.EndDate.HasValue).ToListAsync();
            tmp.OperatorName = theUser.FullName;
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

            _context.Applicants.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task ReviewRequirement(Guid id, Guid requestId, DocumentReviewStatusEnum status, string remark, Guid userId)
        {
            var tmp = await _context.Applicants.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (tmp.RequestStatus == WorkflowStateEnum.NotSet)
                return;
            var response = await _context.ApplicantDocuments.Include(r => r.Requirement).FirstOrDefaultAsync(r => r.Id == id && r.ApplicantId == requestId);
            response.ReviewStatus = status;
            response.ReviewedBy = userId;
            response.DateReviewed = DateTime.UtcNow;
            response.Remark = remark ?? string.Empty;
            _context.ApplicantDocuments.Update(response);
            await _context.SaveChangesAsync();
        }
    }
}