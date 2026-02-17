using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Website
{
    public class GalleryRepository : BaseRepository
	{
		private readonly EccsaContext _context;

		public GalleryRepository(EccsaContext context) : base(context)
		{
			_context = context;
		}
		public async Task<List<PhotoAlbum>> PhotoAlbumList()
		{
			return await _context.PhotoAlbums.OrderBy(s => s.Title).ToListAsync();
		}
		public async Task<PhotoAlbum> AlbumById(Guid id)
		{
			return await _context.PhotoAlbums.FirstOrDefaultAsync(l => l.Id == id);
		}
		public async Task SaveAlbum(PhotoAlbum data)
		{
			if(_context.PhotoAlbums.Any(s => s.Title.ToLower() == data.Title.ToLower() && s.Id != data.Id))
            {
				throw new InvalidOperationException("Title Already Used by another Album.");
            }
			var tmp = await _context.PhotoAlbums.FirstOrDefaultAsync(l => l.Id == data.Id);
			if (tmp == null)
			{
				if (data.Id == Guid.Empty)
				{
					data.Id = Guid.NewGuid();
				}

				_context.PhotoAlbums.Add(data);
			}
			else
			{
				_context.Entry(tmp).CurrentValues.SetValues(data);
			}
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAlbum(Guid id)
		{
			var tmp = await AlbumById(id);
			if (tmp == null) return;
			_context.PhotoAlbums.Remove(tmp);
			await _context.SaveChangesAsync();
		}

		public async Task<List<VideoGallery>> VideoGalleryList()
		{
			return await _context.VideoGalleries.OrderBy(s => s.Title).ToListAsync();
		}
		public async Task<VideoGallery> VideoGalleryById(Guid id)
		{
			return await _context.VideoGalleries.FirstOrDefaultAsync(l => l.Id == id);
		}
		public async Task SaveVideoGallery(VideoGallery data)
		{
			var tmp = await _context.VideoGalleries.FirstOrDefaultAsync(l => l.Id == data.Id);
			if (tmp == null)
			{
				if (data.Id == Guid.Empty)
				{
					data.Id = Guid.NewGuid();
				}

				_context.VideoGalleries.Add(data);
			}
			else
			{
				_context.Entry(tmp).CurrentValues.SetValues(data);
			}
			await _context.SaveChangesAsync();
		}

		public async Task DeleteVideoGallery(Guid id)
		{
			var tmp = await VideoGalleryById(id);
			if (tmp == null) return;
			_context.VideoGalleries.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}
