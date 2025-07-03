using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderITDemo.Data;
using OrderITDemo.Models;
using OrderITDemo.Repository.Base;
using OrderITDemo.ViewModels;
using System.Runtime.CompilerServices;

namespace OrderITDemo.Controllers;

public class OrderController : Controller
{
    private readonly IRepository<Order> _OrderRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;
    private const int pageSize = 30;
    public OrderController
        (IRepository<Order> repository,
        UserManager<AppUser> userManager,
        ApplicationDbContext context)
    {
        _context = context;
        _OrderRepository = repository;
        _userManager = userManager;
    }
    [Authorize(Roles = "User")]
    public IActionResult Index(int page = 1)
    {
        var skipCount = (page - 1) * pageSize;

        var userId = _userManager.GetUserId(User);
        var data = _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Orders)
            .Include(o => o.User)
            .Skip(skipCount)
            .Take(pageSize)
            .ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalCount = _context.Orders.Count() - 1;
        return View(data);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult ManageOrders(int page = 1)
    {/*
        var userId = _userManager.GetUserId(User);*/
        var skipCount = (page - 1) * pageSize;
        var e = _context.Orders
            .OrderByDescending(o => o.StartOrderDate)
            .Include(o => o.Orders)
            .Include(o => o.User)
            .Include(o=>o.Delivery)
           .Skip(skipCount)
            .Take(pageSize)
            .ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalCount = _context.Orders.Count() - 1;
        ViewBag.DD = "All";
        return View(e);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult CancelOrder(int page = 1)
    {/*
        var userId = _userManager.GetUserId(User);*/
        var skipCount = (page - 1) * pageSize;
        var e = _context.Orders
            .OrderByDescending(o => o.StartOrderDate)
            .Where(o => o.OrderStatuss == Order.OrderStatus.Cancelled)
            .Include(o => o.Orders)
            .Include(o => o.User)
          .Skip(skipCount)
            .Take(pageSize)
            .ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalCount = _context.Orders.Count() - 1;
        ViewBag.DD = "Cancel Orders";
        return View("ManageOrders", e);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult PendeingOrder(int page = 1)
    {/*
        var userId = _userManager.GetUserId(User);*/
        var skipCount = (page - 1) * pageSize;
        var e = _context.Orders
            .OrderByDescending(o => o.StartOrderDate)
            .Where(o => o.OrderStatuss == Order.OrderStatus.Pendeing)
            .Include(o => o.Orders)
            .Include(o => o.User)
            .Skip(skipCount)
            .Take(pageSize)
            .ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalCount = _context.Orders.Count() - 1;
        ViewBag.DD = "Pendeing Orders";
        return View("ManageOrders", e);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult ProcessingOrder(int page = 1)
    {/*
        var userId = _userManager.GetUserId(User);*/
        var skipCount = (page - 1) * pageSize;
        var e = _context.Orders
            .OrderByDescending(o => o.StartOrderDate)
            .Where(o => o.OrderStatuss == Order.OrderStatus.Processing)
            .Include(o => o.Orders)
            .Include(o => o.User)
            .Skip(skipCount)
            .Take(pageSize)
            .ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalCount = _context.Orders.Count() - 1;
        ViewBag.DD = "Processing Orders";
        return View("ManageOrders", e);
    }
    [HttpPost]
    public async Task<IActionResult> AddOrder(CartViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index), "Cart");
        }
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var carts = _context.CartsItem.Where(c => c.UserId == user.Id).ToList();

            var order = new Order
            {
                UserId = user.Id,
                StartOrderDate = DateTime.Now,
                OrderStatuss = Order.OrderStatus.Pendeing,
                Carts = carts,
                TotalAmount = 0,
                Address = model.Address
            };
            foreach (var item in carts)
            {
                order.TotalAmount += item.TotalPrice;
            }       
            await _context.Orders.AddAsync(order);
            _context.SaveChanges();

            foreach (var cartItem in order.Carts)
            {
                if (cartItem.MenuName != null)
                {
                    var orderItems = new OrderItems
                    {
                        OrderId = order.OrderId,
                        MenuName = cartItem.MenuName,
                        Quentity = cartItem.Quantity,
                        Price = cartItem.TotalPrice,

                    };
                    _context.OrderItems.Add(orderItems);
                    _context.SaveChanges();

                }
            }
            foreach (var cart in carts)
            {
                _context.CartsItem.Remove(cart);
            }
            var OrdersDet = _context.OrderItems.Where(o => o.OrderId == order.OrderId).ToList();
            order.Orders = OrdersDet;
        }
        _context.SaveChanges();
        return RedirectToAction(nameof(Index), "Home");
    }
    public IActionResult Delete(int id)
    {
        var find = _context.Orders.FirstOrDefault(o => o.OrderId == id);
        if (find != null)
        {
            find.OrderStatuss = Order.OrderStatus.Cancelled;
            _context.Update(find);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return NotFound();
        }
    }
    public IActionResult DeleteAdmin(int id)
    {
        var find = _context.Orders.FirstOrDefault(o => o.OrderId == id);
        if (find != null)
        {
            find.OrderStatuss = Order.OrderStatus.Cancelled;
            _context.Update(find);
            _context.SaveChanges();
            return RedirectToAction(nameof(ManageOrders));
        }
        else
        {
            return NotFound();
        }
    }
    public IActionResult AcceptOrder(int id)
    {
        var find = _context.Orders.FirstOrDefault(o => o.OrderId == id);
        if (find != null)
        {
            find.OrderStatuss = Order.OrderStatus.Processing;
            _context.Update(find);
            _context.SaveChanges();
            return RedirectToAction(nameof(ManageOrders));

        }
        else
        {
            return NotFound();
        }
    }

    public IActionResult OnDelivery(int id)
    {
        var find = _context.Orders.FirstOrDefault(o => o.OrderId == id);
        if (find != null)
        {
            find.OrderStatuss = Order.OrderStatus.UnderDelivery;
            find.StartDeliveryDate = DateTime.Now;
            _context.Update(find);
            _context.SaveChanges();
            return RedirectToAction(nameof(ManageOrders));
        }
        else
        {
            return NotFound();
        }
    }
    [Authorize(Roles = "Delivery")]
    public IActionResult DriverIndex(int page = 1)
    {/*
        var userId = _userManager.GetUserId(User);*/
        var skipCount = (page - 1) * pageSize;
        var e = _context.Orders
            .OrderByDescending(o => o.StartOrderDate)
            .Where(o=>o.OrderStatuss == Order.OrderStatus.Processing && o.DeliveryId == null)
            .Include(o => o.Orders)
            .Include(o => o.User)
           .Skip(skipCount)
            .Take(pageSize)
            .ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalCount = _context.Orders.Count() - 1;
        return View(e);
    }
    [Authorize(Roles = "Delivery")]
    public IActionResult DriverOrders(int page = 1)
    {/*
        var userId = _userManager.GetUserId(User);*/
        var skipCount = (page - 1) * pageSize;
        var driver = _userManager.GetUserId(User);
        var e = _context.Orders
            .OrderByDescending(o => o.StartOrderDate)
            .Where(o => o.DeliveryId == driver )
            .Include(o => o.Orders)
            .Include(o => o.User)
           .Skip(skipCount)
            .Take(pageSize)
            .ToList();
        ViewBag.CurrentPage = page;
        ViewBag.TotalCount = _context.Orders.Count() - 1;
        return View(e);
    }
    [Authorize(Roles = "Delivery")]
    public IActionResult IsTaken(int id)
    {
        var driver = _userManager.GetUserId(User);
        var find = _context.Orders.FirstOrDefault(o => o.OrderId == id);
        if (find != null)
        {
         
            find.DeliveryId = driver;
            _context.Update(find);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(DriverIndex));
    }
    public IActionResult DoneOrder(int id)
    {
        var find = _context.Orders.FirstOrDefault(o => o.OrderId == id);
        if (find != null)
        {
            find.OrderStatuss = Order.OrderStatus.Delivered;
            find.EndOrderDate = DateTime.Now;
            _context.Update(find);
            _context.SaveChanges();
            return RedirectToAction(nameof(DriverOrders));
        }
        else
        {
            return NotFound();
        }
    }
}
