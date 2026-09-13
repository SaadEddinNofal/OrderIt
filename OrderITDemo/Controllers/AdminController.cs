using OrderITDemo.Data;
using OrderITDemo.Models;
using OrderITDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OrderITDemo.Controllers;

public class AdminController : Controller
{
	private readonly ApplicationDbContext _context2;

	public AdminController(ApplicationDbContext context2)
	{
		_context2 = context2;
	}

	[Authorize(Roles = "Admin,Delivery")]
	public async Task<IActionResult> Dashboard()
	{
		var counts = new DashboardViewModel
		{
			TotalUsers = await _context2.Users.CountAsync(),
			TotalCategories = await _context2.Categories.CountAsync(),
			TotalMenuItems = await _context2.Menus.CountAsync(),
			TotalOrders = await _context2.Orders.CountAsync(),
			PendingOrders = await _context2.Orders.CountAsync(o => o.OrderStatuss == Order.OrderStatus.Pendeing),
			ProcessingOrders = await _context2.Orders.CountAsync(o => o.OrderStatuss == Order.OrderStatus.Processing),
			UnderDeliveryOrders = await _context2.Orders.CountAsync(o => o.OrderStatuss == Order.OrderStatus.UnderDelivery),
			DeliveredOrders = await _context2.Orders.CountAsync(o => o.OrderStatuss == Order.OrderStatus.Delivered),
			CancelledOrders = await _context2.Orders.CountAsync(o => o.OrderStatuss == Order.OrderStatus.Cancelled)
		};

		return View(counts);
	}
}
