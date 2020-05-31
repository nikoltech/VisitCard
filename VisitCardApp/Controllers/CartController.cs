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
        private readonly CartModel cartModel;

        public CartController(IProjectManagement projectManagement, CartModel cartModel)
        {
            this.projectManagement = projectManagement;
            this.cartModel = cartModel;
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
                    CartLineModel existsItem = this.cartModel.Items.Where(i => i.ProjectCase.Id == projectId).FirstOrDefault();
                    if (existsItem != null)
                    {
                        existsItem.Quantity += quantity.Value;
                    }
                    else
                    {
                        this.cartModel.Items.Add(new CartLineModel { ProjectCase = model, Quantity = quantity.Value });
                    }
                }

                TempData["ItemAdded"] = model != null;

                return View("Cart", this.cartModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpPost("Remove")]
        public IActionResult RemoveFromCart(int projectId)
        {
            CartLineModel model = this.cartModel.Items.Where(i => i.ProjectCase.Id == projectId).FirstOrDefault();

            if (model != null)
            {
                this.cartModel.Items.Remove(model);
                TempData["ItemRemoved"] = true;
            }

            return View("Cart", this.cartModel);
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