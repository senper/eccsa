using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.API.Seeding
{
    public static partial class InitializeDatabase
    {
        private static void InitializeMembership(EccsaContext context)
        {
            InitializeLegalDocumentTypes(context);
        }
        private static void InitializeLegalDocumentTypes(EccsaContext context)
        {
            var lstTypes = new List<LegalDocumentType>
            {
                new LegalDocumentType
                {
                    Id = 1,
                    AllowMultiple = false,
                    HasExpiry = true,
                    NameEng = "Main Registration",
                    NameLocal = "ዋና ምዝገባ"
                },
                new LegalDocumentType
                {
                    Id = 2,
                    AllowMultiple = true,
                    HasExpiry = true,
                    NameEng = "Trade License",
                    NameLocal = "ንግድ ፈቃድ"
                },
                new LegalDocumentType
                {
                    Id = 3,
                    AllowMultiple = false,
                    HasExpiry = true,
                    NameEng = "TIN Certificate",
                    NameLocal = "የግብር ከፋይ መለያ "
                },
                new LegalDocumentType
                {
                    Id = 4,
                    AllowMultiple = false,
                    HasExpiry = true,
                    NameEng = "VAT Certificate",
                    NameLocal = "የተጨማሪ እሴት ግብር"
                },
                new LegalDocumentType
                {
                    Id = 5,
                    AllowMultiple = false,
                    HasExpiry = true,
                    NameEng = "Article of Incorporation",
                    NameLocal = "መመስረቻ "
                },
                new LegalDocumentType
                {
                    Id = 6,
                    AllowMultiple = false,
                    HasExpiry = true,
                    NameEng = "Article of",
                    NameLocal = "መተዳደሪያ"
                },
                new LegalDocumentType
                {
                    Id = 6,
                    AllowMultiple = false,
                    HasExpiry = true,
                    NameEng = "Professional Competency Certificate",
                    NameLocal = "የሙያ ብቃት ማረጋገጫ"
                },
                new LegalDocumentType
                {
                    Id = 7,
                    AllowMultiple = false,
                    HasExpiry = true,
                    NameEng = "Audit Report",
                    NameLocal = "የኦዲት ሪፖርት"
                }
            };
            context.LegalDocumentTypes.AddRange(lstTypes);
        }
    }
}
