namespace VisitCardApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;

    public class ProjectController : Controller
    {
        private readonly IProjectManagement projectManagement;

        public ProjectController(IProjectManagement projectManagement)
        {
            this.projectManagement = projectManagement;
        }

        // TODO
        [HttpGet("GetProjects")]
        public async Task<IActionResult> GetProjectsAsync(int page = 1, int count = 5)
        {

            return View();
        }

        // TODO
        [HttpGet("GetProjectById")]
        public async Task<IActionResult> GetProjectByIdAsync(int projectId)
        {

            return View();
        }

        // TODO
        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProjectAsync(ProjectCaseModel model)
        {
            
            return View();
        }

        // TODO
        [HttpPatch("UpdateProject")]
        public async Task<IActionResult> UpdateProjectAsync(ProjectCaseModel model)
        {

            return View();
        }

        // TODO
        [HttpDelete("RemoveProject")]
        public async Task<IActionResult> RemoveProjectAsync(int projectId)
        {

            return View();
        }
    }
}