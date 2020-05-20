using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisitCardApp.DataAccess.Entities;

namespace VisitCardApp.BusinessLogic.Interfaces
{
    public interface IProjectManagement
    {
        Task<ProjectCaseModel> CreateProjectCaseAsync(ProjectCaseModel model);

        Task<ProjectCaseModel> GetProjectCaseAsync(int projectId);

        Task<List<ProjectCaseModel>> GetProjectCaseListAsync(int page, int count);

        Task<ProjectCaseModel> UpdateProjectCaseAsync(ProjectCaseModel model);

        Task<bool> RemoveProjectCaseAsync(int projectId);
    }
}
