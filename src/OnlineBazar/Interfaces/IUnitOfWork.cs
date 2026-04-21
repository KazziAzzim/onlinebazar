using OnlineBazar.Models;

namespace OnlineBazar.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<Product> Products { get; }
    IGenericRepository<Category> Categories { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<SiteContent> SiteContents { get; }
    Task<int> SaveChangesAsync();
}
