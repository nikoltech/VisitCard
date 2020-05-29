namespace VisitCardApp.DataAccess.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;
    using VisitCardApp.DataAccess.Entities;

    public class RoleInitService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly AdminSecurity adminSecurity;

        public RoleInitService(
            UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager, 
            IOptions<AdminSecurity> adminOptions)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.adminSecurity = adminOptions.Value;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (await roleManager.FindByNameAsync("admin") == null)
                {
                    await roleManager.CreateAsync(new AppRole("admin"));
                }
                if (await roleManager.FindByNameAsync("user") == null)
                {
                    await roleManager.CreateAsync(new AppRole("user"));
                }
                if (await userManager.FindByNameAsync(this.GetUsernameFromEmail(this.adminSecurity.AdminEmail)) == null)
                {
                    AppUser admin = new AppUser
                    {
                        Email = this.adminSecurity.AdminEmail,
                        UserName = this.GetUsernameFromEmail(this.adminSecurity.AdminEmail)
                    };
                    IdentityResult result = await userManager.CreateAsync(admin, this.adminSecurity.AdminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "admin");
                    }
                }
            }
            catch
            {

                throw;
            }
        }

        private string GetUsernameFromEmail(string email)
        {
            return email?.Split('@')[0];
        }
    }
}
