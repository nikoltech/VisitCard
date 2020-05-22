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

        [HttpGet("List/{page?}/{count?}")]
        public async Task<IActionResult> ListAsync(int? page = 1, int? count = 6)
        {
            try
            {
                List<ProjectCaseModel> models = await this.projectManagement.GetProjectCaseListAsync(page.Value, count.Value);

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

                var result = await this.projectManagement.CreateProjectCaseAsync(model, this.appEnvironment.WebRootPath);

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
        

        
        #endregion
    }
}