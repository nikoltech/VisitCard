namespace VisitCardApp.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Models;

    [Route("{controller}")]
    public class ProjectController : Controller
    {
        private readonly IProjectManagement projectManagement;
        private readonly IWebHostEnvironment appEnvironment;

        public ProjectController(IProjectManagement projectManagement, IWebHostEnvironment appEnvironment)
        {
            this.projectManagement = projectManagement;
            this.appEnvironment = appEnvironment;
        }

        [HttpGet("List/{page}/{count}")]
        public async Task<IActionResult> ListAsync(int page = 1, int count = 6)
        {
            try
            {
                List<ProjectCaseModel> models = await this.projectManagement.GetProjectCaseListAsync(page, count);

                await this.AttachProjectCaseListFilesAsync(models).ConfigureAwait(false);

                return View("List", models);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {

            try
            {
                ProjectCaseModel model = await this.projectManagement.GetProjectCaseByIdAsync(id);

                await this.AttachProjectCaseFilesAsync(model).ConfigureAwait(false);

                return View("ProjectPage", model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpGet("Create")]
        public IActionResult CreateAsync()
        {
            return View("Create", new ProjectViewModel());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(ProjectViewModel reqModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", reqModel);
            }

            try
            {
                ProjectCaseModel model = reqModel.ToAppModel();
                if (reqModel.ImageFile != null)
                {
                    using (BinaryReader reader = new BinaryReader(reqModel.ImageFile.OpenReadStream()))
                    {
                        model.ImageFileName = reqModel.ImageFile.FileName;
                        model.ImageMimeType = reqModel.ImageFile.ContentType;
                        model.Image = reader.ReadBytes((int)reqModel.ImageFile.Length);
                    }
                }

                var result = await this.projectManagement.CreateProjectCaseAsync(model);

                await this.SaveProjectFilesAsync(model).ConfigureAwait(false);

                TempData["Created"] = result != null;

                return RedirectToAction("List", new { page = 1, count = 6 });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            try
            {
                ProjectCaseModel model = await this.projectManagement.GetProjectCaseByIdAsync(id);

                return View("Update", model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpdateAsync(ProjectCaseModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", model);
            }

            try
            {
                ProjectCaseModel updatedModel = await this.projectManagement.UpdateProjectCaseAsync(model);

                await this.UpdateDescriptionAsync(updatedModel).ConfigureAwait(false);

                return View("Update", updatedModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        // TODO: return url
        [HttpDelete("Remove/{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {

            try
            {
                bool result = await this.projectManagement.RemoveProjectCaseAsync(id);

                TempData["Deleted"] = result;

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        #region private methods
        private async Task UpdateDescriptionAsync(ProjectCaseModel projectCase)
        {
            if (!string.IsNullOrEmpty(projectCase.DescriptionPath))
            {
                using (var stream = new StreamWriter(projectCase.DescriptionPath, false, Encoding.UTF8))
                {
                    await stream.WriteAsync(projectCase.Description).ConfigureAwait(false);
                }
            }
        }

        private async Task SaveProjectFilesAsync(ProjectCaseModel projectCase)
        {
            string filePath = $"{this.appEnvironment.WebRootPath}/Files/ProjectFiles/{projectCase.ProjectName}_{DateTime.Now:dd_MM_yyyy__h_mm_ss}_";

            // Save image file
            if (projectCase.Image?.Length > 0)
            {
                projectCase.ImageFileName = projectCase.ImageFileName ?? throw new ArgumentNullException("Image filename cannot be null.");
                projectCase.ImageMimeType = projectCase.ImageMimeType ?? throw new ArgumentNullException("ImageMimeType cannot be null.");

                string imagePath = filePath + projectCase.ImageFileName;
                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    await stream.WriteAsync(projectCase.Image, 0, projectCase.Image.Length).ConfigureAwait(false);
                    projectCase.ImagePath = imagePath;
                }

            }

            // Save project description in file
            string descriptionPath = filePath + "Description.txt";
            using (var stream = new StreamWriter(descriptionPath, false, Encoding.UTF8))
            {
                await stream.WriteAsync(projectCase.Description ?? string.Empty);
                projectCase.DescriptionPath = descriptionPath;
            }
        }

        private async Task AttachProjectCaseListFilesAsync(IEnumerable<ProjectCaseModel>  projectCases)
        {
            foreach (ProjectCaseModel projectCase in projectCases)
            {
                await this.AttachProjectCaseFilesAsync(projectCase).ConfigureAwait(false);
            }
        }

        private async Task AttachProjectCaseFilesAsync(ProjectCaseModel projectCase)
        {
            if (!string.IsNullOrEmpty(projectCase.ImagePath))
            {
                projectCase.Image = await System.IO.File.ReadAllBytesAsync(projectCase.ImagePath).ConfigureAwait(false);
            }

            if (!string.IsNullOrEmpty(projectCase.DescriptionPath))
            {
                using (var stream = new StreamReader(projectCase.DescriptionPath, Encoding.UTF8))
                {
                    projectCase.Description = await stream.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }
        #endregion
    }
}