namespace VisitCardApp.BusinessLogic.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Models;

    public interface IProjectManagement
    {
        Task<ProjectCaseModel> CreateProjectCaseAsync(ProjectCaseModel model, string fileFolderPath);

        Task<ProjectCaseModel> GetProjectCaseByIdAsync(int projectId);

        Task<(List<ProjectCaseModel> projectModels, long total)> GetProjectCaseListAsync(int page, int count, int categoryId);

        Task<ProjectCaseModel> UpdateProjectCaseAsync(ProjectCaseModel model, string webRootFilePath);

        Task<bool> RemoveProjectCaseAsync(int projectId);
    }
}
