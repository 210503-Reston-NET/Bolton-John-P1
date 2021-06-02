using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// Order Model
    /// </summary>
    public class Order
    {
        public Order(int customerId, int locationId, double total, string orderDate)
        {
            this.LocationID = locationId;
            this.CustomerID = customerId;
            this.Total = total;
            this.OrderDate = orderDate;
        }

        public Order()
        {

        }

        public Order(int orderId, int locationId, int customerId, double total, string orderDate) : this(locationId, customerId, total, orderDate)
        {
            this.OrderID = orderId;
        }


        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public int CustomerID { get; set; }
        public int LocationID { get; set; }
        public double Total { get; set; }

        public List<LineItem> LineItems { get; set; }
    }
}