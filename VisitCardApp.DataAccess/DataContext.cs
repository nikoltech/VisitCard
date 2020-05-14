namespace VisitCardApp.DataAccess
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using VisitCardApp.DataAccess.Entities;

    public class DataContext : IdentityDbContext<User, Role, string>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }


    }
}
