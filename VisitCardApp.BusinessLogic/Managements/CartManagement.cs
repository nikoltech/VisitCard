namespace VisitCardApp.BusinessLogic.Managements
{
    using System;
    using System.Threading.Tasks;
    using VisitCardApp.BusinessLogic.Interfaces;
    using VisitCardApp.BusinessLogic.Models;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Repositories;

    public class CartManagement : ICartManagement
    {
        private readonly IRepository repo;

        public CartManagement(IRepository repo)
        {
            this.repo = repo;
        }

        // REASON: Moved to cache. Next task to save order`s hystory.
        public async Task<CartModel> AddToCart(CartLineModel model, string userId)
        {
            model = model ?? throw new ArgumentNullException(nameof(model));
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            try
            {
                //Cart entity = await this.repo.AddToCart(model.ToEntity(), userId).ConfigureAwait(false);

                //CartModel addedModel = new CartModel();
                //addedModel.ToModel(entity);

                //return addedModel;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CartModel> RemoveFromCart(int cartLineId, string userId)
        {
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            try
            {
                //Cart entity = await this.repo.RemoveFromCart(cartLineId, userId).ConfigureAwait(false);

                //CartModel updatedModel = new CartModel();
                //updatedModel.ToModel(entity);

                //return updatedModel;

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SetPaidCart(int cartId, string userId)
        {
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            try
            {
                return await this.repo.SetPaidCart(cartId, userId).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }
    }
}
