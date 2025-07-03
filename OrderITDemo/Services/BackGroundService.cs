using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderITDemo.Data;
using OrderITDemo.Models;
using OrderITDemo.Repository.Base;

namespace OrderITDemo.Services
{
    public class BackGroundService
    {
        private readonly ApplicationDbContext _context;
        public BackGroundService(ApplicationDbContext context)
        {
            _context = context;
         
        }
        public void CancelOrder()
        {
            var currentTime = DateTime.Now;
            var orders = _context.Orders.Where(o => (o.OrderStatuss == Order.OrderStatus.Pendeing)
            &&o.StartOrderDate<currentTime.AddHours(-1)
            )
                .ToList();
            foreach (var item in orders)
            {
               /* if ((item.StartOrderDate.Minute == DateTime.Now.Minute) && (item.StartOrderDate.Hour - )
                {*/
                    item.OrderStatuss = Order.OrderStatus.Cancelled;
                _context.Update(item);

            }
            _context.SaveChanges();
        }
    }
}
