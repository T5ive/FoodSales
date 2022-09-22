using FoodSales.Model.Models;
using FoodSales.Service;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FoodSales.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller
{
    #region Index

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    #endregion Index

    #region Create

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<ActionResult> Upload(IFormFile? file)
    {
        if (file != null)
        {
            var upload = AdminService.UploadFile(file).Result;
            if (upload.Status)
            {
                var orders = AdminService.ImportOrder(upload.Path);
                if (orders)
                {
                    TempData["CreateSuccess"] = "ไฟล์ถูกนำเข้าคลังสำเร็จ";
                    return Task.FromResult<ActionResult>(View("Index", AdminService.GetOrders()));
                }
            }
        }

        return Task.FromResult<ActionResult>(BadRequest());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Order? obj)
    {
        if (obj == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            obj.DisplayOrderDate = obj.OrderDate.ToString("d", CultureInfo.InvariantCulture);
            if (AdminService.Create(obj).Result)
            {
                TempData["CreateSuccess"] = "รายการสินค้าได้ถูกสร้างขึ้นแล้ว";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        return View(obj);
    }

    #endregion Create

    #region Edit

    public IActionResult Edit(int id)
    {
        if (id is 0)
            return NotFound();

        var order = AdminService.GetOrder(id);

        if (order is null)
            return NotFound();

        return View(order);
    }

    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Order obj)
    {
        if (ModelState.IsValid)
        {
            if (AdminService.Edit(obj).Result)
            {
                TempData["EditSuccess"] = "รายการสินค้าได้ถูกแก้ไขแล้ว";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        return View(obj);
    }

    #endregion Edit

    #region Delete

    [ActionName("Delete")]
    [HttpGet]
    public IActionResult Delete(int id)
    {
        if (id is 0)
            return NotFound();

        if (AdminService.Delete(id).Result)
        {
            TempData["RemoveSuccess"] = "ได้ทำการลบข้อมูลเรียบร้อยแล้ว";
            return Json(new
            {
                success = true,
                message = "Delete Successful"
            });
        }

        return Json(new
        {
            success = false,
            message = "Error while deleting"
        });
    }

    #endregion Delete

    #region API Call

    [HttpGet]
    public IActionResult GetAll()
    {
        var list = AdminService.GetOrders();
        return Json(new { data = list });
    }

    #endregion API Call
}