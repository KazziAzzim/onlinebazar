using OnlineBazar.Data;
using OnlineBazar.Interfaces;
using OnlineBazar.Models;

namespace OnlineBazar.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IGenericRepository<Product> Products { get; }
    public IGenericRepository<Category> Categories { get; }
    public IGenericRepository<Order> Orders { get; }
    public IGenericRepository<SiteContent> SiteContents { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Products = new GenericRepository<Product>(context);
        Categories = new GenericRepository<Category>(context);
        Orders = new GenericRepository<Order>(context);
        SiteContents = new GenericRepository<SiteContent>(context);
    }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
