namespace VisitCardApp.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProjectCase
    {
        public int Id { get; set; }

        public string ProjectName { get; set; }

        public string DescriptionPath { get; set; }

        public string UrlPath { get; set; }

        public string ImagePath { get; set; }

        public string ImageMimeType { get; set; }

        public string ImageFileName { get; set; }

        public decimal Cost { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public byte[] Image { get; set; }
    }
}
