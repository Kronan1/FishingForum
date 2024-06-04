using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Areas.Identity.Data;
using FishingForum.DAL;
using Microsoft.Extensions.Options;
namespace FishingForum
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("FishingForumContextConnection") ?? throw new InvalidOperationException("Connection string 'FishingForumContextConnection' not found.");

            builder.Services.AddDbContext<FishingForumContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<FishingForumUser>(options =>
            options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddSignInManager<SignInManager<FishingForumUser>>()
                .AddEntityFrameworkStores<FishingForumContext>();

            builder.Services.AddHttpClient();

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("NeedsAdmin", policy => policy.RequireRole("Admin"));

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/AdminPages", "NeedsAdmin");
            });


            builder.Services.AddScoped<AdminManager>();
            builder.Services.AddScoped<UserManager>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
