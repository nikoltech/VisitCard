namespace VisitCardApp.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.DataAccess.Entities;

    public interface IArticleManagement
    {
        Task<ArticleModel> CreateArticleAsync(ArticleModel model);

        Task<ArticleModel> GetArticleAsync(int articleId);

        Task<List<ArticleModel>> GetArticleListAsync(int page, int count);

        Task<ArticleModel> UpdateArticleAsync(ArticleModel model);

        Task<bool> RemoveProjectCaseAsync(int articleId);
    }
}
