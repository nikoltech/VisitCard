﻿namespace VisitCardApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Helpers;
    using VisitCardApp.Models;

    [Route("{controller}")]
    public class ArticleController : Controller
    {
        private readonly IArticleManagement articleManagement;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly ICategoryManagement categoryManagement;

        public ArticleController(
            IArticleManagement articleManagement,
            IWebHostEnvironment appEnvironment,
            ICategoryManagement categoryManagement)
        {
            this.articleManagement = articleManagement;
            this.appEnvironment = appEnvironment;
            this.categoryManagement = categoryManagement;
        }

        [HttpGet("List/{page?}/{count?}/{categoryId?}")]
        public async Task<IActionResult> ListAsync(int? page = 1, int? count = 6, int? categoryId = null)
        {
            try
            {
                List<ArticleModel> models = await this.articleManagement.GetArticleListAsync(page.Value, count.Value, categoryId.GetValueOrDefault());

                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Article).ConfigureAwait(false);
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
                ArticleModel model = await this.articleManagement.GetArticleByIdAsync(id);

                return View("ArticlePage", model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpGet("Create")]
        public async Task<IActionResult> CreateAsync()
        {
            List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Article).ConfigureAwait(false);
            ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

            return View();
        }

        // TODO: files, ArticleViewModel
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(ArticleViewModel reqModel)
        {
            if (!ModelState.IsValid)
            {
                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Article).ConfigureAwait(false);
                ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

                return View("Create", reqModel);
            }

            try
            {
                ArticleModel model = reqModel.ToAppModel();
                model.ArticleImages = this.ReadFilesBytes(reqModel.Files);

                var result = await this.articleManagement.CreateArticleAsync(model, this.appEnvironment.WebRootPath);

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
                ArticleModel model = await this.articleManagement.GetArticleByIdAsync(id);

                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Article).ConfigureAwait(false);
                ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

                return View("Update", new ArticleViewModel(model));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpPost("UpdateArticle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAsync(ArticleViewModel reqModel)
        {
            if (!ModelState.IsValid)
            {
                List<CategoryModel> categoryList = await this.categoryManagement.GetCategoryListAsync(DataAccess.Enums.CategoryType.Article).ConfigureAwait(false);
                ViewData["Categories"] = new SelectList(categoryList, nameof(CategoryModel.Id), nameof(CategoryModel.Name));

                return View("Update", reqModel);
            }

            try
            {
                ArticleModel updatedModel = await this.articleManagement.UpdateArticleAsync(reqModel.ToAppModel());

                return RedirectToAction("", new { id = updatedModel.Id });
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
                bool result = await this.articleManagement.RemoveArticleAsync(id);

                TempData["Deleted"] = result;

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        #region private methods
        private List<ArticleImageModel> ReadFilesBytes(FormFileCollection files)
        {
            List<ArticleImageModel> images = new List<ArticleImageModel>();
            foreach (IFormFile file in files)
            {
                if (file != null)
                {
                    using (BinaryReader reader = new BinaryReader(file.OpenReadStream()))
                    {
                        byte[] img = reader.ReadBytes((int)file.Length);
                        images.Add(new ArticleImageModel { FileName = file.FileName, ImageMimeType = file.ContentType, File = img });
                    }
                }
            }

            return images;
        }


        #endregion
    }
}