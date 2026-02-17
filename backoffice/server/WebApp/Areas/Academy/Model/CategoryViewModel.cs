
using ECCSA.BackOffice.Server.Model.Academy;
using System.Collections.Generic;
using System.Linq;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model
{
    public class CategoryViewModel : AcademyViewModel<Category>
    {

        public List<Lookup> CategoryLookup
        {
            get { var tmp = TheList?.Where(c => TheModel == null || c.Id != TheModel.Id).Select(i => new Lookup
            { Id = i.Id, Name = $"{i.Code}-{i.NameEng} " }
        ).ToList();
                //tmp.Insert(0, new Lookup() { Id = 0, Name =string.Empty });
                return tmp;
            }
        }
        
    }
}
