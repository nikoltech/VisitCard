﻿namespace VisitCardApp.DataAccess.Repositories
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Enums;

    public class Repository : IRepository
    {
        private readonly DataContext context;

        private readonly UserManager<AppUser> userManager;

        public Repository(DataContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        #region ProjectCase

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectCase"></param>
        /// <param name="fileFolderPath"></param>
        /// <returns>null if fail</returns>
        public async Task<ProjectCase> CreateProjectCaseAsync(ProjectCase projectCase, string webRootFilePath)
        {
            projectCase = projectCase ?? throw new ArgumentNullException(nameof(projectCase));
            webRootFilePath = webRootFilePath ?? throw new ArgumentNullException(nameof(webRootFilePath));

            try
            {
                projectCase = await this.SaveProjectFilesAsync(projectCase, webRootFilePath).ConfigureAwait(false);

                this.context.ProjectCases.Add(projectCase);

                if (await this.context.SaveChangesAsync() > 0)
                {
                    return projectCase;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Project with id not found</exception>
        /// <exception cref="ArgumentNullException">projectId</exception>
        public async Task<ProjectCase> GetProjectCaseAsync(int projectId)
        {
            if (projectId == 0)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            try
            {
                ProjectCase projectCase = await this.context.ProjectCases.Where(p => p.Id == projectId).FirstOrDefaultAsync();

                if (projectCase == null)
                {
                    throw new Exception($"Project with id {projectId} not found.");
                }

                await this.AttachProjectCaseFilesAsync(projectCase).ConfigureAwait(false);

                return projectCase;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <param name="categoryId"></param>
        /// <returns>CategoryType.All`s data if categoryId is zero</returns>
        public async Task<(List<ProjectCase> entities, long total)> GetProjectCaseListAsync(int page, int count, int categoryId)
        {
            try
            {
                int skip = this.SkipSize(page, count);

                IQueryable<ProjectCase> projectCasesQuery = this.context.ProjectCases;

                Category category = null;
                if (categoryId != 0)
                {
                    category = await this.context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                }
                else
                {
                    category = await this.context.Categories.FirstOrDefaultAsync(c => c.Type == CategoryType.All);
                }

                if (category != null && category.Type != CategoryType.All)
                {
                    projectCasesQuery = projectCasesQuery.Where(p => p.CategoryId == categoryId);
                }

                long total = await projectCasesQuery.CountAsync();

                List<ProjectCase> projectCases = await projectCasesQuery.Skip(skip).Take(count).ToListAsync();

                await this.AttachProjectCaseListFilesAsync(projectCases).ConfigureAwait(false);

                return (projectCases, total);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatedProject"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Project with id not found</exception>
        /// <exception cref="ArgumentNullException">updatedProject</exception>
        public async Task<ProjectCase> UpdateProjectCaseAsync(ProjectCase updatedProject, string webRootFilePath)
        {
            updatedProject = updatedProject ?? throw new ArgumentNullException(nameof(updatedProject));
            webRootFilePath = webRootFilePath ?? throw new ArgumentNullException(nameof(webRootFilePath));

            try
            {
                ProjectCase projectCase = await this.context.ProjectCases.Where(p => p.Id == updatedProject.Id).FirstOrDefaultAsync();

                if (projectCase == null)
                {
                    throw new Exception($"Project with id {updatedProject.Id} not found.");
                }

                projectCase.ProjectName = updatedProject.ProjectName;

                if (!string.IsNullOrEmpty(projectCase.DescriptionPath))
                {
                    await this.SaveTextFileAsync(projectCase.DescriptionPath, updatedProject.Description).ConfigureAwait(false);
                }

                if (updatedProject.Image?.Length > 0)
                {
                    updatedProject.ImageMimeType = updatedProject.ImageMimeType ?? throw new ArgumentNullException("ImageMimeType cannot be null.");

                    if (!string.IsNullOrEmpty(projectCase.ImagePath) && File.Exists(projectCase.ImagePath))
                    {
                        File.Delete(projectCase.ImagePath);
                    }

                    string folderPath = this.GetProjectFolderPath(projectCase);
                    projectCase.Image = updatedProject.Image;
                    projectCase = await this.SaveProjectImageAsync(projectCase, webRootFilePath, folderPath).ConfigureAwait(false);
                }

                if (this.context.Entry(projectCase).State == EntityState.Unchanged || await this.context.SaveChangesAsync() > 0)
                {
                    return projectCase;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RemoveProjectCaseAsync(int projectId)
        {
            if (projectId == 0)
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            try
            {
                ProjectCase projectCase = await this.context.ProjectCases.Where(p => p.Id == projectId).FirstOrDefaultAsync();

                if (projectCase == null)
                {
                    return true;
                }

                if (!string.IsNullOrEmpty(projectCase.ImagePath) && File.Exists(projectCase.ImagePath))
                {
                    File.Delete(projectCase.ImagePath);
                }

                if (!string.IsNullOrEmpty(projectCase.DescriptionPath) && File.Exists(projectCase.DescriptionPath))
                {
                    File.Delete(projectCase.DescriptionPath);
                }

                this.context.ProjectCases.Remove(projectCase);

                return await this.context.SaveChangesAsync() > 0;
            }
            catch
            {
                throw;
            }
        }

        #region Project Image

        #endregion

        #endregion

        // TODO: Update images
        #region Articles
        public async Task<Article> CreateArticleAsync(Article article, string webRootFilePath)
        {
            article = article ?? throw new ArgumentNullException(nameof(article));
            webRootFilePath = webRootFilePath ?? throw new ArgumentNullException(nameof(webRootFilePath));

            try
            {
                AppUser user = await this.context.Users.Where(u => u.Id.Equals(article.UserId)).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                article = await this.SaveArticleFilesAsync(article, webRootFilePath).ConfigureAwait(false);

                this.context.Articles.Add(article);

                if (await this.context.SaveChangesAsync() > 0)
                {
                    return article;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Article> GetArticleByIdAsync(int articleId)
        {
            if (articleId == 0)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            try
            {
                Article article = await this.context.Articles
                    .Include(a => a.ArticleImages)
                    .Include(a => a.Comments)
                    .Where(p => p.Id == articleId).FirstOrDefaultAsync();

                if (article == null)
                {
                    throw new Exception($"Project with id {articleId} not found.");
                }

                article.Text = await this.GetTextFileAsync(article.TextPath).ConfigureAwait(false);

                return article;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(List<Article> entities, long total)> GetArticleListAsync(int page, int count, int categoryId)
        {
            try
            {
                int skip = this.SkipSize(page, count);

                IQueryable<Article> articlesQuery = this.context.Articles;

                Category category = null;
                if (categoryId != 0)
                {
                    category = await this.context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                }
                else
                {
                    category = await this.context.Categories.FirstOrDefaultAsync(c => c.Type == CategoryType.All);
                }

                if (category != null && category.Type != CategoryType.All)
                {
                    articlesQuery = articlesQuery.Where(p => p.CategoryId == categoryId);
                }

                long total = await articlesQuery.CountAsync();

                List<Article> articles = await articlesQuery
                    .Include(a => a.ArticleImages)
                    .Include(a => a.Comments)
                    .Skip(skip).Take(count).ToListAsync();

                foreach (Article article in articles)
                {
                    article.Text = await this.GetTextFileAsync(article.TextPath).ConfigureAwait(false);
                }

                return (articles, total);
            }
            catch
            {
                throw;
            }
        }

        // updates without images
        public async Task<Article> UpdateArticleAsync(Article updatedArticle, string userId)
        {
            updatedArticle = updatedArticle ?? throw new ArgumentNullException(nameof(updatedArticle));
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            try
            {
                Article article = await this.context.Articles.Where(p => p.Id == updatedArticle.Id && p.UserId.Equals(userId)).FirstOrDefaultAsync();

                if (article == null)
                {
                    throw new Exception($"Project with id {updatedArticle.Id} not found.");
                }

                return await this.UpdateArticleAsync(updatedArticle, article).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<Article> UpdateArticleAsync(Article updatedArticle)
        {
            updatedArticle = updatedArticle ?? throw new ArgumentNullException(nameof(updatedArticle));

            try
            {
                Article article = await this.context.Articles.Where(p => p.Id == updatedArticle.Id).FirstOrDefaultAsync();

                if (article == null)
                {
                    throw new Exception($"Project with id {updatedArticle.Id} not found.");
                }

                return await this.UpdateArticleAsync(updatedArticle, article).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        private async Task<Article> UpdateArticleAsync(Article updatedArticle, Article article)
        {
            updatedArticle = updatedArticle ?? throw new ArgumentNullException(nameof(updatedArticle));

            try
            {
                if (!string.IsNullOrEmpty(article.TextPath))
                {
                    await this.SaveTextFileAsync(article.TextPath, updatedArticle.Text).ConfigureAwait(false);
                }

                article.Topic = updatedArticle.Topic;

                if (this.context.Entry(article).State == EntityState.Unchanged || await this.context.SaveChangesAsync() > 0)
                {
                    return article;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RemoveArticleAsync(int articleId, string userId)
        {
            userId = userId ?? throw new ArgumentNullException(nameof(userId));
            if (articleId == 0)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            Article article = await this.context.Articles
                    .Include(a => a.ArticleImages)
                    .Where(a => a.Id == articleId && a.UserId.Equals(userId)).FirstOrDefaultAsync();

            if (article == null)
            {
                throw new Exception("Article does not exists.");
            }

            return await this.RemoveArticleAsync(article).ConfigureAwait(false);
        }

        public async Task<bool> RemoveArticleAsync(int articleId)
        {
            if (articleId == 0)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            try
            {
                Article article = await this.context.Articles
                    .Include(a => a.ArticleImages)
                    .Where(p => p.Id == articleId).FirstOrDefaultAsync();

                if (article == null)
                {
                    throw new Exception("Article does not exists.");
                }

                return await this.RemoveArticleAsync(article).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> RemoveArticleAsync(Article article)
        {
            article = article ??  throw new ArgumentNullException(nameof(article));

            try
            {
                foreach (ArticleImage im in article.ArticleImages)
                {
                    if (!string.IsNullOrEmpty(im.FilePath))
                    {
                        File.Delete(im.FilePath);
                    }
                }

                this.RemoveFile(article.TextPath);

                this.context.ArticleImages.RemoveRange(article.ArticleImages);
                this.context.Articles.Remove(article);

                return await this.context.SaveChangesAsync() > 0;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Comment> AddArticleCommentAsync(Comment comment)
        {
            comment = comment ?? throw new ArgumentNullException(nameof(comment));

            try
            {
                AppUser user = await this.context.Users.Where(u => u.Id.Equals(comment.UserId)).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                Article article = await this.GetArticleByIdAsync(comment.ArticleId).ConfigureAwait(false);

                if (article == null)
                {
                    throw new Exception($"Article not found.");
                }

                comment.UserName = user.UserName;

                article.Comments.Add(comment);

                if (await this.context.SaveChangesAsync() > 0)
                {
                    return comment;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RemoveArticleCommentAsync(int commentId)
        {
            try
            {
                Comment comment = await this.context.Comments.Where(c => c.Id == commentId).FirstOrDefaultAsync();

                if (comment == null)
                {
                    throw new Exception("Comment does not exist.");
                }

                this.context.Comments.Remove(comment);

                if (await this.context.SaveChangesAsync() > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {

                throw;
            }
        }


        #region Article Images

        #endregion

        #endregion

        #region Category
        public async Task<List<Category>> GetCategoryListAsync(CategoryType type)
        {
            try
            {
                return type switch
                {
                    CategoryType.All => await this.context.Categories.ToListAsync(),
                    _ => await this.context.Categories.Where(c => c.Type == type).ToListAsync(),
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                return await this.context.Categories.Where(c => c.Id == categoryId).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            category = category ?? throw new ArgumentNullException(nameof(category));

            try
            {
                Category existAllCategoryType = await this.context.Categories.Where(c => c.Type == CategoryType.All).FirstOrDefaultAsync();

                if (existAllCategoryType != null && category.Type == CategoryType.All)
                {
                    throw new Exception("Cannot add more than one category type All.");
                }

                this.context.Categories.Add(category);

                if (await this.context.SaveChangesAsync() > 0)
                {
                    return category;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            category = category ?? throw new ArgumentNullException(nameof(category));

            try
            {
                Category updatedEntity = this.context.Categories.Update(category).Entity;

                if (this.context.Entry(category).State == EntityState.Unchanged || await this.context.SaveChangesAsync() > 0)
                {
                    return updatedEntity;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RemoveCategoryAsync(int categoryId)
        {
            try
            {
                Category category = await this.GetCategoryByIdAsync(categoryId).ConfigureAwait(false);
                if (category != null)
                {
                    this.context.Categories.Remove(category);
                    if (await this.context.SaveChangesAsync() > 0)
                    {
                        return true;
                    }

                    return false;
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Cart
        public async Task<Cart> AddToCart(CartLine cartLine, string userId)
        {
            cartLine = cartLine ?? throw new ArgumentNullException(nameof(cartLine));
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            try
            {
                AppUser user = await this.context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                Cart cart = await this.context.Carts.Where(c => c.UserId.Equals(userId) && !c.IsPaid).FirstOrDefaultAsync();
                if (cart == null)
                {
                    cart = new Cart { UserId = user.Id, Items = new List<CartLine>() };
                }

                cart.Items.Add(cartLine);

                if (await this.context.SaveChangesAsync() > 0)
                {
                    return cart;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Cart> RemoveFromCart(int cartLineId, string userId)
        {
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            try
            {
                AppUser user = await this.context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                Cart cart = await this.context.Carts.Where(c => c.UserId.Equals(userId) && !c.IsPaid).FirstOrDefaultAsync();
                if (cart == null)
                {
                    throw new Exception("Cart not found.");
                }

                CartLine cartLine = cart.Items.Where(l => l.Id == cartLineId).FirstOrDefault();
                if (cartLine != null)
                {
                    cart.Items.Remove(cartLine);
                }

                if (await this.context.SaveChangesAsync() > 0)
                {
                    return cart;
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SetPaidCart(int cartId, string userId)
        {
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            try
            {
                AppUser user = await this.context.Users.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                Cart cart = await this.context.Carts.Where(c => c.UserId.Equals(userId) && c.Id == cartId).FirstOrDefaultAsync();
                if (cart == null)
                {
                    throw new Exception("Cart not found.");
                }

                // TODO
                cart.IsPaid = true;

                return await this.context.SaveChangesAsync() > 0;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region private methods
        private int SkipSize(int page, int elementsAmount)
        {
            double toSkip = (page - 1) * elementsAmount;

            toSkip = toSkip < 0 ? 0 : toSkip;

            return (int)toSkip;
        }

        #region Project
        private string GetProjectFolderPath(ProjectCase projectCase) => $"/Files/ProjectFiles/{new string(projectCase.ProjectName.Take(10).ToArray())}_{DateTime.Now:dd_MM_yyyy__h_mm_ss}_";

        private async Task<ProjectCase> SaveProjectFilesAsync(ProjectCase projectCase, string webRootFilePath)
        {
            // string filePath = $"{this.appEnvironment.WebRootPath}/Files/ProjectFiles/{projectCase.ProjectName}_{DateTime.Now:dd_MM_yyyy__h_mm_ss}_";
            string folderPath = this.GetProjectFolderPath(projectCase);

            // Save image file
            await this.SaveProjectImageAsync(projectCase, webRootFilePath, folderPath).ConfigureAwait(false);

            // Save project description in file
            string descriptionPath = webRootFilePath + folderPath + "Description.txt";
            projectCase.DescriptionPath = await this.SaveTextFileAsync(descriptionPath, projectCase.Description).ConfigureAwait(false);

            return projectCase;
        }

        private async Task<ProjectCase> SaveProjectImageAsync(ProjectCase projectCase, string webRootFilePath, string folderPath)
        {
            if (projectCase.Image?.Length > 0)
            {
                projectCase.ImageMimeType = projectCase.ImageMimeType ?? throw new ArgumentNullException("ImageMimeType cannot be null.");

                string urlPath = folderPath + projectCase.ImageFileName;
                string filePath = webRootFilePath + urlPath;

                projectCase.ImagePath = await this.SaveImageFileAsync(projectCase.Image, filePath).ConfigureAwait(false);
                projectCase.UrlPath = urlPath;
            }

            return projectCase;
        }

        private async Task AttachProjectCaseListFilesAsync(IEnumerable<ProjectCase> projectCases)
        {
            foreach (ProjectCase projectCase in projectCases)
            {
                await this.AttachProjectCaseFilesAsync(projectCase).ConfigureAwait(false);
            }
        }

        private async Task AttachProjectCaseFilesAsync(ProjectCase projectCase)
        {
            //projectCase.Image = await this.GetImageFileAsync(projectCase.ImagePath).ConfigureAwait(false);
            projectCase.Description = await this.GetTextFileAsync(projectCase.DescriptionPath).ConfigureAwait(false);
        }

        #endregion

        #region Article
        private string GetArticleFolderFilePath(Article article) => $"/Files/ArticleFiles/{new string(article.Topic.Take(10).ToArray())}_{DateTime.Now:dd_MM_yyyy__h_mm_ss}_";

        /* AttachArticleFilesAsync
        private async Task AttachArticleListFilesAsync(IEnumerable<Article> articles)
        {
            foreach (Article article in articles)
            {
                await this.AttachArticleFilesAsync(article, true);
            }
        }
        
        private async Task AttachArticleFilesAsync(Article article, bool isOnlyOneImage = false)
        {
            article.ArticleImages = new List<ArticleImage>();
            foreach (ArticleImage im in article.ArticleImages)
            {
                byte[] img = await this.GetImageFileAsync(im.FilePath).ConfigureAwait(false);
                if (img != null)
                {
                    article.ArticleImages.Add(new ArticleImage 
                    {
                        Article = im.Article,
                        Id = im.Id,
                        ImageMimeType = im.ImageMimeType,
                        UrlPath = im.UrlPath
                    });
                }

                if (isOnlyOneImage) break;
            }

            article.Text = await this.GetTextFileAsync(article.TextPath).ConfigureAwait(false);
        }*/

        private async Task<Article> SaveArticleFilesAsync(Article article, string webRootFilePath)
        {
            string folderPath = this.GetArticleFolderFilePath(article);

            // Save images
            foreach (ArticleImage image in article.ArticleImages)
            {
                image.ImageMimeType = image.ImageMimeType ?? throw new ArgumentNullException("ImageMimeType cannot be null.");

                string urlPath = folderPath + image.FileName;
                string filePath = webRootFilePath + urlPath;

                await this.SaveImageFileAsync(image.File, filePath).ConfigureAwait(false);

                image.FilePath = filePath;
                image.UrlPath = urlPath;
                image.Article = article;
            }

            // Save project description in file
            string textPath = webRootFilePath + folderPath + "Text.txt";
            article.TextPath = await this.SaveTextFileAsync(textPath, article.Text).ConfigureAwait(false);

            return article;
        }

        #endregion

        #region file methods

        /// <summary>
        /// SaveImageFileAsync. If image exists, it will be reopened and truncate
        /// </summary>
        /// <returns>image path or null if fail</returns>
        private async Task<string> SaveImageFileAsync(byte[] image, string filePath)
        {
            if (image?.Length > 0)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await stream.WriteAsync(image, 0, image.Length);
                }

                return filePath;
            }

            return null;
        }

        /// <summary>
        /// GetImageFileAsync
        /// </summary>
        /// <returns>image path or null if fail</returns>
        private async Task<byte[]> GetImageFileAsync(string filePath)
        {
            byte[] result = null;
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                result = await System.IO.File.ReadAllBytesAsync(filePath);
            }

            return result;
        }

        /// <summary>
        /// SaveTextFileAsync.
        /// 
        /// If file does not exists, it will be created
        /// </summary>
        /// <returns>image path or null if fail</returns>
        private async Task<string> SaveTextFileAsync(string textPath, string text)
        {
            using (FileStream fs = new FileStream(textPath, FileMode.Create, FileAccess.Write))
            {
                using (var stream = new StreamWriter(fs, Encoding.UTF8))
                {
                    await stream.WriteAsync(text ?? string.Empty);
                }
            }

            return textPath;
        }

        /// <summary>
        /// GetTextFileAsync
        /// </summary>
        /// <returns>image path or null if fail</returns>
        private async Task<string> GetTextFileAsync(string textPath)
        {
            string result = null;
            if (!string.IsNullOrEmpty(textPath) && File.Exists(textPath))
            {
                using (var stream = new StreamReader(textPath, Encoding.UTF8))
                {
                    result = await stream.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            return result;
        }

        private void RemoveFiles(IEnumerable<string> filePathList)
        {
            foreach (string path in filePathList)
            {
                this.RemoveFile(path);
            }
        }

        private void RemoveFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        #endregion

        #endregion private methods
    }
}
