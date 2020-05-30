namespace VisitCardApp.BusinessLogic.Managements
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Repositories;

    public class ArticleManagement : IArticleManagement
    {
        private readonly IRepository repo;

        public ArticleManagement(IRepository repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileFolderPath">Need folders {fileFolderPath}/Files/ArticleFiles</param>
        /// <returns></returns>
        public async Task<ArticleModel> CreateArticleAsync(ArticleModel model, string fileFolderPath)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));
            fileFolderPath = fileFolderPath ?? throw new ArgumentNullException(nameof(fileFolderPath));

            try
            {
                Article entity = await this.repo.CreateArticleAsync(model.ToEntity(), fileFolderPath).ConfigureAwait(false);

                ArticleModel addedModel = new ArticleModel();
                addedModel.ToModel(entity);

                return addedModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ArticleModel> GetArticleByIdAsync(int articleId)
        {
            try
            {
                Article entity = await this.repo.GetArticleByIdAsync(articleId).ConfigureAwait(false);

                ArticleModel model = new ArticleModel();
                model.ToModel(entity);

                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ArticleModel>> GetArticleListAsync(int page, int count, int categoryId)
        {
            try
            {
                List<Article> entities = await this.repo.GetArticleListAsync(page, count, categoryId).ConfigureAwait(false);

                List<ArticleModel> models = this.ToModelList<Article, ArticleModel>(entities);

                return models;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ArticleModel> UpdateArticleAsync(ArticleModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            try
            {
                Article entity = await this.repo.UpdateArticleAsync(model.ToEntity()).ConfigureAwait(false);

                ArticleModel updatedModel = new ArticleModel();
                updatedModel.ToModel(entity);

                return updatedModel;
            }
            catch
            {
                throw;
            }
        }

        // TODO
        // public async Task<ArticleModel> UpdateArticleImageAsync(imageId..., webRootFilePath...) ...

        public async Task<bool> RemoveArticleAsync(int articleId)
        {
            try
            {
                return await this.repo.RemoveArticleAsync(articleId).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        public async Task<CommentModel> AddCommentAsync(CommentModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            try
            {
                Comment entity = await this.repo.AddArticleCommentAsync(model.ToEntity()).ConfigureAwait(false);

                entity = entity ?? throw new Exception("Error saving comment.");

                CommentModel updatedModel = new CommentModel();
                updatedModel.ToModel(entity);

                return updatedModel;
            }
            catch
            {

                throw;
            }
        }

        public async Task<bool> RemoveCommentAsync(int commentId)
        {
            try
            {
                return await this.repo.RemoveArticleCommentAsync(commentId).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        #region private methods
        private List<TModel> ToModelList<TEntity, TModel>(IEnumerable<TEntity> entities)
            where TModel : IEntityModel<TEntity>, new()
            where TEntity : class
        {
            List<TModel> models = new List<TModel>();

            foreach (TEntity entity in entities)
            {
                TModel model = new TModel();
                model.ToModel(entity);
                models.Add(model);
            }

            return models;
        }
        #endregion
    }
}
