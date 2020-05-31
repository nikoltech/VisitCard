namespace VisitCardApp.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.Helpers;
    using VisitCardApp.Models;

    [Authorize]
    [Route("{controller}")]
    public class CartController : Controller
    {
        private readonly IProjectManagement projectManagement;

        public CartController(IProjectManagement projectManagement)
        {
            this.projectManagement = projectManagement;
        }

        public IActionResult Index()
        {
            return View("Cart", this.GetCart());
        }

        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            try
            {
                CartModel cart = this.GetCart();

                ProjectCaseModel model = await this.projectManagement.GetProjectCaseByIdAsync(id);

                if (model != null)
                {
                    CartLineModel existsItem = cart.Items.Where(i => i.ProjectCase.Id == id).FirstOrDefault();
                    if (existsItem != null)
                    {
                        existsItem.Quantity += quantity;
                    }
                    else
                    {
                        cart.Items.Add(new CartLineModel { ProjectCase = model, Quantity = quantity });
                    }

                    cart = this.UpdateCart(cart);
                }

                TempData["ItemAdded"] = model != null;

                return View("Cart", cart);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        [HttpPost("Remove")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int id)
        {
            try
            {
                CartModel cart = this.GetCart();

                CartLineModel model = cart.Items.Where(i => i.ProjectCase.Id == id).FirstOrDefault();

                if (model != null)
                {
                    if (model.Quantity > 1)
                    {
                        model.Quantity--;
                    }
                    else
                    {
                        cart.Items.Remove(model);
                    }
                }

                TempData["ItemRemoved"] = model != null;

                cart = this.UpdateCart(cart);

                return View("Cart", cart);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { Message = ex.Message });
            }
        }

        #region private methods
        private CartModel GetCart()
        {
            CartModel cart = this.HttpContext.Session.Get<CartModel>("Cart");
            if (cart == null)
            {
                cart = new CartModel();
                this.HttpContext.Session.Set("Cart", cart);
            }
            return cart;
        }

        private CartModel UpdateCart(CartModel cart)
        {
            this.HttpContext.Session.Set("Cart", cart);
            return this.HttpContext.Session.Get<CartModel>("Cart");
        }
        #endregion
    }
}