using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using VisitCardApp.BusinessLogic.Communications;
using VisitCardApp.BusinessLogic.Interfaces;
using VisitCardApp.BusinessLogic.Managements;
using VisitCardApp.DataAccess;
using VisitCardApp.DataAccess.Entities;
using VisitCardApp.DataAccess.Repositories;
using VisitCardApp.DataAccess.Services;
using VisitCardApp.DataAccess.Services.User;

namespace VisitCardApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddConfiguration(configuration);

            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(this.Configuration["Data:ConnectionString"], options => options.MigrationsAssembly("VisitCardApp")));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.Configure<EmailConfig>(this.Configuration.GetSection("EmailConfiguration"));
            services.Configure<AdminSecurity>(this.Configuration.GetSection("AdminSecurity"));


            // Resolve dependencies
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IArticleManagement, ArticleManagement>();
            services.AddScoped<IProjectManagement, ProjectManagement>();
            services.AddScoped<ICategoryManagement, CategoryManagement>();
            services.AddScoped<UserService>();
            services.AddScoped<RoleInitService>();

            // Add caching
            services.AddMemoryCache();

            // Add Compression
            // TODO: Check images on responce
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    "text/javascript",
                    "image/svg+xml",
                    "application/manifest+json"
                });
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Include compression before static files
            app.UseResponseCompression();

            app.UseStaticFiles(this.GetStaticFileOptions())
                .UseFileServer(new FileServerOptions()
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(env.ContentRootPath, "node_modules")
                    ),
                    RequestPath = "/node_modules",
                    EnableDirectoryBrowsing = false
                });

            app.UseRouting();

            // Use with default auth and with tokens
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "project",
                    pattern: "{controller}/{id:int}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DataContext>())
                {
                    context.Database.Migrate();
                    int result = context.Initialize().Result;

                    RoleInitService roleInitService = serviceScope.ServiceProvider.GetRequiredService<RoleInitService>();
                    roleInitService.InitializeAsync().GetAwaiter().GetResult();
                }
            }
        }

        private StaticFileOptions GetStaticFileOptions()
        {
            return new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=604800 "); // 7 days
                }
            };
        }
    }
}
