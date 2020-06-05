namespace VisitCardApp.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Square;
    using Square.Apis;
    using Square.Exceptions;
    using Square.Models;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.Helpers;
    using VisitCardApp.Models;
    using VisitCardApp.Models.Payment;

    [Produces("application/json")]
    [Authorize]
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly IProjectManagement projectManagement;
        private readonly AppSettings appSettings;

        public CartController(IProjectManagement projectManagement, IOptions<AppSettings> appSettings)
        {
            this.projectManagement = projectManagement;
            this.appSettings = appSettings?.Value;
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

        // TEST
        // TODO
        [HttpGet("PaymentForm")]
        public IActionResult PaymentForm()
        {
            try
            {
                string paymentFormUrl = this.appSettings.Environment == "sandbox" ?
                "https://js.squareupsandbox.com/v2/paymentform" : "https://js.squareup.com/v2/paymentform";

                PaymentInfoModel model = new PaymentInfoModel
                {
                    ApplicationId = this.appSettings.ApplicationId,
                    LocationId = this.appSettings.LocationId,
                    PaymentFormUrl = paymentFormUrl,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                return View("PaymentError", new PaymentResultModel { ResultMessage = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPaymentAsync([FromBody]string nonce)
        {
            try
            {
                Square.Environment environment = this.appSettings.Environment == "sandbox" ?
                Square.Environment.Sandbox : Square.Environment.Production;

                // Build base client
                SquareClient client = new SquareClient.Builder()
                    .Environment(environment)
                    .AccessToken(this.appSettings.AccessToken)
                    .Build();

                string testNonce = string.Empty;

                //string nonce = Request.Form["nonce"];

                IPaymentsApi PaymentsApi = client.PaymentsApi;
                nonce = testNonce;
                CreatePaymentRequest request_body = new CreatePaymentRequest(nonce, this.NewIdempotencyKey(), new Money(100, "USD"));

                CreatePaymentResponse responce = await PaymentsApi.CreatePaymentAsync(request_body);

                return View("PaymentResult", responce);
            }
            catch (Exception ex)
            {
                return View("PaymentError", new PaymentResultModel { ResultMessage = ex.Message });
            }
        }

        [HttpPost("Paid")]
        public IActionResult Paid()
        {
            Square.Environment environment = this.appSettings.Environment == "sandbox" ?
                Square.Environment.Sandbox : Square.Environment.Production;

            // Build base client
            SquareClient client = new SquareClient.Builder()
            .Environment(environment)
            .AccessToken(this.appSettings.AccessToken)
            .Build();


            string nonce = Request.Form["nonce"];
            IPaymentsApi PaymentsApi = client.PaymentsApi;
            // Every payment you process with the SDK must have a unique idempotency key.
            // If you're unsure whether a particular payment succeeded, you can reattempt
            // it with the same idempotency key without worrying about double charging
            // the buyer.
            string uuid = this.NewIdempotencyKey();

            // Monetary amounts are specified in the smallest unit of the applicable currency.
            // This amount is in cents. It's also hard-coded for $1.00,
            // which isn't very useful.
            Money amount = new Money.Builder()
                .Amount(500L)
                .Currency("USD")
                .Build();

            // To learn more about splitting payments with additional recipients,
            // see the Payments API documentation on our [developer site]
            // (https://developer.squareup.com/docs/payments-api/overview).
            CreatePaymentRequest createPaymentRequest = new CreatePaymentRequest.Builder(nonce, uuid, amount)
                .Note("From Square Visit Cart App")
                .Build();

            try
            {
                CreatePaymentResponse response = PaymentsApi.CreatePayment(createPaymentRequest);
                PaymentResultModel model = new PaymentResultModel
                {
                    ResultMessage = "Payment complete! " + response.Payment.Note
                };

                return View(model);
            }
            catch (ApiException ex)
            {
                return View("PaymentError", new ErrorViewModel { Message = ex.Message });
            }
        }

        #region private methods
        private string NewIdempotencyKey()
        {
            return Guid.NewGuid().ToString();
        }

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