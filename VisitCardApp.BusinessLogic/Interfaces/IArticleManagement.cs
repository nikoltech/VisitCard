namespace VisitCardApp.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Models;

    public interface IArticleManagement
    {
        Task<ArticleModel> CreateArticleAsync(ArticleModel model, string fileFolderPath);

        Task<ArticleModel> GetArticleByIdAsync(int articleId);

        Task<List<ArticleModel>> GetArticleListAsync(int page, int count, int categoryId);

        Task<ArticleModel> UpdateArticleAsync(ArticleModel model);

        Task<bool> RemoveArticleAsync(int articleId);

        Task<CommentModel> AddCommentAsync(CommentModel model);

        Task<bool> RemoveCommentAsync(int commentId);
    }
}
