using FoodSales.Model.Models;
using FoodSales.Utility;
using Microsoft.AspNetCore.Http;

namespace FoodSales.Service;

public static class AdminService
{
    private const string ApiUrl = "https://localhost:7263/api/";

    public static async Task<UploadResult?> UploadFile(IFormFile file)
    {
        UploadResult? uploadResult;

        var bytes = await file.GetBytes();
        var byteArrayContent = new ByteArrayContent(bytes);
        var content = new MultipartFormDataContent();
        content.Add(byteArrayContent, "file", file.FileName);

        var client = new HttpClient();
        var response = client.PostAsync($"{ApiUrl}File/UploadFile", content);
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

    public static bool ImportOrder(string fileName)
    {
        var client = new HttpClient();
        var response = client.GetAsync($"{ApiUrl}File/ImportOrder?fileName={fileName}");
        var result = response.Result;
        return result.IsSuccessStatusCode;
    }

    public static IEnumerable<Order> GetOrders()
    {
        IEnumerable<Order>? orders;

        var client = new HttpClient();
        var response = client.GetAsync($"{ApiUrl}Order/GetAll");
        var result = response.Result;
        if (result.IsSuccessStatusCode)
        {
            var readTask = result.Content.ReadAsAsync<IList<Order>>();
            readTask.Wait();

            orders = readTask.Result;
        }
        else
        {
            orders = new List<Order>();
        }

        return orders;
    }

    public static IEnumerable<Order> GetOrders(DateTime startDate, DateTime endDate)
    {
        IEnumerable<Order>? orders;

        var client = new HttpClient();
        var response = client.GetAsync($"{ApiUrl}Order/GetBetween?startDate={startDate}&endDate={endDate}");
        var result = response.Result;
        if (result.IsSuccessStatusCode)
        {
            var readTask = result.Content.ReadAsAsync<IList<Order>>();
            readTask.Wait();

            orders = readTask.Result;
        }
        else
        {
            orders = new List<Order>();
        }

        return orders;
    }

    public static Order? GetOrder(int id)
    {
        var client = new HttpClient();
        var response = client.GetAsync($"{ApiUrl}Order/Get?id={id}");
        var result = response.Result;
        if (result.IsSuccessStatusCode)
        {
            var readTask = result.Content.ReadAsAsync<Order>();
            readTask.Wait();

            return readTask.Result;
        }
        return null;
    }

    public static async Task<bool> Create(Order obj)
    {
        var client = new HttpClient();
        var response = client.PostAsJsonAsync($"{ApiUrl}Order/Create", obj);
        response.Wait();
        var result = response.Result;
        return await Task.FromResult(result.IsSuccessStatusCode);
    }

    public static async Task<bool> Edit(Order obj)
    {
        var client = new HttpClient();
        var response = client.PostAsJsonAsync($"{ApiUrl}Order/Edit", obj);
        response.Wait();
        var result = response.Result;
        return await Task.FromResult(result.IsSuccessStatusCode);
    }

    public static async Task<bool> Delete(int id)
    {
        var client = new HttpClient();
        var response = client.GetAsync($"{ApiUrl}Order/Delete?id={id}");
        var result = response.Result;
        return await Task.FromResult(result.IsSuccessStatusCode);
    }
}