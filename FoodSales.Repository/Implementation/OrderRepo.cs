using FoodSales.DataAccess.Data;
using FoodSales.Model.Models;
using FoodSales.Repository.IRepository;

namespace FoodSales.Repository.Implementation;

public class OrderRepo : Repository<Order>, IOrderRepo
{
    private readonly ApplicationDbContext _db;

    public OrderRepo(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Order obj)
    {
        _db.Order.Update(obj);
    }
}