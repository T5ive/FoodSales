using FoodSales.Model.Models;

namespace FoodSales.Repository.IRepository;

public interface IOrderRepo : IRepository<Order>
{
    void Update(Order obj);
}