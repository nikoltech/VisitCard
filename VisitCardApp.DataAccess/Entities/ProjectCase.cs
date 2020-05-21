namespace VisitCardApp.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProjectCase
    {
        public int Id { get; set; }

        public string ProjectName { get; set; }

        public string DescriptionPath { get; set; }

        public string ImagePath { get; set; }

        public string ImageMimeType { get; set; }
    }
}
