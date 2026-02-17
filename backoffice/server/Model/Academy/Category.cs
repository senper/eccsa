using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Academy
{
    public class Category
    {
        public int Id { get; set; }
        public String Code { get; set; }
        public String NameEng { get; set; }
        public String NameLocal { get; set; }
        public int? ParentId { get; set; }
        public int Status { get; set; }


        public virtual ICollection<Category> ChildrenCategories { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<NeedAssessmentDetail> NeedAssessmentDetails { get; set; }


        public virtual Category ParentCategory { get; set; }

    }
}