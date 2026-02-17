using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Reports
{
    public class ReportField
    {
        public int Id { get; set; }

        public int ReportId { get; set; }

        [Required]
        public string FieldName { get; set; }


        [NotMapped]
        public string DisplayCaption
        {
            get
            {
                if(Report == null)
                {
                    return Caption;
                }
                if (Report.PKFieldId == Id)
                    return "*";
                return Caption;
            }
        }

        [Required]
        public string Caption { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsViewable { get; set; }

        public bool IsGroupField { get; set; }

        public bool IsPercent { get; set; }

        public bool IsNumeric { get; set; }

        public bool IsFilterField { get; set; }

        //[Required]
        //[StringLength(500)]
        public string DropDownSource { get; set; }

        public Guid? DropDownSourceId { get; set; }

        public FilterFieldType FieldTypeId { get; set; }
        public int IsDefault { get; set; }

        [Required]
        [StringLength(100)]
        public string FilterFieldName { get; set; }

        public virtual Report Report { get; set; }
        [NotMapped]
        public string HtmlFilterFieldName
        {
            get { return FilterFieldName + "fval"; }
        }
        [NotMapped]
        public string HtmlFilterFieldName1
        {
            get { return FilterFieldName + "fval1"; }
        }
        [NotMapped]
        public string HtmlFilterFieldAndOr
        {
            get { return FieldName + "fAO"; }
        }
        [NotMapped]
        public string HtmlFieldAggregate
        {
            get { return FieldName + "fagg"; }
        }
        [NotMapped]
        public string HtmlFieldPivot
        {
            get { return FieldName + "fpiv"; }
        }
    }
}
