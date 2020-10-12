namespace VisitCardApp.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class ArticleImage
    {
        public int Id { get; set; }

        public string UrlPath { get; set; }

        public string FilePath { get; set; }

        public string ImageMimeType { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }

        [NotMapped]
        public byte[] File { get; set; }

        [NotMapped]
        public string FileName { get; set; }
    }
}
