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
        var now = DateTime.UtcNow;

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
            var fashion = await db.Categories.FirstAsync(c => c.Slug == "fashion");

            db.Products.AddRange(
                new Product
                {
                    Name = "Noise Cancelling Headphones",
                    Slug = "noise-cancelling-headphones",
                    Description = "Premium wireless headphones with immersive sound.",
                    Price = 199.99m,
                    StockQuantity = 100,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "4K Ultra HD Smart TV",
                    Slug = "4k-ultra-hd-smart-tv",
                    Description = "55-inch smart TV with vibrant colors and streaming apps.",
                    Price = 549.00m,
                    StockQuantity = 35,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1593784991095-a205069470b6?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Gaming Laptop Pro 15",
                    Slug = "gaming-laptop-pro-15",
                    Description = "High-performance laptop designed for gaming and creators.",
                    Price = 1299.00m,
                    StockQuantity = 20,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1517336714739-489689fd1ca8?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Wireless Mechanical Keyboard",
                    Slug = "wireless-mechanical-keyboard",
                    Description = "Compact keyboard with tactile switches and RGB backlight.",
                    Price = 119.50m,
                    StockQuantity = 75,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1511467687858-23d96c32e4ae?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "Ergonomic Wireless Mouse",
                    Slug = "ergonomic-wireless-mouse",
                    Description = "Silent-click mouse with adjustable DPI and long battery life.",
                    Price = 39.99m,
                    StockQuantity = 150,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1527814050087-3793815479db?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "Smart Fitness Watch",
                    Slug = "smart-fitness-watch",
                    Description = "Track heart rate, workouts, and notifications on the go.",
                    Price = 179.00m,
                    StockQuantity = 80,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Portable Bluetooth Speaker",
                    Slug = "portable-bluetooth-speaker",
                    Description = "Water-resistant speaker with deep bass and all-day battery.",
                    Price = 69.00m,
                    StockQuantity = 120,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1589003077984-894e133dabab?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "Mirrorless Camera Kit",
                    Slug = "mirrorless-camera-kit",
                    Description = "24MP mirrorless camera with interchangeable lens system.",
                    Price = 899.00m,
                    StockQuantity = 18,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1516035069371-29a1b244cc32?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Wireless Earbuds",
                    Slug = "wireless-earbuds",
                    Description = "True wireless earbuds with active noise cancellation.",
                    Price = 89.99m,
                    StockQuantity = 140,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1606220838315-056192d5e927?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "USB-C Fast Charger",
                    Slug = "usb-c-fast-charger",
                    Description = "65W compact charger for phones, tablets, and laptops.",
                    Price = 29.99m,
                    StockQuantity = 200,
                    CategoryId = electronics.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1615526675159-e248f522d6c0?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "Classic Denim Jacket",
                    Slug = "classic-denim-jacket",
                    Description = "Timeless denim jacket with a modern slim fit.",
                    Price = 74.99m,
                    StockQuantity = 90,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1544022613-e87ca75a784a?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Men's Casual Sneakers",
                    Slug = "mens-casual-sneakers",
                    Description = "Comfortable everyday sneakers with breathable mesh.",
                    Price = 64.00m,
                    StockQuantity = 110,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Women's Running Shoes",
                    Slug = "womens-running-shoes",
                    Description = "Lightweight running shoes built for speed and comfort.",
                    Price = 89.00m,
                    StockQuantity = 95,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1460353581641-37baddab0fa2?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Cotton Crew Neck T-Shirt",
                    Slug = "cotton-crew-neck-tshirt",
                    Description = "Soft premium cotton t-shirt for daily wear.",
                    Price = 19.99m,
                    StockQuantity = 220,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "Slim Fit Chino Pants",
                    Slug = "slim-fit-chino-pants",
                    Description = "Versatile chinos suitable for office or weekend style.",
                    Price = 49.99m,
                    StockQuantity = 140,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "Leather Crossbody Bag",
                    Slug = "leather-crossbody-bag",
                    Description = "Elegant and practical leather bag with adjustable strap.",
                    Price = 109.00m,
                    StockQuantity = 60,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "Polarized Sunglasses",
                    Slug = "polarized-sunglasses",
                    Description = "UV-protective sunglasses with lightweight frame.",
                    Price = 34.99m,
                    StockQuantity = 180,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1511499767150-a48a237f0083?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                },
                new Product
                {
                    Name = "Wool Blend Winter Coat",
                    Slug = "wool-blend-winter-coat",
                    Description = "Warm tailored coat for a polished winter look.",
                    Price = 149.00m,
                    StockQuantity = 45,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1539533018447-63fcce2678e3?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Women's Floral Dress",
                    Slug = "womens-floral-dress",
                    Description = "Flowy floral dress ideal for spring and summer outings.",
                    Price = 59.00m,
                    StockQuantity = 85,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = true
                },
                new Product
                {
                    Name = "Analog Leather Strap Watch",
                    Slug = "analog-leather-strap-watch",
                    Description = "Minimalist analog watch with genuine leather strap.",
                    Price = 129.99m,
                    StockQuantity = 70,
                    CategoryId = fashion.Id,
                    ImageUrl = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?auto=format&fit=crop&w=1200&q=80",
                    IsFeatured = false
                }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Testimonials.Any())
        {
            db.Testimonials.AddRange(
                new Testimonial
                {
                    CustomerName = "Sophia Carter",
                    City = "Austin, TX",
                    Rating = 5,
                    ReviewText = "The product quality is excellent and delivery was faster than expected. My order arrived in perfect condition and the entire checkout flow was very smooth.",
                    DisplayOrder = 1
                },
                new Testimonial
                {
                    CustomerName = "Daniel Nguyen",
                    City = "Seattle, WA",
                    Rating = 4,
                    ReviewText = "Great customer support and reliable packaging. I had one quick question before placing the order and the response was prompt and helpful.",
                    DisplayOrder = 2
                },
                new Testimonial
                {
                    CustomerName = "Ava Johnson",
                    City = "Chicago, IL",
                    Rating = 5,
                    ReviewText = "I have placed multiple orders and the consistency is impressive. The catalog is easy to browse and every item has matched the listing details.",
                    DisplayOrder = 3
                },
                new Testimonial
                {
                    CustomerName = "Liam Thompson",
                    City = "Denver, CO",
                    Rating = 5,
                    ReviewText = "Very happy with the experience. Pricing was fair, shipping updates were clear, and the product looked exactly like the photos on the site.",
                    DisplayOrder = 4
                },
                new Testimonial
                {
                    CustomerName = "Mia Rodriguez",
                    City = "Miami, FL",
                    Rating = 4,
                    ReviewText = "Clean website, easy order tracking, and good value overall. The store has become one of my go-to options for gifts and essentials.",
                    DisplayOrder = 5
                },
                new Testimonial
                {
                    CustomerName = "Noah Williams",
                    City = "Phoenix, AZ",
                    Rating = 5,
                    ReviewText = "Highly recommended. The buying process was straightforward and the final product quality exceeded my expectations.",
                    DisplayOrder = 6
                }
            );
            await db.SaveChangesAsync();
        }

        if (!db.Deals.Any())
        {
            db.Deals.AddRange(
                new Deal
                {
                    Title = "Flat ₹200 OFF",
                    Description = "Save instantly on orders above ₹1999. Valid for selected categories.",
                    ImageUrl = "https://images.unsplash.com/photo-1607082350899-7e105aa886ae?auto=format&fit=crop&w=1200&q=80",
                    DiscountType = "Flat",
                    DiscountValue = 200m,
                    StartDate = now.AddHours(-2),
                    EndDate = now.AddHours(10),
                    RedirectUrl = "/products",
                    DisplayOrder = 1,
                    IsActive = true,
                    CreatedAt = now
                },
                new Deal
                {
                    Title = "Weekend 25% OFF",
                    Description = "Get 25% discount on fashion picks for a limited time.",
                    ImageUrl = "https://images.unsplash.com/photo-1483985988355-763728e1935b?auto=format&fit=crop&w=1200&q=80",
                    DiscountType = "Percentage",
                    DiscountValue = 25m,
                    StartDate = now.AddHours(-1),
                    EndDate = now.AddDays(1),
                    RedirectUrl = "/products?category=fashion",
                    DisplayOrder = 2,
                    IsActive = true,
                    CreatedAt = now
                },
                new Deal
                {
                    Title = "Electronics Mega Deal",
                    Description = "Flat ₹500 OFF on select electronics. Limited stock available.",
                    ImageUrl = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&w=1200&q=80",
                    DiscountType = "Flat",
                    DiscountValue = 500m,
                    StartDate = now.AddHours(-3),
                    EndDate = now.AddHours(6),
                    RedirectUrl = "/products?category=electronics",
                    DisplayOrder = 3,
                    IsActive = true,
                    CreatedAt = now
                },
                new Deal
                {
                    Title = "Archived Offer",
                    Description = "Sample inactive deal for admin preview/testing.",
                    ImageUrl = "https://images.unsplash.com/photo-1555529669-e69e7aa0ba9a?auto=format&fit=crop&w=1200&q=80",
                    DiscountType = "Percentage",
                    DiscountValue = 15m,
                    StartDate = now.AddDays(-7),
                    EndDate = now.AddDays(-1),
                    RedirectUrl = "/products",
                    DisplayOrder = 4,
                    IsActive = false,
                    CreatedAt = now
                }
            );
            await db.SaveChangesAsync();
        }
    }
}
