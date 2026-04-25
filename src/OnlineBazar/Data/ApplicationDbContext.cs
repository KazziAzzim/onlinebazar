using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineBazar.Models;

namespace OnlineBazar.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<SiteContent> SiteContents => Set<SiteContent>();
    // Use a conventional auto-property for EF tooling compatibility
    public DbSet<Testimonial> Testimonials { get; set; } = null!;
    public DbSet<Banner> Banners { get; set; } = null!;
    public DbSet<Deal> Deals { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Category>(entity =>
        {
            entity.HasIndex(c => c.Slug).IsUnique();
            entity.Property(c => c.Name).HasMaxLength(120).IsRequired();
        });

        builder.Entity<Product>(entity =>
        {
            entity.HasIndex(p => p.Slug).IsUnique();
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            entity.Property(p => p.ViewCount).HasDefaultValue(0);
            entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<Order>(entity =>
        {
            entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(o => o.OrderDate).HasDefaultValueSql("GETUTCDATE()");
            entity.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<OrderItem>(entity =>
        {
            entity.Property(i => i.Price).HasColumnType("decimal(18,2)");
            entity.HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId);
            entity.HasOne(i => i.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Testimonial>(entity =>
        {
            entity.Property(t => t.CustomerName).HasMaxLength(120).IsRequired();
            entity.Property(t => t.City).HasMaxLength(120).IsRequired();
            entity.Property(t => t.ReviewText).HasMaxLength(1200).IsRequired();
            entity.Property(t => t.Rating).IsRequired();
        });

        builder.Entity<Banner>(entity =>
        {
            entity.Property(b => b.Title).HasMaxLength(250);
            entity.Property(b => b.Subtitle).HasMaxLength(500);
            entity.Property(b => b.ImageUrl).HasMaxLength(500).IsRequired();
            entity.Property(b => b.RedirectUrl).HasMaxLength(500);
        });

        builder.Entity<Deal>(entity =>
        {
            entity.Property(d => d.Title).HasMaxLength(180).IsRequired();
            entity.Property(d => d.Description).HasMaxLength(500);
            entity.Property(d => d.ImageUrl).HasMaxLength(500);
            entity.Property(d => d.DiscountType).HasMaxLength(20).IsRequired();
            entity.Property(d => d.DiscountValue).HasColumnType("decimal(18,2)");
            entity.Property(d => d.RedirectUrl).HasMaxLength(500);
        });
    }
}
