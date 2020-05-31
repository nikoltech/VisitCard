namespace VisitCardApp.BusinessLogic.Models
{
    using System.Collections.Generic;

    public class CartModel
    {
        public CartModel()
        {
            Items = new List<CartLineModel>();
        }

        public List<CartLineModel> Items { get; set; }
    }
}
