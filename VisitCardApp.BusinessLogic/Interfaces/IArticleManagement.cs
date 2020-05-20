namespace VisitCardApp.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Models;

    public interface IArticleManagement
    {
        Task<ArticleModel> CreateArticleAsync(ArticleModel model);

        Task<ArticleModel> GetArticleByIdAsync(int articleId);

        Task<List<ArticleModel>> GetArticleListAsync(int page, int count);

        Task<ArticleModel> UpdateArticleAsync(ArticleModel model);

        Task<bool> RemoveArticleAsync(int articleId);
    }
}
