namespace VisitCardApp.DataAccess.Entities
{
    using Microsoft.AspNetCore.Identity;
    using VisitCardApp.BusinessLogic.Models;

    public class UserModel : IEntityModel<User>
    {
        public User ToEntity()
        {
            throw new System.NotImplementedException();
        }

        public void ToModel(User entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
