using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineBazar.Data;
using OnlineBazar.Models;

namespace OnlineBazar.Extensions;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await db.Database.MigrateAsync();

        string[] roles = ["Admin", "Manager", "Customer"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@onlinebazar.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                Email = adminEmail,
                UserName = adminEmail,
                FullName = "Platform Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin1234!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new Category { Name = "Electronics", Slug = "electronics", Description = "Latest gadgets" },
                new Category { Name = "Fashion", Slug = "fashion", Description = "Trending styles" }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Products.Any())
        {
            var electronics = await db.Categories.FirstAsync(c => c.Slug == "electronics");
            db.Products.Add(new Product
            {
                Name = "Noise Cancelling Headphones",
                Slug = "noise-cancelling-headphones",
                Description = "Premium wireless headphones.",
                Price = 199.99m,
                StockQuantity = 100,
                CategoryId = electronics.Id,
                IsFeatured = true
            });
            await db.SaveChangesAsync();
        }
    }
}
