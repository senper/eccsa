

using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Common
{
    public class FlagType
    {
        public int Id { get; set; }
		public String Name { get; set; }
		public String NameLocal { get; set; }


        public virtual ICollection<Flag> Flags { get; set; }


        
    }
}