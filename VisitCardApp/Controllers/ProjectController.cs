namespace VisitCardApp.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Helpers;
    using VisitCardApp.Models;

    [Route("{controller}")]
    public class ProjectController : Controller
    {
        private readonly IProjectManagement projectManagement;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly ICategoryManagement categoryManagement;

        public ProjectController(
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
                (List<ProjectCaseModel> projectModels, long total) = await this.projectManagement.GetProjectCaseListAsync(page.Value, count.Value, categoryId.GetValueOrDefault());

                ProjectListViewModel model = new ProjectListViewModel
                {
                    ProjectCases = projectModels,
                    CategoryId = categoryId,
                    Pagination = new PaginationModel
                    {
                        AmountPerPage = count.Value,
                        Page = page.Value,
                        TotalItems = total
                    }
                };

                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Project).ConfigureAwait(false);
                ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

                return View("List", projectModels);
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

        #region private methods
        

        
        #endregion
    }
}