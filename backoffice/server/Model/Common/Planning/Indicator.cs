using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Planning
{
    public class Indicator
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public String Name{ get; set; }
        public String Description { get; set; }

        public FrequencyEnum Frequency { get; set; }


        public Guid? ParentId { get; set; }

        public ECCSAModuleFlag ResponsibleUnit { get; set; }

        public IndicatorTypeEnum IndicatorType { get; set; }

       // public UnitOfMeasureEnum UnitOfMeasure { get; set; }

        public StatusEnum Status { get; set; }
       // public virtual ICollection<Plan> Plans { get; set; }

        public virtual ICollection<Indicator> SubIndicators { get; set; }

        public virtual Indicator ParentIndicator { get; set; }
        [NotMapped]
        public string IdString => Id.ToString().Replace("-", "")[^10..];
    }
}