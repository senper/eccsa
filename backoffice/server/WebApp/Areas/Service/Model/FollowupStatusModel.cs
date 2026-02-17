using ECCSA.BackOffice.Server.Model.Common.Servicing;
using Microsoft.AspNetCore.Http;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{

    public class FollowupStatusModel : ServiceFollowup
    {
        public IFormFile File { get; set; }
       
    }
    
}
