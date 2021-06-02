using StoreModels;
using System.ComponentModel;
namespace StoreWebUI.Models
{
    public class OrderVM
    {
        public OrderVM()
        {

        }

        public OrderVM(Order order)
        {
            LocationID = order.LocationID;
            OrderID = order.OrderID;
            CustomerID = order.CustomerID;
            Total = order.Total;
            OrderDate = order.OrderDate;
        }


        [DisplayName("Order ID")]
        public int OrderID { get; set; }

        [DisplayName("Customer")]
        public int CustomerID { get; set; }


        [DisplayName("Store Name")]
        public int LocationID { get; set; }

        public double Total { get; set; }

        [DisplayName("Order Date")]
        public string OrderDate { get; set; }
    }
}
