using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Advocacy;

namespace ECCSA.BackOffice.Server.Persistence.Repositories
{
    public class BaseRepository
    {
        private readonly EccsaContext _context;
        private readonly string[] blackWord = new string[] {
             "the","about","our", "of"
};
        public BaseRepository(EccsaContext context)
        {
            _context = context;
        }
        public async Task GenerateBacklog()
        {
            var hasContent = await _context.Contents.AnyAsync();
            if (hasContent) return;
            var serviceTypes = await _context.ServiceTypes.Include(s => s.ServiceRequirements).ToListAsync();
            var news = await _context.News.ToListAsync();
            foreach (var serviceType in serviceTypes)
            {
                await GenerateKeyword(serviceType);
                foreach (var req in serviceType.ServiceRequirements)
                {
                    await GenerateKeyword(serviceType, req);
                }
            }
            foreach (var n in news)
            {
                await GenerateKeyword(ContentTypeEnum.News, n.Id,$"{n.Title} {n.Brief}", n.Title, n.Brief, n.Id);
            }
        }
        private ContentTypeEnum GetContentType(ServiceType data)
        {
            if (data.ServiceOwner.HasFlag(ECCSAModuleFlag.Academy))
            {
                return ContentTypeEnum.Academy;
            }
            else if (data.ServiceOwner.HasFlag(ECCSAModuleFlag.Arbitration))
            {
                return ContentTypeEnum.Arbitration;
            }
            else if (data.ServiceOwner.HasFlag(ECCSAModuleFlag.TradePromotion))
            {
                return ContentTypeEnum.TradePromotion;
            }
            else if (data.ServiceOwner.HasFlag(ECCSAModuleFlag.Investment))
            {
                return ContentTypeEnum.TradeFacilitation;
            }
            else if (data.ServiceOwner.HasFlag(ECCSAModuleFlag.Research))
            {
                return ContentTypeEnum.Research;
            }
            else if (data.ServiceOwner.HasFlag(ECCSAModuleFlag.Bmo))
            {
                return ContentTypeEnum.BMO;
            }
            return ContentTypeEnum.None;
        }

        public async Task GenerateKeyword(ServiceType data)
        {
           
            var contentType = GetContentType(data);
            await GenerateKeyword(contentType, data.Id, $"{data.NameEng} {data.DescriptionEng}", data.NameEng, data.DescriptionEng, data.Id);
            
        }

        public async Task GenerateKeyword(ServiceType sType, ServiceRequirement data)
        {
            var contentType = GetContentType(sType);
            await GenerateKeyword(contentType, data.ServiceTypeId, $"{data.NameEng} {data.DescriptionEng}", sType.NameEng, sType.DescriptionEng, data.Id);
        }

        public async Task GenerateKeyword(Dialog data)
        {
            var contentType = ContentTypeEnum.PPD;
            await GenerateKeyword(contentType, data.Id, $"{data.Title} {data.Description}", data.Title, data.Description, data.Id);
        }
        public async Task GenerateKeyword(Dialog sType, Recommendation data)
        {
            var contentType = ContentTypeEnum.PPD;
            await GenerateKeyword(contentType, sType.Id, $"{data.Title} {data.ImplementationText}", sType.Title, sType.Description, data.Id);
        }
        public async Task GenerateKeyword(OtherMedia data)
        {
            ContentTypeEnum contentType = ContentTypeEnum.Media;
            
            await GenerateKeyword(contentType,data.Id, $"{data.Title} {data.Brief} {data.AuthorName}", data.Title, data.Brief, data.Id);
        }
        public async Task GenerateKeyword(Law data)
        {
            var baseId = 0;
            switch (data.Category)
            {
                case LawCategoryEnum.Trade:
                    baseId = (int)ContentTypeEnum.TradeLawProclamation;
                    break;
                case LawCategoryEnum.Investment:
                    baseId = (int)ContentTypeEnum.InvestmentLawProclamation;
                    break;
                case LawCategoryEnum.Customs:
                    baseId = (int)ContentTypeEnum.CustomsLawProclamation;
                    break;
                case LawCategoryEnum.Domestic:
                    baseId = (int)ContentTypeEnum.DomesticLawProclamation;
                    break;

                case LawCategoryEnum.Land:
                    baseId = (int)ContentTypeEnum.LandLawProclamation;
                    break;
                case LawCategoryEnum.Other:
                    baseId = (int)ContentTypeEnum.OtherLawProclamation;
                    break;
                default:
                    return;
            }
            switch (data.LawType)
            {
                case LawTypeEnum.Directive:
                    baseId += 1;
                    break;
                case LawTypeEnum.Regulation:
                    baseId += 2;
                    break;
            }
            await GenerateKeyword((ContentTypeEnum)baseId, data.Id, $"{data.Title} {data.Brief}", data.Title,  data.Brief, data.Id);
        }

        public async Task RemoveKeyword( Guid contentId, Guid detailId)
        {
            var keywords = await _context.Keywords.Where(k => k.DetailId == detailId).ToListAsync();
            var content = await _context.Contents.FirstOrDefaultAsync(k => k.Id == contentId);
            if (content != null && contentId == detailId)
                _context.Contents.Remove(content);
            _context.Keywords.RemoveRange(keywords);
        }
        public async Task SaveContent(Guid id, ContentTypeEnum contentType, String title, String brief)
        {
            
            var data = new SearchResult { Id = id, ContentType = contentType, Brief = brief, Title = title };
            var tmp = await _context.Contents.FirstOrDefaultAsync(c => c.Id == id);
            if(tmp == null)
            {
                _context.Contents.Add(data);
            }else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
        }
        public async Task GenerateKeyword(ContentTypeEnum contentType, Guid contentId, String phrase, string title, string brief, Guid detailId)
        {
            await GenerateCommonKeywords();
            var words = phrase.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var tmp = await _context.Keywords.Where(k => k.DetailId == detailId).ToListAsync();

            if(tmp != null && tmp.Count > 0)
            {
                _context.Keywords.RemoveRange(tmp);
                await _context.SaveChangesAsync();
            }
            List<string> currentWords = new List<string>();
            var commonKeywords = await _context.CommonKeywords.FirstOrDefaultAsync(c => c.ContentType == contentType);
            if (commonKeywords != null && commonKeywords.Keywords != null && commonKeywords.Keywords.Count > 0)
            {
                foreach (var word in commonKeywords.Keywords)
                {
                    var lower = word.ToLowerInvariant();
                    if (lower.Length < 4) continue;
                    if (blackWord.Any(w => w == lower)) continue;
                    if (currentWords.Any(w => w == lower)) continue;
                    currentWords.Add(lower);
                    _context.Keywords.Add(new Keyword
                    {
                        Id = Guid.NewGuid(),
                        ContentId = contentId,
                        ContentType = contentType,
                        Word = word,
                        Normalized = lower
                    });
                }
            }
            foreach (var wrd in words)
            {
                var word = Sanitize(wrd);
                var lower = word.ToLowerInvariant();
                if (lower.Length < 4) continue;
                if (blackWord.Any(w => w == lower)) continue;
                if (currentWords.Any(w => w == lower)) continue;
                currentWords.Add(lower);
                _context.Keywords.Add(new Keyword 
                {
                    Id = Guid.NewGuid(),
                    ContentId = contentId,
                    ContentType = contentType,
                    Word = word,
                    Normalized = lower
                });
            }

            await SaveContent(contentId, contentType, title, brief);
        }
        private static string Sanitize(string wrd)
        {
            return wrd
                .Replace("(","")
                .Replace(")", "")
                .Replace("!", "")
                .Replace("@", "")
                .Replace("#", "")
                .Replace("$", "")
                .Replace("%", "")
                .Replace("^", "")
                .Replace("*", "")
                .Replace("-", "")
                .Replace("_", "")
                .Replace("+", "")
                .Replace("<", "")
                .Replace(",", "")
                .Replace(">", "")
                .Replace(".", "")
                .Replace("/", "")
                .Replace("?", "")
                .Replace(";", "")
                .Replace(":", "");
        }
        
        public async Task<List<SearchResult>> GetSearchResults(string searchPrase)
        {
            searchPrase = Sanitize(searchPrase).ToLowerInvariant();
            //var keywords = await _context.Keywords.Where(k => k.Normalized.Contains(searchPrase)).ToListAsync();
            var lst = await _context.Contents.Where(c => _context.Keywords.Any(k => k.Normalized.Contains(searchPrase) && k.ContentId == c.Id)).ToListAsync();
            return lst;
            
        }
        
        private async Task GenerateCommonKeywords()
        {
            var tmp = await _context.CommonKeywords.AnyAsync();
            if (tmp) return;
            foreach (int i in Enum.GetValues(typeof(ContentTypeEnum)))
            {
                var contentType = ((ContentTypeEnum)i).GetDisplayName();
                if (contentType == null) continue;
                var words = contentType.Split(" ").ToList();
                
                _context.CommonKeywords.Add(new CommonKeyword()
                {
                    ContentType = ((ContentTypeEnum)i),
                    Keywords = words
                });

            }
            await _context.SaveChangesAsync();
        }
      
    }
}
