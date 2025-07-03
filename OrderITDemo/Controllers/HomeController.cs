using System.Diagnostics;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderITDemo.Data;
using OrderITDemo.Models;
using OrderITDemo.Services;

namespace OrderITDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BackGroundService _orderServices;
        private readonly ApplicationDbContext c;
        public HomeController(ILogger<HomeController> logger , BackGroundService orderServices , ApplicationDbContext a)
        {
            c = a;
            _orderServices = orderServices;
            _logger = logger;
        }
        public void CancelOrder()
        {
            var currentTime = DateTime.Now;
            var orders = c.Orders.Where(o => (o.OrderStatuss == Order.OrderStatus.Pendeing)
            )
                .ToList();
            foreach (var item in orders)
            {
                /* if ((item.StartOrderDate.Minute == DateTime.Now.Minute) && (item.StartOrderDate.Hour - )
                 {*/
                item.OrderStatuss = Order.OrderStatus.Cancelled;
                c.Update(item);
            }
            c.SaveChanges();
        }
        public IActionResult Index()
        {
            RecurringJob.AddOrUpdate(()=>CancelOrder() , Cron.Hourly );
        //    _orderServices.CancelOrder();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
