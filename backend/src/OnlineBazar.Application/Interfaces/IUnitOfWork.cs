using OnlineBazar.Domain.Entities;

namespace OnlineBazar.Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Listing> Listings { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IRepository<T> where T : class
{
    IQueryable<T> Query();
    Task AddAsync(T entity, CancellationToken ct = default);
}
