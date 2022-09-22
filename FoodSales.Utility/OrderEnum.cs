using System.ComponentModel.DataAnnotations;

namespace FoodSales.Utility;

public class OrderEnum
{
    public enum Region
    {
        East,
        West
    }

    public enum City
    {
        Boston,

        [Display(Name = "New York")]
        NewYork,

        [Display(Name = "Los Angeles")]
        LosAngeles,

        [Display(Name = "San Diego")]
        SanDiego
    }

    public enum Category
    {
        Bars,
        Cookies,
        Crackers,
        Snacks
    }

    public enum Product
    {
        Arrowroot,
        Banana,
        Bran,
        Carrot,

        [Display(Name = "Chocolate Chip")]
        ChocolateChip,

        [Display(Name = "Oatmeal Raisin")]
        OatmealRaisin,

        [Display(Name = "Potato Chips")]
        PotatoChips,

        Pretzels,

        [Display(Name = "Whole Wheat")]
        WholeWheat
    }
}