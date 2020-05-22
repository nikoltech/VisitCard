namespace VisitCardApp.BusinessLogic.Models
{
    using System.ComponentModel.DataAnnotations;
    using VisitCardApp.DataAccess.Entities;

    public class ProjectCaseModel : IEntityModel<ProjectCase>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Не более 200 символов")]
        public string ProjectName { get; set; }

        [Required]
        public string Description { get; set; }

        public string DescriptionPath { get; set; }

        public string ImagePath { get; set; }

        public byte[] Image { get; set; }

        public string ImageFileName { get; set; }

        public string ImageMimeType { get; set; }

        public ProjectCase ToEntity()
        {
            return new ProjectCase
            {
                Id = this.Id,
                ProjectName = this.ProjectName,
                Description = this.Description,
                DescriptionPath = this.DescriptionPath,
                Image = this.Image,
                ImagePath = this.ImagePath,
                ImageMimeType = this.ImageMimeType,
                ImageFileName = this.ImageFileName
            };
        }

        public void ToModel(ProjectCase entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.ProjectName = entity.ProjectName;
                this.Description = entity.Description;
                this.DescriptionPath = entity.DescriptionPath;
                this.Image = entity.Image;
                this.ImagePath = entity.ImagePath;
                this.ImageMimeType = entity.ImageMimeType;
                this.ImageFileName = entity.ImageFileName;
            }
        }
    }
}
