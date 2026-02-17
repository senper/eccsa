using System;
using System.IO;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class PhotoAlbum
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasFiles()
        {
            string GalleryImagePath = Path.Combine(
                                            Directory.GetCurrentDirectory(),
                                            "wwwroot", "pr",
                                            "gallery");
            string filePath = Path.Combine(GalleryImagePath, Title);
            if (Directory.Exists(filePath))
            {
                return Directory.GetFiles(filePath).Length > 0;
            }
            return false;
        }
    }
    public class VideoGallery
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
