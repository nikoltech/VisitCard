namespace VisitCardApp.DataAccess
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using VisitCardApp.DataAccess.Entities;

    public partial class DataContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<InitializeCode> InitializeCodes { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleImage> ArticleImages { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProjectCase> ProjectCases { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartLine> CartLines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<InitializeCode>().HasNoKey();
        }
    }
}
