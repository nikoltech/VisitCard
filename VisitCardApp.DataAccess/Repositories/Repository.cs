namespace VisitCardApp.DataAccess.Repositories
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Helpers;

    public class Repository : IRepository
    {
        private readonly DataContext context;

        private readonly UserManager<AppUser> userManager;

        public Repository(DataContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        // TODO: CRUD image
        #region ProjectCase
        public async Task<ProjectCase> CreateProjectCaseAsync(ProjectCase projectCase)
        {
            projectCase = projectCase ?? throw new ArgumentNullException(nameof(projectCase));

            try
            {
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

                return projectCase;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ProjectCase>> GetProjectCaseListAsync(int page, int count)
        {
            try
            {
                int skip = this.SkipSize(page, count);

                List<ProjectCase> projectCases = await this.context.ProjectCases.Skip(skip).Take(count).ToListAsync();

                return projectCases;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProjectCase> UpdateProjectCaseAsync(ProjectCase updatedProject)
        {
            updatedProject = updatedProject ?? throw new ArgumentNullException(nameof(updatedProject));

            try
            {
                ProjectCase projectCase = await this.context.ProjectCases.Where(p => p.Id == updatedProject.Id).FirstOrDefaultAsync();

                if (projectCase == null)
                {
                    throw new Exception($"Project with id {updatedProject.Id} not found.");
                }

                projectCase.ProjectName = updatedProject.ProjectName;

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

        // TODO
        //public async Task<ProjectCase> UpdateProjectCaseImageAsync(int projectId, ....)

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

                if (!string.IsNullOrEmpty(projectCase.ImagePath))
                {
                    File.Delete(projectCase.ImagePath);
                }

                if (!string.IsNullOrEmpty(projectCase.DescriptionPath))
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

        // TODO: CRUD images
        #region Articles
        public async Task<Article> CreateArticleAsync(Article article)
        {
            article = article ?? throw new ArgumentNullException(nameof(article));

            string filePath = $"~/Files/ArticleFiles/{article.Topic.Take(10)}_{DateTime.Now:dd_MM_yyyy__h_mm_ss}_";

            try
            {
                // Save image file
                foreach (FileHelper f in article.ArticleImages)
                {
                    if (f.File.Length > 0)
                    {
                        string imagePath = filePath + f.FileName;
                        using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await stream.WriteAsync(f.File, 0, f.File.Length).ConfigureAwait(false);
                            article.ArticleImagesPath.Add(new ArticleImage { ImagePath = imagePath, Article = article });
                        }

                    }
                }
                

                // Save project description in file
                string textPath = filePath + "Text.txt";
                using (var stream = new StreamWriter(textPath, false, Encoding.UTF8))
                {
                    await stream.WriteAsync(article.Text ?? string.Empty);
                    article.TextPath = textPath;
                }

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
                    .Include(a => a.ArticleImagesPath)
                    .Where(p => p.Id == articleId).FirstOrDefaultAsync();

                if (article == null)
                {
                    throw new Exception($"Project with id {articleId} not found.");
                }

                await this.AttachArticleFilesAsync(article).ConfigureAwait(false);

                return article;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Article>> GetArticleListAsync(int page, int count)
        {
            try
            {
                int skip = this.SkipSize(page, count);

                List<Article> articles = await this.context.Articles
                    .Include(a => a.ArticleImagesPath)
                    .Skip(skip).Take(count).ToListAsync();

                foreach (Article article in articles)
                {
                    await this.AttachArticleFilesAsync(article).ConfigureAwait(false);
                }

                return articles;
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

                if (!string.IsNullOrEmpty(article.TextPath))
                {
                    using (var stream = new StreamWriter(article.TextPath, false, Encoding.UTF8))
                    {
                        await stream.WriteAsync(article.Text).ConfigureAwait(false);
                    }
                }

                article.Topic = article.Topic;

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

        public async Task<bool> RemoveArticleAsync(int articleId)
        {
            if (articleId == 0)
            {
                throw new ArgumentNullException(nameof(articleId));
            }

            try
            {
                Article article = await this.context.Articles
                    .Include(a => a.ArticleImagesPath)
                    .Where(p => p.Id == articleId).FirstOrDefaultAsync();

                if (article == null)
                {
                    return true;
                }

                foreach (ArticleImage im in article.ArticleImagesPath)
                {
                    if (!string.IsNullOrEmpty(im.ImagePath))
                    {
                        File.Delete(im.ImagePath);
                    }
                }

                if (!string.IsNullOrEmpty(article.TextPath))
                {
                    File.Delete(article.TextPath);
                }

                this.context.Articles.Remove(article);

                return await this.context.SaveChangesAsync() > 0;
            }
            catch
            {
                throw;
            }
        }

        #region Article Images

        #endregion

        #endregion


        #region private methods
        private int SkipSize(int page, int elementsAmount)
        {
            double toSkip = (page - 1) * elementsAmount;

            toSkip = toSkip < 0 ? 0 : toSkip;

            return (int)toSkip;
        }

        

        private async Task AttachArticleFilesAsync(Article article)
        {
            article.ArticleImages = new List<FileHelper>();
            foreach (ArticleImage im in article.ArticleImagesPath)
            {
                if (!string.IsNullOrEmpty(im.ImagePath))
                {
                    byte[] img = await File.ReadAllBytesAsync(im.ImagePath);
                    article.ArticleImages.Add(new FileHelper(null, img, im.ImageMimeType));
                }

            }

            if (!string.IsNullOrEmpty(article.TextPath))
            {
                using (var stream = new StreamReader(article.TextPath, Encoding.UTF8))
                {
                    article.Text = await stream.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }


        #endregion
    }
}
