namespace VisitCardApp.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Helpers;
    using VisitCardApp.Models;

    [Authorize]
    public class CartController : Controller
    {
        private readonly IProjectManagement projectManagement;

        public CartController(IProjectManagement projectManagement)
        {
            this.projectManagement = projectManagement;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Add/{id}/{quantity?}")]
        public async Task<IActionResult> AddToCart(int projectId, int? quantity = 1)
        {
            try
            {
                ProjectCaseModel model = await this.projectManagement.GetProjectCaseByIdAsync(projectId);

                if (model != null)
                {
                    this.GetCart().Items.Add(new CartLineModel { ProjectCase = model, Quantity = quantity.Value });
                }

                TempData["CartAdded"] = true;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpPost("Remove")]
        public IActionResult RemoveFromCart(int projectId)
        {
            CartLineModel model = this.GetCart().Items.Where(i => i.ProjectCase.Id == projectId).FirstOrDefault();

            if (model != null)
            {
                GetCart().Items.Remove(model);
                TempData["CartRemoved"] = true;
            }

            return View("Cart", this.GetCart());
        }

        #region private methods
        private CartModel GetCart()
        {
            CartModel cart = this.HttpContext.Session.Get<CartModel>("Cart");
            if (cart == null)
            {
                this.HttpContext.Session.Set("Cart", new CartModel());
            }
            return cart;
        }
        #endregion
    }
}