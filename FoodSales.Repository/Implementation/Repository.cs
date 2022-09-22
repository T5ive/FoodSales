using FoodSales.DataAccess.Data;
using FoodSales.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FoodSales.Repository.Implementation;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T?> DbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        DbSet = _db.Set<T>();
    }

    public IEnumerable<T?> All(Expression<Func<T?, bool>> filter)
    {
        IQueryable<T?> query = DbSet;
        query = query.Where(filter);
        return query.ToList();
    }

    public void Add(T? entity)
    {
        DbSet.Add(entity);
    }

    public void AddList(IEnumerable<T>? entity)
    {
        DbSet.AddRange(entity);
    }

    public IEnumerable<T?> GetAll()
    {
        IQueryable<T?> query = DbSet;
        return query.ToList();
    }

    public T? GetFirstOrDefault(Expression<Func<T?, bool>> filter)
    {
        IQueryable<T?> query = DbSet;
        query = query.Where(filter);
        return query.FirstOrDefault();
    }

    public void Remove(T? entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T?> entity)
    {
        DbSet.RemoveRange(entity);
    }
}