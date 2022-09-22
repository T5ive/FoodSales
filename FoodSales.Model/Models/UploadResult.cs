namespace FoodSales.Model.Models;

public class UploadResult
{
    public string? FileName { get; set; }
    public string? Path { get; set; }

    public bool Status { get; set; }

    public string? Result { get; set; }

    public void Update(string fileName, string path, bool status, string result)
    {
        FileName = fileName;
        Path = path;
        Status = status;
        Result = result;
    }
}