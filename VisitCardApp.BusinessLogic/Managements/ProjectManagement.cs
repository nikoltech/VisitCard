namespace VisitCardApp.BusinessLogic.Managements
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Repositories;

    public class ProjectManagement : IProjectManagement
    {
        private readonly IRepository repo;

        public ProjectManagement(IRepository repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileFolderPath">Need folders {fileFolderPath}/Files/ProjectFiles</param>
        /// <returns></returns>
        public async Task<ProjectCaseModel> CreateProjectCaseAsync(ProjectCaseModel model, string fileFolderPath)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));
            fileFolderPath = fileFolderPath ?? throw new ArgumentNullException(nameof(fileFolderPath));

            try
            {
                ProjectCase entity = await this.repo.CreateProjectCaseAsync(model.ToEntity(), fileFolderPath).ConfigureAwait(false);

                if (entity == null) { return null; }

                ProjectCaseModel addedModel = new ProjectCaseModel();
                addedModel.ToModel(entity);

                return addedModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProjectCaseModel> GetProjectCaseByIdAsync(int projectId)
        {
            try
            {
                ProjectCase entity = await this.repo.GetProjectCaseAsync(projectId).ConfigureAwait(false);

                if (entity == null) { return null; }

                ProjectCaseModel model = new ProjectCaseModel();
                model.ToModel(entity);

                return model;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ProjectCaseModel>> GetProjectCaseListAsync(int page, int count, int categoryId)
        {
            try
            {
                List<ProjectCase> entities = await this.repo.GetProjectCaseListAsync(page, count, categoryId).ConfigureAwait(false);

                List<ProjectCaseModel> models = this.ToModelList<ProjectCase, ProjectCaseModel>(entities);

                return models;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProjectCaseModel> UpdateProjectCaseAsync(ProjectCaseModel model, string webRootFilePath)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));
            webRootFilePath = webRootFilePath ?? throw new ArgumentNullException(nameof(webRootFilePath));

            try
            {
                ProjectCase entity = await this.repo.UpdateProjectCaseAsync(model.ToEntity(), webRootFilePath).ConfigureAwait(false);

                ProjectCaseModel updatedModel = new ProjectCaseModel();
                updatedModel.ToModel(entity);

                return updatedModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RemoveProjectCaseAsync(int projectId)
        {
            try
            {
                return await this.repo.RemoveProjectCaseAsync(projectId).ConfigureAwait(false);
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
