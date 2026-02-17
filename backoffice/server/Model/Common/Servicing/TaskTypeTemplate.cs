using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Common.Servicing
{
    public class TaskTypeTemplate
    {
        public Guid Id { get; set; }

        [NotMapped]
        public String Name => $"{NameEng} {NameLocal}";
        public string NameEng { get; set; }

        public string NameLocal { get; set; }

        public string DescriptionEng { get; set; }

        public string DescriptionLocal { get; set; }


    }

}