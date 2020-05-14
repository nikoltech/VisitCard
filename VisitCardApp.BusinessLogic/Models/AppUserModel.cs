namespace VisitCardApp.DataAccess.Entities
{
    using Microsoft.AspNetCore.Identity;
    using VisitCardApp.BusinessLogic.Models;

    public class AppUserModel : IEntityModel<AppUser>
    {
        public AppUser ToEntity()
        {
            throw new System.NotImplementedException();
        }

        public void ToModel(AppUser entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
