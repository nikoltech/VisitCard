namespace VisitCardApp.BusinessLogic.Models
{
    using System.ComponentModel.DataAnnotations;
    using VisitCardApp.DataAccess.Entities;

    public class ProjectCaseModel : IEntityModel<ProjectCase>
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string ProjectName { get; set; }

        public string Description { get; set; }

        public byte[] Image { get; set; }

        public string ImageFileName { get; set; }

        public ProjectCase ToEntity()
        {
            return new ProjectCase
            {
                Id = this.Id,
                ProjectName = this.ProjectName,
                Description = this.Description,
                Image = this.Image,
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
                this.Image = entity.Image;
                this.ImageFileName = entity.ImageFileName;
            }
        }
    }
}
