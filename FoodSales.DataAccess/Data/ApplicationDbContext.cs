using FoodSales.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSales.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Order { get; set; }
}