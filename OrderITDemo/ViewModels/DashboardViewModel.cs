namespace OrderITDemo.ViewModels;

public class DashboardViewModel
{
	public int TotalUsers { get; set; }
	public int TotalCategories { get; set; }
	public int TotalMenuItems { get; set; }
	public int TotalOrders { get; set; }

	public int PendingOrders { get; set; }
	public int ProcessingOrders { get; set; }
	public int UnderDeliveryOrders { get; set; }
	public int DeliveredOrders { get; set; }
	public int CancelledOrders { get; set; }
}
