using StoreModels;
using System.Collections.Generic;

namespace StoreBL
{
    public interface IOrderBL
    {
        List<Order> GetCustomerOrders(int customerId);

        List<Order> GetLocationOrders(int locationId);

        Order AddOrder(Order order, Location location, Customer customer);

        Order UpdateOrder(Order order, Location location, Customer customer);

        Order ViewOrder(int orderId);

        List<Order> GetAllOrders();

        Order DeleteOrder(Order order);
    }

    public interface IProductBL
    {
        Product AddProduct(Product product);

        List<Product> GetAllProducts();

        Product GetProductById(int id);

        Product EditProduct(Product product);

        Product DeleteProduct(Product product);

        double GetTotal(List<int> quantity);
    }

    public interface ILocationBL
    {
        Location AddLocation(Location location);

        List<Location> GetAllLocations();

        Location GetLocationById(int locationId);

        Location GetLocation(string locationName);

        Location DeleteLocation(Location location);

        Location EditLocation(Location location);
    }

    public interface ILineItemBL
    {
        LineItem AddLineItem(LineItem lineItem, Product product);

        List<LineItem> GetAllLineItems();

        List<LineItem> GetLineItems(int orderID);

        LineItem DeleteLineItem(LineItem lineItem);
    }

    public interface IInventoryBL
    {
        List<int> ReplenishInventory(string nameOfStore, List<int> productQuantity);

        List<int> SubtractInventory(string nameOfStore, List<int> quantity);

        List<Inventory> GetStoreInventoryByLocation(int locationId);

        Inventory GetStoreInventory(int inventoryId);

        List<Inventory> GetAllInventories();

        Inventory AddInventory(Inventory inventory);

        Inventory EditInventory(Inventory inventory);
    }

    public interface ICustomerBL
    {
        List<Customer> GetAllCustomers();

        Customer AddCustomer(Customer customer);

        Customer SearchCustomer(string firstName, string lastName);

        Customer SearchCustomer(int customerId);

        Customer EditCustomer(Customer customer);

        Customer DeleteCustomer(Customer customer);
    }
}