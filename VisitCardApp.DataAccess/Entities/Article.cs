namespace VisitCardApp.DataAccess.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using VisitCardApp.DataAccess.Helpers;

    public class Article
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public string TextPath { get; set; }

        public List<ArticleImage> ArticleImagesPath { get; set; }

        [NotMapped]
        public string Text { get; set; }

        [NotMapped]
        public List<FileHelper> ArticleImages { get; set; }
    }
}
