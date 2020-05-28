﻿namespace VisitCardApp.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Enums;

    public interface IRepository
    {
        Task<ProjectCase> CreateProjectCaseAsync(ProjectCase projectCase, string webRootFilePath);

        Task<ProjectCase> GetProjectCaseAsync(int projectId);

        Task<List<ProjectCase>> GetProjectCaseListAsync(int page, int count, int categoryId);

        Task<ProjectCase> UpdateProjectCaseAsync(ProjectCase updatedProject, string webRootFilePath);

        Task<bool> RemoveProjectCaseAsync(int projectId);

        Task<Article> CreateArticleAsync(Article article, string webRootFilePath);

        Task<Article> GetArticleByIdAsync(int articleId);

        Task<List<Article>> GetArticleListAsync(int page, int count, int categoryId);

        Task<Article> UpdateArticleAsync(Article updatedArticle);

        Task<bool> RemoveArticleAsync(int articleId);

        Task<List<Category>> GetCategoryListAsync(CategoryType type = CategoryType.All);

        Task<Category> CreateCategoryAsync(Category category);

        Task<Category> UpdateCategoryAsync(Category category);

        Task<bool> RemoveCategoryAsync(Category category);
    }
}
