namespace VisitCardApp.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using VisitCardApp.BusinessLogic.Models;

    public class ProjectViewModel
    {
        public ProjectViewModel() { }

        public ProjectViewModel(ProjectCaseModel projectCaseModel)
        {
            this.Id = projectCaseModel.Id;
            this.ProjectName = projectCaseModel.ProjectName;
            this.Description = projectCaseModel.Description;
        }

        public int Id { get; set; }

        [Required()]
        [MaxLength(200, ErrorMessage = "Не более 200 символов")]
        [Display(Name = "Название")]
        public string ProjectName { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Изображение")]
        public IFormFile ImageFile { get; set; }

        public ProjectCaseModel ToAppModel()
        {
            return new ProjectCaseModel
            {
                Id = this.Id,
                ProjectName = this.ProjectName,
                Description = this.Description
            };
        }
    }
}
