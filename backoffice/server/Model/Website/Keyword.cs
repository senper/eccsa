using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class Keyword
    {
        public Guid Id { get; set; }

        public Guid ContentId { get; set; }
        public Guid DetailId { get; set; }
        public string Word { get; set; }
        public string Normalized { get; set; }
        public ContentTypeEnum ContentType { get; set; }

        public static Dictionary<ContentTypeEnum, List<String>> CommonKeywords
        {
            get
            {
                return new Dictionary<ContentTypeEnum, List<String>>();
            }
        }
    }
    public class CommonKeyword
    {
        public ContentTypeEnum ContentType { get; set; }
        public List<String> Keywords { get; set; }
    }
    
}
