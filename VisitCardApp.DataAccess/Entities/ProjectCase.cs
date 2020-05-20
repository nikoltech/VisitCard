namespace VisitCardApp.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ProjectCase
    {
        public int Id { get; set; }

        public string ProjectName { get; set; }

        public string DescriptionPath { get; set; }

        public string ImagePath { get; set; }

        public string ImageFileName { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public byte[] Image { get; set; }
    }
}
