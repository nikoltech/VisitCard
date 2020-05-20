namespace VisitCardApp.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;

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

            return View();
        }

        // TODO
        [HttpGet("GetArticleById")]
        public async Task<IActionResult> GetArticleByIdAsync(int articleId)
        {

            return View();
        }

        // TODO
        [HttpPost("CreateArticle")]
        public async Task<IActionResult> CreateArticleAsync(ArticleModel model)
        {

            return View();
        }

        // TODO
        [HttpPatch("UpdateArticle")]
        public async Task<IActionResult> UpdateArticleAsync(ArticleModel model)
        {

            return View();
        }

        // TODO
        [HttpDelete("RemoveArticle")]
        public async Task<IActionResult> RemoveArticleAsync(int articleId)
        {

            return View();
        }
    }
}