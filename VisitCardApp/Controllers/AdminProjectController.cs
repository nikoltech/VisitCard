namespace VisitCardApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Models;

    [Authorize(Roles = "admin")]
    [Route("{controller}")]
    public class AdminProjectController : Controller
    {
        private readonly IProjectManagement projectManagement;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly ICategoryManagement categoryManagement;

        public AdminProjectController(
            IProjectManagement projectManagement,
            IWebHostEnvironment appEnvironment,
            ICategoryManagement categoryManagement)
        {
            this.projectManagement = projectManagement;
            this.appEnvironment = appEnvironment;
            this.categoryManagement = categoryManagement;
        }

        [HttpGet("List/{page?}/{count?}/{categoryId?}")]
        public async Task<IActionResult> ListAsync(int? page = 1, int? count = 6, int? categoryId = null)
        {
            try
            {
                List<ProjectCaseModel> models = await this.projectManagement.GetProjectCaseListAsync(page.Value, count.Value, categoryId.GetValueOrDefault());

                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Project).ConfigureAwait(false);
                ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

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
        public async Task<IActionResult> CreateAsync()
        {
            List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Project).ConfigureAwait(false);
            ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));
            
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(ProjectViewModel reqModel)
        {
            if (!ModelState.IsValid)
            {
                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Project).ConfigureAwait(false);
                ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

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
                ProjectViewModel vModel = new ProjectViewModel(model);

                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Project).ConfigureAwait(false);
                ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

                return View("Update", vModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpPost("UpdateProject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(ProjectViewModel reqModel)
        {
            if (!ModelState.IsValid)
            {
                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Project).ConfigureAwait(false);
                ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

                return View("Update", reqModel);
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

                ProjectCaseModel updatedModel = await this.projectManagement.UpdateProjectCaseAsync(model, this.appEnvironment.WebRootPath);

                return RedirectToAction("", new { id = reqModel.Id });
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        // TODO: return url
        [HttpPost("Remove/{id}")]
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