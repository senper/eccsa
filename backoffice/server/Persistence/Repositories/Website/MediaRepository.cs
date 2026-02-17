using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Website
{
    public class MediaRepository : BaseRepository
	{
		private readonly EccsaContext _context;

		public MediaRepository(EccsaContext context) : base(context)
		{
			_context = context;
		}
		public async Task<List<OtherMedia>> List()
		{
			return await _context.OtherMedia.Include(m=> m.MediaType).ToListAsync();
		}
        public async Task<List<MediaType>> MediaTypes()
        {
            return await _context.MediaTypes.Include(m=> m.Menu).Include(m=> m.QuickLink.Header).OrderBy(m=> m.Name).ToListAsync();
        }
        public async Task<List<OtherMedia>> List(int mediaTypeId)
		{
			return await _context.OtherMedia.Where(m => m.MediaTypeId == mediaTypeId).ToListAsync();
		}
		public async Task<OtherMedia> ById(Guid id)
		{
			return await _context.OtherMedia.Include(m=> m.MediaType).FirstOrDefaultAsync(l => l.Id == id);
		}
		public async Task<OtherMedia> View(Guid id)
		{

			var tmp = await _context.OtherMedia.FirstOrDefaultAsync(l => l.Id == id);
			tmp.Views += 1;
			_context.OtherMedia.Update(tmp);
			await _context.SaveChangesAsync();
			return tmp;
		}
		public async Task Save(OtherMedia data, Guid userId)
		{
			data.UpdateStamp = DateTime.UtcNow;
			data.UpdatedBy = userId;
			data.AuthorName ??= string.Empty;
			data.AuthorNameLocal ??= string.Empty;
			var tmp = await _context.OtherMedia.FirstOrDefaultAsync(l => l.Id == data.Id);
			if (tmp == null)
			{
				if (data.Id == Guid.Empty)
				{
					data.Id = Guid.NewGuid();
				}
				data.Content = string.Empty;
                data.ContentLocal = string.Empty;
				data.CreatedBy = userId;
				data.CreateStamp = DateTime.UtcNow;
				_context.OtherMedia.Add(data);
				
			}
			else
			{
				data.Content = String.IsNullOrWhiteSpace(data.Content) ? tmp.Content : data.Content;
                data.ContentLocal = String.IsNullOrWhiteSpace(data.ContentLocal) ? tmp.ContentLocal : data.ContentLocal;
                data.AuthorName = String.IsNullOrWhiteSpace(data.AuthorName) ? tmp.AuthorName : data.AuthorName;
                _context.Entry(tmp).CurrentValues.SetValues(data);
			}
			await GenerateKeyword(data);
			await _context.SaveChangesAsync();
		}
		public async Task Delete(Guid id)
		{
			var tmp = await ById(id);
			if (tmp == null) return;
			_context.OtherMedia.Remove(tmp);
			await RemoveKeyword(id,id);
			await _context.SaveChangesAsync();
		}

		public async Task Like(Guid id)
		{
			var tmp = await ById(id);
			if (tmp == null) return;
			tmp.Likes += 1;
			_context.OtherMedia.Update(tmp);
			await _context.SaveChangesAsync();
		}
		public async Task DisLike(Guid id)
		{
			var tmp = await ById(id);
			if (tmp == null) return;
			tmp.Dislikes += 1;
			_context.OtherMedia.Update(tmp);
			await _context.SaveChangesAsync();
		}
		public async Task AddComment(Guid id, Comment comment)
		{
			var tmp = await ById(id);
			if (tmp == null) return;
			tmp.Comments.Add(comment);
			_context.OtherMedia.Update(tmp);
			await _context.SaveChangesAsync();
		}
		public async Task RemoveComment(Guid id, string commentId)
		{
			var tmp = await ById(id);
			if (tmp == null) return;
			var comment = tmp.Comments.Find(c => c.Id == commentId);
			if (comment == null) return;
			tmp.Comments.Remove(comment);
			_context.OtherMedia.Update(tmp);
			await _context.SaveChangesAsync();
		}
	}
}


