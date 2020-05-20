namespace VisitCardApp.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.DataAccess.Entities;

    public interface IRepository
    {
        Task<ProjectCase> CreateProjectCaseAsync(ProjectCase projectCase);

        Task<ProjectCase> GetProjectCaseAsync(int projectId);

        Task<List<ProjectCase>> GetProjectCaseListAsync(int page, int count);

        Task<ProjectCase> UpdateProjectCaseAsync(ProjectCase updatedProject);

        Task<bool> RemoveProjectCaseAsync(int projectId);

        Task<Article> CreateArticleAsync(Article article);

        Task<Article> GetArticleAsync(int articleId);

        Task<List<Article>> GetArticleListAsync(int page, int count);

        Task<Article> UpdateArticleAsync(Article updatedArticle);

        Task<bool> RemoveArticleAsync(int articleId);
    }
}
