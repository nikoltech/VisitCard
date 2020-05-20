namespace VisitCardApp.BusinessLogic.Managements
{
    using System;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Repositories;

    public class ProjectManagement : IProjectManagement
    {
        private readonly IRepository repo;

        public ProjectManagement(IRepository repo)
        {
            this.repo = repo;
        }

        //Task<ProjectCase> CreateProjectCaseAsync(ProjectCase projectCase);
        public async Task<ProjectCaseModel> CreateProjectCaseAsync(ProjectCaseModel model)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));

            try
            {
                ProjectCase projectCase = await this.repo.CreateProjectCaseAsync(model.ToEntity()).ConfigureAwait(false);

                return model;
            }
            catch
            {
                throw;
            }
        }

        //Task<ProjectCase> GetProjectCaseAsync(int projectId);
        public async Task<ProjectCaseModel> GetProjectCaseAsync(int projectId)
        {
            try
            {
                ProjectCase entity = await this.repo.GetProjectCaseAsync(projectId).ConfigureAwait(false);

                ProjectCaseModel model = new ProjectCaseModel();
                model.ToModel(entity);

                return model;
            }
            catch
            {
                throw;
            }
        }

        //Task<List<ProjectCase>> GetProjectCaseListAsync(int page, int count);

        //Task<ProjectCase> UpdateProjectCaseAsync(ProjectCase updatedProject);

        //Task<bool> RemoveProjectCaseAsync(int projectId);
    }
}
