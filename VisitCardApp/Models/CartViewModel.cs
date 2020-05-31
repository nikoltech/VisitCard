namespace VisitCardApp.Models
{
    using VisitCardApp.BusinessLogic.Models;
    
    public class CartViewModel
    {
        public CartModel Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}
