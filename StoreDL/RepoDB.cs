using System.Collections.Generic;
using System.Linq;
using StoreModels;

namespace StoreDL
{
    public class RepoDB : IRepository
    {
        private readonly WidgetStoreDBContext _context;

        public RepoDB(WidgetStoreDBContext context)
        {
            _context = context;
        }

        public Customer AddCustomer(Customer customer)
        {
            _context.Customers.Add(
                new Customer
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PhoneNumber = customer.PhoneNumber,
                    Email = customer.Email
                }
            );
            _context.SaveChanges();
            return customer;
        }

        public LineItem AddLineItem(LineItem lineItem, Product product)
        {
            _context.LineItems.Add(
                new LineItem
                {
                    ProductID = lineItem.ProductID,
                    Quantity = lineItem.Quantity,
                    OrderID = lineItem.OrderID
                }
            );
            _context.SaveChanges();
            return lineItem;
        }

        public Location AddLocation(Location location)
        {
            _context.Locations.Add(
                new Location
                {
                    StoreName = location.StoreName,
                    Address = location.Address,
                    City = location.City,
                    State = location.State
                }
            );
            _context.SaveChanges();
            return location;
        }

        public Order AddOrder(Order order, Location location, Customer customer)
        {
            _context.Orders.Add(
                new Order
                {
                    LocationID = order.LocationID,
                    CustomerID = order.CustomerID,
                    Total = order.Total,
                    OrderDate = order.OrderDate
                }
            );
            _context.SaveChanges();
            return order;
        }

        public Product AddProduct(Product product)
        {
            _context.Products.Add(
                new Product
                {
                    ItemName = product.ItemName,
                    Price = product.Price,
                    Description = product.Description
                }
            );
            _context.SaveChanges();
            return product;
        }

        public Inventory GetStoreInventory(Inventory inventory)
        {
            var found = _context.Inventories.FirstOrDefault(inven =>
                inven.LocationID == inventory.LocationID && inven.ProductID == inventory.ProductID &&
                inven.Quantity == inventory.Quantity);
            if (found == null) return null;
            return new Inventory(found.InventoryID, inventory.LocationID, inventory.ProductID, found.Quantity);
        }

        public Inventory AddInventory(Inventory inventory, Location location, Product product)
        {
            _context.Inventories.Add(
                new Inventory
                {
                    InventoryID = inventory.InventoryID,
                    LocationID = GetLocation(location).LocationID,
                    ProductID = GetProduct(product).ProductID,
                    Quantity = inventory.Quantity
                }
            );
            _context.SaveChanges();
            return inventory;
        }

        public Inventory UpdateInventory(Inventory inventory)
        {
            var updateInventory = _context.Inventories.First(inven => inven.InventoryID == inventory.InventoryID);
            updateInventory.Quantity = inventory.Quantity;
            _context.SaveChanges();
            return inventory;
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.Select(customer =>
                new Customer(customer.CustomerID, customer.FirstName, customer.LastName, customer.PhoneNumber,
                    customer.Email)
            ).ToList();
        }

        public List<Inventory> GetAllInventories()
        {
            return _context.Inventories.Select(inventory =>
                new Inventory(inventory.InventoryID, inventory.LocationID, inventory.ProductID, inventory.Quantity)
            ).ToList();
        }

        public Customer GetCustomer(Customer customer)
        {
            var found = _context.Customers.FirstOrDefault(custo =>
                custo.FirstName == customer.FirstName && custo.LastName == customer.LastName &&
                custo.PhoneNumber == customer.PhoneNumber && custo.Email == customer.Email);
            if (found == null) return null;
            return new Customer(found.CustomerID, found.FirstName, found.LastName, found.PhoneNumber, found.Email);
        }

        public LineItem GetLineItem(LineItem lineItem)
        {
            var found = _context.LineItems.FirstOrDefault(li =>
                li.ProductID == lineItem.ProductID && li.Quantity == lineItem.Quantity &&
                li.OrderID == lineItem.OrderID);
            if (found == null) return null;
            return new LineItem(found.LineItemID, lineItem.ProductID, found.Quantity, found.OrderID);
        }

        public Location GetLocation(Location location)
        {
            var found = _context.Locations.FirstOrDefault(loca =>
                loca.StoreName == location.StoreName && loca.Address == location.Address &&
                loca.City == location.City && loca.State == location.State);
            if (found == null) return null;
            return new Location(found.LocationID, found.StoreName, found.Address, found.City, found.State);
        }

        public List<LineItem> GetAllLineItems()
        {
            return _context.LineItems.Select(lineItem =>
                new LineItem(lineItem.ProductID, lineItem.Quantity, lineItem.OrderID)
            ).ToList();
        }

        public List<Location> GetAllLocations()
        {
            return _context.Locations.Select(location =>
                new Location(location.LocationID, location.StoreName, location.Address, location.City, location.State)
            ).ToList();
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.Select(order =>
                new Order(order.OrderID, order.CustomerID, order.LocationID, order.Total, order.OrderDate)
            ).ToList();
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.Select(product =>
                new Product(product.ProductID, product.ItemName, product.Price, product.Description)
            ).ToList();
        }

        public Order GetOrder(Order order)
        {
            var found = _context.Orders.FirstOrDefault(ord =>
                ord.LocationID == order.LocationID && ord.CustomerID == order.CustomerID && ord.Total == order.Total &&
                ord.OrderDate == order.OrderDate);
            if (found == null) return null;
            return new Order(found.OrderID, order.LocationID, order.CustomerID, found.Total, found.OrderDate);
        }

        public Order DeleteOrder(Order order)
        {
            var deleteOrder = _context.Orders.First(ord => ord.OrderID == order.OrderID);
            if (deleteOrder != null)
            {
                _context.Orders.Remove(deleteOrder);
                _context.SaveChanges();
                return order;
            }

            return null;
        }

        public Product GetProduct(Product product)
        {
            var found = _context.Products.FirstOrDefault(prod =>
                prod.ItemName == product.ItemName && prod.Price == product.Price &&
                prod.Description == product.Description);
            if (found == null) return null;
            return new Product(found.ProductID, found.ItemName, found.Price, found.Description);
        }

        public Order UpdateOrder(Order order, Location location, Customer customer)
        {
            var updateOrder = _context.Orders.First(ord => ord.OrderID == order.OrderID);
            updateOrder.Total = order.Total;
            _context.SaveChanges();
            return order;
        }

        public LineItem DeleteLineItem(LineItem lineItem)
        {
            var deleteLineItem = _context.LineItems.First(li => li.LineItemID == lineItem.LineItemID);
            if (deleteLineItem != null)
            {
                _context.LineItems.Remove(deleteLineItem);
                _context.SaveChanges();
                return lineItem;
            }

            return null;
        }

        public Location GetLocationById(int locationId)
        {
            return _context.Locations.Find(locationId);
        }

        public Location DeleteLocation(Location location)
        {
            var toBeDeleted = _context.Locations.First(loca => loca.LocationID == location.LocationID);
            _context.Locations.Remove(toBeDeleted);
            _context.SaveChanges();
            return location;
        }

        public Location EditLocation(Location location)
        {
            var editLocation = _context.Locations.First(loca => loca.LocationID == location.LocationID);
            editLocation.StoreName = location.StoreName;
            editLocation.City = location.City;
            editLocation.State = location.State;
            editLocation.Address = location.Address;
            _context.SaveChanges();
            return location;
        }

        public Customer EditCustomer(Customer customer)
        {
            var editCustomer = _context.Customers.First(custo => custo.CustomerID == customer.CustomerID);
            editCustomer.FirstName = customer.FirstName;
            editCustomer.LastName = customer.LastName;
            editCustomer.PhoneNumber = customer.PhoneNumber;
            editCustomer.Email = customer.Email;
            _context.SaveChanges();
            return customer;
        }

        public Customer DeleteCustomer(Customer customer)
        {
            var toBeDeleted = _context.Customers.First(custo => custo.CustomerID == customer.CustomerID);
            _context.Customers.Remove(toBeDeleted);
            _context.SaveChanges();
            return customer;
        }

        public Product EditProduct(Product product)
        {
            var editProduct = _context.Products.First(prod => prod.ProductID == product.ProductID);
            editProduct.ItemName = product.ItemName;
            editProduct.Price = product.Price;
            editProduct.Description = product.Description;
            _context.SaveChanges();
            return product;
        }

        public Product DeleteProduct(Product product)
        {
            var toBeDeleted = _context.Products.First(prod => prod.ProductID == product.ProductID);
            _context.Products.Remove(toBeDeleted);
            _context.SaveChanges();
            return product;
        }

        public Inventory GetStoreInventory(int inventoryId)
        {
            return _context.Inventories.Find(inventoryId);
        }
    }
}