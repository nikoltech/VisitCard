namespace VisitCardApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Enums;
    using VisitCardApp.Models;

    [Authorize(Roles = "admin")]
    [Route("{controller}")]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryManagement categoryManagement;

        public AdminCategoryController(ICategoryManagement categoryManagement)
        {
            this.categoryManagement = categoryManagement;
        }

        [HttpGet("List")]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                List<CategoryModel> models = await this.categoryManagement.GetCategoryListAsync().ConfigureAwait(false);

                return View(models);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpGet("Create")]
        public IActionResult CreateAsync()
        {
            ViewData["CategoryTypes"] = this.GetCategoryTypeSelectList();

            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(CategoryModel model)
        {
            if (!this.ModelState.IsValid)
            {
                ViewData["CategoryTypes"] = this.GetCategoryTypeSelectList();

                return View(model);
            }

            try
            {
                var result = await this.categoryManagement.CreateCategoryAsync(model).ConfigureAwait(false);

                TempData["Created"] = result != null;

                return RedirectToAction("List");
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
                CategoryModel model = await this.categoryManagement.GetCategoryByIdAsync(id).ConfigureAwait(false);

                ViewData["CategoryTypes"] = this.GetCategoryTypeSelectList();

                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(CategoryModel model)
        {
            if (!this.ModelState.IsValid)
            {
                ViewData["CategoryTypes"] = this.GetCategoryTypeSelectList();

                return View(model);
            }

            try
            {
                var result = await this.categoryManagement.UpdateCategoryAsync(model).ConfigureAwait(false);

                TempData["Updated"] = result != null;

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpPost("Remove/{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            try
            {
                bool result = await this.categoryManagement.RemoveCategoryAsync(id);

                TempData["Deleted"] = result;

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        #region private methods
        private IEnumerable<SelectListItem> GetCategoryTypeSelectList()
        {
            var types = Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().ToList().Where(x => !x.Equals(CategoryType.None)).Select(x => new SelectListItem
            {
                Text = x.ToString(),
                Value = ((int)x).ToString()
            }).ToList();

            return types;

            // var items = Enum.GetValues(typeof(CategoryType)).Cast<CategoryType>().Where(x => !x.Equals(CategoryType.None)).Select(x => new 
            //{
            //    Text = x.ToString(),
            //    Value = (int)x
            //}).ToList();

            // return new SelectList(types, "Value", "Text");
        }
        #endregion

    }
}
