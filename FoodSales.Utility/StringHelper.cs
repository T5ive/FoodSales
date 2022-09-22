namespace FoodSales.Utility;

public static class StringHelper
{
    public static OrderEnum.Region ToRegion(this string str)
    {
        return str switch
        {
            "East" => OrderEnum.Region.East,
            "West" => OrderEnum.Region.West
        };
    }

    public static OrderEnum.City ToCity(this string str)
    {
        return str switch
        {
            "Boston" => OrderEnum.City.Boston,
            "New York" => OrderEnum.City.NewYork,
            "Los Angeles" => OrderEnum.City.LosAngeles,
            "San Diego" => OrderEnum.City.SanDiego
        };
    }

    public static OrderEnum.Category ToCategory(this string str)
    {
        return str switch
        {
            "Bars" => OrderEnum.Category.Bars,
            "Cookies" => OrderEnum.Category.Cookies,
            "Crackers" => OrderEnum.Category.Crackers,
            "Snacks" => OrderEnum.Category.Snacks
        };
    }

    public static OrderEnum.Product ToProduct(this string str)
    {
        return str switch
        {
            "Arrowroot" => OrderEnum.Product.Arrowroot,
            "Banana" => OrderEnum.Product.Banana,
            "Bran" => OrderEnum.Product.Bran,
            "Carrot" => OrderEnum.Product.Carrot,
            "ChocolateChip" => OrderEnum.Product.ChocolateChip,
            "OatmealRaisin" => OrderEnum.Product.OatmealRaisin,
            "PotatoChips" => OrderEnum.Product.PotatoChips,
            "Pretzels" => OrderEnum.Product.Pretzels,
            "WholeWheat" => OrderEnum.Product.WholeWheat
        };
    }
}