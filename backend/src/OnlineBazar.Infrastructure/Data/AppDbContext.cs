using Microsoft.EntityFrameworkCore;
using OnlineBazar.Domain.Entities;

namespace OnlineBazar.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity(j => j.ToTable("UserRoles"));

        modelBuilder.Entity<Listing>().Property(x => x.Price).HasPrecision(18, 2);
    }
}
