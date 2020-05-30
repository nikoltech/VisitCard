namespace VisitCardApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Models;

    [Authorize(Roles = "admin")]
    [Route("{controller}")]
    public class AdminController : Controller
    {
        private readonly IProjectManagement projectManagement;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly ICategoryManagement categoryManagement;
        private readonly IArticleManagement articleManagement;

        public AdminController(
            IProjectManagement projectManagement,
            IArticleManagement articleManagement,
            IWebHostEnvironment appEnvironment,
            ICategoryManagement categoryManagement)
        {
            this.articleManagement = articleManagement;
            this.projectManagement = projectManagement;
            this.appEnvironment = appEnvironment;
            this.categoryManagement = categoryManagement;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region private methods



        #endregion
    }
}