using StoreModels;
using System.Collections.Generic;
namespace StoreDL
{
    public interface IRepository
    {
        List<Customer> GetAllCustomers();
        Customer AddCustomer(Customer customer);
        Location GetLocationById(int locationId);
        Location DeleteLocation(Location location);
        Product AddProduct(Product product);
        Product GetProduct(Product product);
        List<Product> GetAllProducts();
        Customer DeleteCustomer(Customer customer);
        Inventory GetStoreInventory(Inventory inventory);
        LineItem DeleteLineItem(LineItem lineItem);
        Customer GetCustomer(Customer customer);
        Customer EditCustomer(Customer customer);
        Location AddLocation(Location location);
        Location GetLocation(Location location);
        List<Location> GetAllLocations();
        Inventory GetStoreInventory(int inventoryId);
        Order AddOrder(Order order, Location location, Customer customer);
        Order UpdateOrder(Order order, Location location, Customer customer);
        Order GetOrder(Order order);
        Inventory AddInventory(Inventory inventory, Location location, Product product);
        Inventory UpdateInventory(Inventory inventory);
        List<Inventory> GetAllInventories();
        Product EditProduct(Product product);
        Product DeleteProduct(Product product);
        Order DeleteOrder(Order order);
        List<Order> GetAllOrders();
        List<LineItem> GetAllLineItems();
        LineItem AddLineItem(LineItem lineItem, Product product);
        LineItem GetLineItem(LineItem lineItem);
        Location EditLocation(Location location);
    }
}