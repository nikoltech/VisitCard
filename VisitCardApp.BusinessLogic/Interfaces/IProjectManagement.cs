namespace VisitCardApp.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Models;

    public interface IProjectManagement
    {
        Task<ProjectCaseModel> CreateProjectCaseAsync(ProjectCaseModel model, string fileFolderPath);

        Task<ProjectCaseModel> GetProjectCaseByIdAsync(int projectId);

        Task<List<ProjectCaseModel>> GetProjectCaseListAsync(int page, int count);

        Task<ProjectCaseModel> UpdateProjectCaseAsync(ProjectCaseModel model);

        Task<bool> RemoveProjectCaseAsync(int projectId);
    }
}
