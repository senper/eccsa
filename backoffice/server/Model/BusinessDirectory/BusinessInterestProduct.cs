using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.BusinessDirectory
{
    public class BusinessInterestProduct
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String NameNormalized { get; set; }
        public virtual ICollection<BusinessInterestSourceDetail> BusinessInterestSourceDetails { get; set; }
    }
}
