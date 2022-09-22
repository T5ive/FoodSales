namespace FoodSales.Repository.IRepository;

public interface IUnitOfWork
{
    IOrderRepo Order { get; set; }

    void Save();

    void SaveAsync();
}