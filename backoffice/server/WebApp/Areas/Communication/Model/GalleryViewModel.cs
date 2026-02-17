using ECCSA.BackOffice.Server.Model.Website;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model
{
    public class PhotoAlbumViewModel : CommunicationViewModel<PhotoAlbum>
    {
        public string Controller => "Gallery";
    }
    public class VideoGalleryViewModel : CommunicationViewModel<VideoGallery>
    {
        public string Controller => "Gallery";
    }
}
