using System.Collections.Generic;
using StoreModels;

namespace StoreDL
{
    public class StoreSCStorage
    {
        public static List<Customer> Customers = new()
        {
            new("firstName", "lastName", "phone#", "email")
        };

        public static List<Location> Locations = new()
        {
            new("name", "address", "city", "state")
        };

        public static List<Product> Products = new()
        {
            new("name", 1.99, "description")
        };

        public static List<Order> Orders = new()
        {
            new(1, 1, 1, 1.99, "")
        };

        public static List<LineItem> LineItems = new()
        {
            new(1, 1, 1)
        };

        public static List<Inventory> Inventories = new()
        {
            new(1, 1, 1)
        };
    }
}