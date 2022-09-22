using ExcelDataReader;
using FoodSales.Model.Models;
using FoodSales.Repository.IRepository;
using FoodSales.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Text;

namespace FoodSales.API.Areas.Admin.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IUnitOfWork _unitOfWork;

    public FileController(IWebHostEnvironment env, IUnitOfWork unitOfWork)
    {
        _env = env;
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Route("UploadFile")]
    public async Task<ActionResult<UploadResult>> UploadFile(IFormFile? file)
    {
        var uploadResult = new UploadResult();

        if (file != null)
        {
            try
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension is ".xls" or ".xlsx")
                {
                    var rootPath = _env.ContentRootPath;
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
                    uploadResult.Update(fileName, filePath, true, "success");
                    return Ok(uploadResult);
                }
                uploadResult.Result = "invalid file";
            }
            catch (Exception e)
            {
                uploadResult.Result = e.Message;
            }
        }

        return BadRequest(uploadResult);
    }

    [HttpGet]
    [Route("ImportOrder")]
    public IActionResult ImportOrder(string fileName)
    {
        var order = new List<Order>();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var header = true;
        while (reader.Read())
        {
            try
            {
                if (header)
                {
                    header = false;
                    continue;
                }

                var date = Convert.ToDateTime(reader.GetValue(0).ToString());
                var region = reader.GetValue(1).ToString() ?? "";
                var city = reader.GetValue(2).ToString() ?? "";
                var category = reader.GetValue(3).ToString() ?? "";
                var product = reader.GetValue(4).ToString() ?? "";
                order.Add(new Order
                {
                    OrderDate = date,
                    DisplayOrderDate = date.ToString("d", CultureInfo.InvariantCulture),
                    Region = region.ToRegion(),
                    City = city.ToCity(),
                    Category = category.ToCategory(),
                    Product = product.ToProduct(),
                    Quantity = reader.GetDouble(5),
                    UnitPrice = Math.Round(reader.GetDouble(6), 2),
                    TotalPrice = Math.Round(reader.GetDouble(7), 2)
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        stream.FlushAsync();
        stream.DisposeAsync();
        try
        {
            System.IO.File.Delete(fileName);
            _unitOfWork.Order.AddList(order);
            _unitOfWork.Save();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}