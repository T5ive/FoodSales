using ExcelDataReader;
using FoodSales.Model.Models;
using FoodSales.Utility;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;

namespace FoodSales.Service;

public static class DisplayService
{
    public static async Task<UploadResult> Upload(IFormFile file, string rootPath)
    {
        var uploadResult = new UploadResult();
        var extension = Path.GetExtension(file.FileName);
        if (extension is ".xls" or ".xlsx")
        {
            var fileName = WebUtility.HtmlEncode(Guid.NewGuid().ToString());
            var uploads = Path.Combine(rootPath, "files");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            var filePath = Path.Combine(uploads, fileName + extension);

            await using var fileStreams = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStreams);
            fileStreams.Flush();
            await fileStreams.DisposeAsync();
            uploadResult.FileName = fileName;
            uploadResult.Path = filePath;
            uploadResult.Status = true;
            uploadResult.Result = "success";
            return uploadResult;
        }
        uploadResult.Result = "invalid file";
        return uploadResult;
    }

    public static async Task<UploadResult?> UploadApi(IFormFile file)
    {
        UploadResult? uploadResult;
        const string apiUrl = "https://localhost:7263/api/Display";

        var bytes = await file.GetBytes();
        var byteArrayContent = new ByteArrayContent(bytes);
        var content = new MultipartFormDataContent();
        content.Add(byteArrayContent, "file", file.FileName);

        var client = new HttpClient();
        var response = client.PostAsync($"{apiUrl}/Upload", content);
        var result = response.Result;
        if (result.IsSuccessStatusCode)
        {
            var readTask = result.Content.ReadAsAsync<UploadResult>();
            readTask.Wait();

            uploadResult = readTask.Result;
        }
        else
        {
            uploadResult = null;
        }

        return uploadResult;
    }

    public static IEnumerable<Order>? GetOrdersApi(string fileName)
    {
        IEnumerable<Order>? orders;
        const string apiUrl = "https://localhost:7263/api/Display";

        var client = new HttpClient();
        var response = client.GetAsync($"{apiUrl}/GetOrder?fileName={fileName}");
        var result = response.Result;
        if (result.IsSuccessStatusCode)
        {
            var readTask = result.Content.ReadAsAsync<IList<Order>>();
            readTask.Wait();

            orders = readTask.Result;
        }
        else
        {
            orders = null;
        }

        return orders;
    }

    public static IEnumerable<Order> GetOrder(string fileName)
    {
        var order = new List<Order>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var header = true;
        while (reader.Read())
        {
            if (header)
            {
                header = false;
                continue;
            }
            order.Add(new Order
            {
                OrderDate = Convert.ToDateTime(reader.GetValue(0).ToString()),
                Region = reader.GetValue(1).ToString().ToRegion(),
                City = reader.GetValue(2).ToString().ToCity(),
                Category = reader.GetValue(3).ToString().ToCategory(),
                Product = reader.GetValue(4).ToString().ToProduct(),
                Quantity = reader.GetDouble(5),
                UnitPrice = Math.Round(reader.GetDouble(6), 2),
                TotalPrice = Math.Round(reader.GetDouble(7), 2)
            });
        }

        stream.FlushAsync();
        stream.DisposeAsync();
        File.Delete(fileName);
        return order;
    }
}