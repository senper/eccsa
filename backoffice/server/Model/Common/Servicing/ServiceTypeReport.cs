using System;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class ServiceTypeReport
    {
        public Guid Id { get; set; }
        public Guid ServiceTypeId { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public Guid ServiceRequirementId { get; set; }
        public virtual ServiceRequirement ServiceRequirement { get; set; }
        public string Label { get;set; }
        public string FieldName { get; set; }
        public int EnumValue { get; set; }
    }
    
}
