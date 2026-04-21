using Microsoft.EntityFrameworkCore;
using OnlineBazar.Application.Interfaces;
using OnlineBazar.Domain.Entities;
using OnlineBazar.Infrastructure.Data;

namespace OnlineBazar.Infrastructure.Repositories;

public class EfRepository<T>(AppDbContext db) : IRepository<T> where T : class
{
    public IQueryable<T> Query() => db.Set<T>().AsQueryable();
    public Task AddAsync(T entity, CancellationToken ct = default) => db.Set<T>().AddAsync(entity, ct).AsTask();
}

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
    public IRepository<User> Users => new EfRepository<User>(db);
    public IRepository<Listing> Listings => new EfRepository<Listing>(db);
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
