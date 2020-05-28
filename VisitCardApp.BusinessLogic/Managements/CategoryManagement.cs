﻿namespace VisitCardApp.BusinessLogic.Managements
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Enums;
    using VisitCardApp.DataAccess.Repositories;

    public class CategoryManagement : ICategoryManagement
    {
        private readonly IRepository repo;

        public CategoryManagement(IRepository repository)
        {
            this.repo = repository;
        }

        public async Task<List<CategoryModel>> GetCategoryListAsync(CategoryType type = CategoryType.All)
        {
            try
            {
                List<Category> entities = await this.repo.GetCategoryListAsync(type).ConfigureAwait(false);
                return this.ToModelList<Category, CategoryModel>(entities);
            }
            catch
            {
                throw;
            }
        }

        public async Task<CategoryModel> CreateCategoryAsync(CategoryModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            try
            {
                Category category = await this.repo.CreateCategoryAsync(model.ToEntity()).ConfigureAwait(false);
                CategoryModel addedModel = new CategoryModel();
                addedModel.ToModel(category);

                return addedModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CategoryModel> UpdateCategoryAsync(CategoryModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            try
            {
                Category category = await this.repo.UpdateCategoryAsync(model.ToEntity()).ConfigureAwait(false);

                CategoryModel updatedModel = new CategoryModel();
                updatedModel.ToModel(category);

                return updatedModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RemoveCategoryAsync(CategoryModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            try
            {
                return await this.repo.RemoveCategoryAsync(model.ToEntity()).ConfigureAwait(false);
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
