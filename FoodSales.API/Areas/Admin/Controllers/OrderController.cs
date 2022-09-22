using FoodSales.Model.Models;
using FoodSales.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace FoodSales.API.Areas.Admin.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Route("Create")]
    public IActionResult Create(Order? obj)
    {
        try
        {
            if (obj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Order.Add(obj);
                _unitOfWork.Save();
                return Ok();
            }

            return NotFound(obj);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("Get")]
    public Task<ActionResult<Order>> Get(int id)
    {
        try
        {
            var obj = _unitOfWork.Order.GetFirstOrDefault(i => i.Id == id);
            return obj is null ? Task.FromResult<ActionResult<Order>>(NotFound()) : Task.FromResult<ActionResult<Order>>(Ok(obj));
        }
        catch (Exception e)
        {
            return Task.FromResult<ActionResult<Order>>(BadRequest(e.Message));
        }
    }

    [HttpGet]
    [Route("GetBetween")]
    public Task<ActionResult<Order>> GetBetween(DateTime startDate, DateTime endDate)
    {
        try
        {
            var obj = _unitOfWork.Order.GetAll();
            var between = obj
                .Where(i => i.OrderDate >= startDate)
                .Where(i => i.OrderDate <= endDate);
            return between is null ? Task.FromResult<ActionResult<Order>>(NotFound()) : Task.FromResult<ActionResult<Order>>(Ok(between));
        }
        catch (Exception e)
        {
            return Task.FromResult<ActionResult<Order>>(BadRequest(e.Message));
        }
    }

    [HttpPost]
    [Route("Edit")]
    public IActionResult Edit(Order? obj)
    {
        try
        {
            if (obj == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Order.Update(obj);
                _unitOfWork.Save();
                return Ok();
            }

            return NotFound(obj);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("Delete")]
    public IActionResult Delete(int id)
    {
        try
        {
            var obj = _unitOfWork.Order.GetFirstOrDefault(i => i.Id == id);
            if (obj is null) return NotFound();

            _unitOfWork.Order.Remove(obj);
            _unitOfWork.Save();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("DeleteBetween")]
    public IActionResult DeleteBetween(int start, int end)
    {
        try
        {
            List<Order> orders = new List<Order>();
            for (int i = start; i <= end; i++)
            {
                var order = _unitOfWork.Order.GetFirstOrDefault(v => v.Id == i);
                if (order is null)
                    continue;
                orders.Add(order);
            }
            _unitOfWork.Order.RemoveRange(orders);
            _unitOfWork.Save();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [Route("DeleteRange")]
    public IActionResult DeleteRange(IEnumerable<Order> orders)
    {
        try
        {
            _unitOfWork.Order.RemoveRange(orders);
            _unitOfWork.Save();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("GetAll")]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Order.GetAll();
        return Ok(productList);
    }
}