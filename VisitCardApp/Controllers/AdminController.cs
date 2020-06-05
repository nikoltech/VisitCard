namespace VisitCardApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        public AdminController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        #region private methods

        #endregion
    }
}