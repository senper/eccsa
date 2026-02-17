using ECCSA.BackOffice.Server.Model.Advocacy;
using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ECCSA.BackOffice.Server.Model.Communication
{
    public class Resource : Entity
	{
		public String Title { get; set; }

        public String TitleLocal { get; set; }

		[NotMapped]
		public String TitleFull => $"{Title} {TitleLocal}";
        public ResourceType Type { get; set; }

        public Guid TypeId { get; set; }

        public int TypeValue => (int)Type;
		public String Description { get; set; }

        public String DescriptionLocal { get; set; }

        public String Author { get; set; }

        public String AuthorLocal { get; set; }
        [NotMapped]
        public String AuthorFull => $"{Author} {AuthorLocal}";

        public DateTime PublicationDate { get; set; }
		[NotMapped]
		public string PublicationDateStr => PublicationDate.ToString("yyyy-MM-dd");

		public string Url { get; set; }
		[NotMapped]
		public bool HasUrlForUserRequest { get; set; }
		 
		public string Extension { get; set; }

		public decimal? DigitalCost { get; set; }

		public string PrintUri { get; set; }
		public decimal? PrintCost { get; set; }
		public ECCSAModuleFlag OwnerModule { get; set; }
		public string Profile { get; set; }

        public string ProfileLocal { get; set; }

        public static Resource FromAdvocacy(Publication p)
        {
			return new Resource
			{
				Title = p.Title,
				PrintCost = p.PrintCost,
				DigitalCost = p.DigitalCost,
				Description = p.Description,
				CreateStamp = p.CreateStamp,
				Author = p.Author,
				CreatedBy = p.CreatedBy,
				Id = p.Id,
				PrintUri = p.PrintUri,
				PublicationDate = p.PublicationDate,
				Type = ResourceType.Publications,
				UpdatedBy = p.UpdatedBy,
				UpdateStamp = p.UpdateStamp,
				Url = p.Url,
			};
        }

		public bool FileExists()
		{
			if (string.IsNullOrWhiteSpace(Extension)) return false;
			string path = Path.Combine(
											Directory.GetCurrentDirectory(),
											"wwwroot", 
											OwnerModule == ECCSAModuleFlag.Communication ?
											"pr" : OwnerModule.ToString().ToLower(),
											"documents");
			string filePath = Path.Combine(path, Id.ToString()[^15..].Replace("-", ""));

			return File.Exists(filePath + Extension);
		}
		[NotMapped]
		public bool HasFile => FileExists();
	}

	public record ResourceDescription(Guid Id, string Title, ResourceType Type);

}