using FoodSales.DataAccess.Data;
using FoodSales.Repository.IRepository;

namespace FoodSales.Repository.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public IOrderRepo Order { get; set; }

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Order = new OrderRepo(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }

    public void SaveAsync()
    {
        _db.SaveChangesAsync();
    }
}