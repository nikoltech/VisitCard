namespace VisitCardApp.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Models;

    public interface IArticleManagement
    {
        Task<ArticleModel> CreateArticleAsync(ArticleModel model, string fileFolderPath);

        Task<ArticleModel> GetArticleByIdAsync(int articleId);

        Task<(List<ArticleModel> models, long total)> GetArticleListAsync(int page, int count, int categoryId);

        Task<ArticleModel> UpdateArticleAsync(ArticleModel model);

        Task<ArticleModel> UpdateArticleAsync(ArticleModel model, string userId);

        Task<bool> RemoveArticleAsync(int articleId);

        Task<bool> RemoveArticleAsync(int articleId, string userId);

        Task<CommentModel> AddCommentAsync(CommentModel model);

        Task<bool> RemoveCommentAsync(int commentId);
    }
}
