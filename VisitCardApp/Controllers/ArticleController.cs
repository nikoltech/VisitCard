namespace VisitCardApp.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Models;

    // TODO: Article model, entity add ImageMimeType. Rewrite as project entity
    public class ArticleController : Controller
    {
        private readonly IArticleManagement articleManagement;

        public ArticleController(IArticleManagement articleManagement)
        {
            this.articleManagement = articleManagement;
        }

        // TODO
        [HttpGet("GetArticleList")]
        public async Task<IActionResult> GetArticleListAsync(int page = 1, int count = 5)
        {

            try
            {


                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        // TODO
        [HttpGet("GetArticleById")]
        public async Task<IActionResult> GetArticleByIdAsync(int articleId)
        {

            try
            {
                

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        // TODO
        [HttpPost("CreateArticle")]
        public async Task<IActionResult> CreateArticleAsync(ArticleModel model)
        {

            try
            {
                

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        // TODO
        [HttpPatch("UpdateArticle")]
        public async Task<IActionResult> UpdateArticleAsync(ArticleModel model)
        {

            try
            {
                

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        // TODO
        [HttpDelete("RemoveArticle")]
        public async Task<IActionResult> RemoveArticleAsync(int articleId)
        {

            try
            {
                

                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }
    }
}