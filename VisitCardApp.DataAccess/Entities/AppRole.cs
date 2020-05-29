namespace VisitCardApp.DataAccess.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class AppRole : IdentityRole
    {
        public AppRole() { }

        public AppRole(string roleName)
            :base(roleName)
        { }
    }
}
