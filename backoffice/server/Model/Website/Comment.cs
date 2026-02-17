using System;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class Comment
    {
        public static Comment Default => new Comment 
        { 
            Author = Guid.Empty,
            Text = String.Empty,
            AuthorName = String.Empty,
            DateCreated = DateTime.Now,
            Dislikes = 0,
            Id = String.Empty
        };
        public String Id { get; set; } = "";
        public String Text { get; set; } = string.Empty;

        public Guid Author { get; set; } = Guid.Empty;

        public String AuthorName { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Today;

        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;

    }
}