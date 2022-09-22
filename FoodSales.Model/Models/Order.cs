using FoodSales.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FoodSales.Model.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Today;

    [ValidateNever]
    public string DisplayOrderDate { get; set; }

    public OrderEnum.Region Region { get; set; } //TODO Add to model

    public OrderEnum.City City { get; set; } //TODO Add to model

    public OrderEnum.Category Category { get; set; } //TODO Add to model

    public OrderEnum.Product Product { get; set; } //TODO Add to model

    [Required]
    public double Quantity { get; set; }

    [Required]
    public double UnitPrice { get; set; } //TODO - Product

    [Required]
    public double TotalPrice { get; set; } //TODO - Cal
}