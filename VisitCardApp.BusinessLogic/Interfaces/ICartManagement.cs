namespace VisitCardApp.BusinessLogic.Interfaces
{
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Models;

    public interface ICartManagement
    {
        Task<CartModel> AddToCart(CartLineModel model, string userId);

        Task<CartModel> RemoveFromCart(int cartLineId, string userId);

        Task<bool> SetPaidCart(int cartId, string userId);
    }
}
