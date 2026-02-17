using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Website
{
    public class BmoRepository : BaseRepository
    {
        public const int MenuId = 46;
        public const int HeaderId = 3;
        private readonly EccsaContext _context;

        public BmoRepository(EccsaContext context) : base(context)
        {
            _context = context;
        }
       

        public async Task<List<QuickLink>> Links()
        {
            return await _context.QuickLinks.Include(p => p.Header).Where(p=> p.HeaderId == HeaderId).ToListAsync();
        }
        public async Task<QuickLinkHeader> HeaderById()
        {
            return await _context.QuickLinkHeaders
                .Include(h => h.QuickLinks)
                .FirstOrDefaultAsync(h => h.Id == HeaderId);
        }
      

        public async Task<MediaType> MediaType()
        {
            var menu = await _context.Menus.FirstOrDefaultAsync(m => m.Id == MenuId);
            var mediaType = await _context.MediaTypes.FirstOrDefaultAsync(m => m.MenuId == MenuId);
            if (menu == null || mediaType == null) return null;
            var menuQLHeader = await _context.QuickLinkHeaders.Include(h => h.QuickLinks.OrderBy(s => s.Order)).FirstOrDefaultAsync(h => h.Id == HeaderId);
            mediaType.QuickLinkHeader = menuQLHeader;
            var defaultQL = menuQLHeader?.QuickLinks.FirstOrDefault(a => a.MenuId == menu.Id);
            if (menuQLHeader != null && defaultQL != null)
            {
                foreach (var ql in menuQLHeader.QuickLinks.Where(q => q.MenuId != null))
                {
                    var qlMenu = await _context.Menus.FirstOrDefaultAsync(m => m.Id == ql.MenuId);
                    if (qlMenu != null)
                    {
                        ql.Path = qlMenu.Path + "?menuId=" + ql.MenuId;
                    }
                }
                var qlMediaType = await _context.MediaTypes.FirstOrDefaultAsync(m => m.QuickLinkId == defaultQL.Id);
                if (qlMediaType == null)
                {
                    mediaType.Contents = new List<OtherMedia>();
                }
                else
                {
                    mediaType.Contents = await _context.OtherMedia.Where(o => o.MediaTypeId == qlMediaType.Id).ToListAsync();
                }
            }
            else
            {
                mediaType.Contents = await _context.OtherMedia.Where(o => (int)o.MediaTypeId == mediaType.Id).ToListAsync();
            }
            if (mediaType.Contents.Count > 1)
            {
                mediaType.Contents = mediaType.Contents.Select(c => new OtherMedia
                {
                    Id = c.Id,
                    Title = c.Title,
                    TitleLocal = c.TitleLocal,
                    Brief = c.Brief,
                    BriefLocal = c.BriefLocal
                }).ToList();
            }
            mediaType.Menu = null;
            mediaType.QuickLink = null;
            return mediaType;
        }

        public async Task<MediaType> MediaTypeByUrl(string url)
        {
            var menu = await _context.Menus.FirstOrDefaultAsync(m => m.Path == "/" + url && m.Children.Count <= 0);
            var mediaType = await _context.MediaTypes.FirstOrDefaultAsync(m => m.MenuId == MenuId);
            if (menu == null || mediaType == null) return null;
            var menuQLHeader = await _context.QuickLinkHeaders.Include(h => h.QuickLinks.OrderBy(s => s.Order)).FirstOrDefaultAsync(h => h.Id == menu.HeaderId);
            mediaType.QuickLinkHeader = menuQLHeader;
            var defaultQL = menuQLHeader?.QuickLinks.FirstOrDefault(a => a.MenuId == menu.Id);
            if (menuQLHeader != null && defaultQL != null)
            {
                foreach (var ql in menuQLHeader.QuickLinks.Where(q => q.MenuId != null))
                {
                    var qlMenu = await _context.Menus.FirstOrDefaultAsync(m => m.Id == ql.MenuId);
                    if (qlMenu != null)
                    {
                        ql.Path = qlMenu.Path;// + "?menuId=" + ql.MenuId;
                    }
                }
                var qlMediaType = await _context.MediaTypes.FirstOrDefaultAsync(m => m.QuickLinkId == defaultQL.Id);
                if (qlMediaType == null)
                {
                    mediaType.Contents = new List<OtherMedia>();
                }
                else
                {
                    mediaType.Contents = await _context.OtherMedia.Where(o => o.MediaTypeId == qlMediaType.Id).ToListAsync();
                }
            }
            else
            {
                mediaType.Contents = await _context.OtherMedia.Where(o => (int)o.MediaTypeId == mediaType.Id).ToListAsync();
            }
            if (mediaType.Contents.Count > 1)
            {
                mediaType.Contents = mediaType.Contents.Select(c => new OtherMedia
                {
                    Id = c.Id,
                    Title = c.Title,
                    TitleLocal = c.TitleLocal,
                    Brief = c.Brief,
                    BriefLocal = c.BriefLocal
                }).ToList();
            }
            mediaType.Menu = new Menu { Title = menu.Title, TitleLocal = menu.TitleLocal };
            mediaType.QuickLink = null;
            return mediaType;
        }

        public async Task<List<OtherMedia>> QuickLinkContents(int quickLinkId)
        {
            var qlMediaType = await _context.MediaTypes.FirstOrDefaultAsync(m => m.QuickLinkId == quickLinkId);
            var contents = await _context.OtherMedia.Where(o => (int)o.MediaTypeId == qlMediaType.Id).ToListAsync();
            if (contents.Count > 1)
            {
                contents = contents.Select(c => new OtherMedia
                {
                    Id = c.Id,
                    Title = c.Title,
                    TitleLocal = c.TitleLocal,
                    Brief = c.Brief,
                    BriefLocal = c.BriefLocal,
                    MediaType = null
                }).ToList();
            }
            return contents;
        }
        public async Task<OtherMedia> GetContent(Guid id)
        {
            return await _context.OtherMedia.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<Menu>> MenusForWeb()
        {
            return await _context.Menus.Include(m => m.Children.Where(c => c.ShowOnMainMenu)).Where(m => m.ParentId == null && m.ShowOnMainMenu).ToListAsync();
        }
      
        public async Task Save(QuickLink data)
        {
            var tmp = await _context.QuickLinks.FirstOrDefaultAsync(s => s.Id == data.Id);

            if (tmp == null)
            {
                _context.QuickLinks.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLink(int headerId, int id)
        {
            var tmp = await _context.QuickLinks.FirstOrDefaultAsync(s => s.Id == id && s.HeaderId == headerId);
            if (tmp == null)
            {
                return;
            }

            _context.QuickLinks.Remove(tmp);

            await _context.SaveChangesAsync();
        }

     
        public async Task<List<MediaType>> BmoTypes()
        {
            var lst = await _context.MediaTypes.Include(m => m.Menu).Include(q=> q.QuickLink).Where(m=> m.QuickLink.HeaderId == HeaderId || (m.MenuId.HasValue && m.MenuId == MenuId)).ToListAsync();
            var menus = await _context.Menus.Where(m => m.ParentId.HasValue || m.Children.Count <= 0).ToListAsync();
            var links = await _context.QuickLinks.ToListAsync();
            foreach (var mediaType in lst)
            {
                if (mediaType.MenuId.HasValue)
                {
                    mediaType.Menu = menus.FirstOrDefault(m => m.Id == mediaType.MenuId.Value);
                }
                if (mediaType.QuickLinkId.HasValue)
                {
                    mediaType.QuickLink = links.FirstOrDefault(m => m.Id == mediaType.QuickLinkId.Value);
                }
            }

            return lst;
        }
        public async Task Save(MediaType data)
        {
            var tmp = await _context.MediaTypes.FirstOrDefaultAsync(s => s.Id == data.Id);
            data.NameLocal = string.IsNullOrEmpty(data.NameLocal) ? string.Empty : data.NameLocal;
            if (tmp == null || data.Id <= 0)
            {
                var max = await _context.MediaTypes.OrderByDescending(r => r.Id).FirstOrDefaultAsync();
                data.Id = max.Id + 1;
                var qlink =new QuickLink()
                {
                    Title = data.Name,
                    TitleLocal = data.NameLocal,
                    Order = data.Id,
                    HeaderId = HeaderId
                };
                _context.QuickLinks.Add(qlink);
               await _context.SaveChangesAsync();
                qlink = await _context.QuickLinks.FirstOrDefaultAsync(r=> r.Order== data.Id && r.Title == data.Name && r.TitleLocal == data.NameLocal);
                data.QuickLinkId = qlink.Id;
                data.MediaAuthor = Model.Common.ECCSAModuleEnum.Bmo;
                _context.MediaTypes.Add(data);
            }
            else
            {
                tmp.Name= data.Name;
                tmp.NameLocal= data.NameLocal;
                _context.MediaTypes.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMediaType(int id)
        {
            var tmp = await _context.MediaTypes.FirstOrDefaultAsync(s => s.Id == id);
            var contents = await _context.OtherMedia.AnyAsync(s => s.MediaTypeId == id);
           
            if (contents)
            {
                throw new InvalidOperationException("Remove Content First");
            }
            if (tmp == null)
            {
                return;
            }
            var qlik = await _context.QuickLinks.FirstOrDefaultAsync(l => l.Id == tmp.QuickLinkId);
            _context.MediaTypes.Remove(tmp);
            _context.QuickLinks.Remove(qlik);

            await _context.SaveChangesAsync();
        }

        public async Task<List<OtherMedia>> GetContentsByType(int bmoTypeId)
        {
            return await _context.OtherMedia.Include(m => m.MediaType).Where(m=> m.MediaTypeId == bmoTypeId).ToListAsync();
        }
    }
}


