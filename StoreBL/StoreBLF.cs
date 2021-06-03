using StoreDL;
using StoreModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreBL
{
    public class CustomerBL : ICustomerBL
    {
        private IRepository _repo;

        public CustomerBL(IRepository repo)
        {
            _repo = repo;
        }

        public Customer AddCustomer(Customer customer)
        {
            if (_repo.GetCustomer(customer) != null)
            {
                throw new Exception("Customer already exists");
            }
            return _repo.AddCustomer(customer);
        }

        public Customer DeleteCustomer(Customer customer)
        {
            Customer toBeDeleted = _repo.GetCustomer(customer);
            if (toBeDeleted != null)
            {
                return _repo.DeleteCustomer(toBeDeleted);
            }
            else
            {
                throw new Exception("Customer Does Not Exist");
            }
        }

        public Customer EditCustomer(Customer customer)
        {
            return _repo.EditCustomer(customer);
        }

        public List<Customer> GetAllCustomers()
        {
            return _repo.GetAllCustomers();
        }

        public Customer SearchCustomer(string firstName, string lastName)
        {
            List<Customer> customers = GetAllCustomers();
            if (customers.Count == 0)
            {
                throw new Exception("No customers found");
            }
            else
            {
                foreach (Customer customer in customers)
                {
                    if (firstName.Equals(customer.FirstName) && lastName.Equals(customer.LastName))
                    {
                        return customer;
                    }
                }
                throw new Exception("No matching customer found");
            }
        }

        public Customer SearchCustomer(int customerId)
        {
            List<Customer> customers = GetAllCustomers();
            if (customers.Count == 0)
            {
                throw new Exception("No customers found");
            }
            else
            {
                foreach (Customer customer in customers)
                {
                    if (customerId.Equals(customer.CustomerID))
                    {
                        return customer;
                    }
                }
                throw new Exception("No matching customer found");
            }
        }
    }

    public class InventoryBL : IInventoryBL
    {
        private IRepository _repo;
        private ILocationBL _locationBL;
        private IProductBL _productBL;

        public InventoryBL(IRepository repo, ILocationBL locationBL, IProductBL productBL)
        {
            _repo = repo;
            _locationBL = locationBL;
            _productBL = productBL;
        }

        public Inventory AddInventory(Inventory inventory)
        {
            if (_repo.GetStoreInventory(inventory) != null)
            {
                throw new Exception("Store inventory already exists");
            }
            Location location = _locationBL.GetLocationById(inventory.LocationID);
            Product product = _productBL.GetProductById(inventory.ProductID);
            return _repo.AddInventory(inventory, location, product);
        }

        public Inventory EditInventory(Inventory inventory)
        {
            _repo.UpdateInventory(inventory);
            return inventory;
        }

        public List<Inventory> GetAllInventories()
        {
            return _repo.GetAllInventories();
        }

        public Inventory GetStoreInventory(int inventoryId)
        {
            return _repo.GetStoreInventory(inventoryId);
        }

        public List<Inventory> GetStoreInventoryByLocation(int locationId)
        {
            List<Inventory> inventories = _repo.GetAllInventories();
            List<Inventory> storeInventory = new List<Inventory>();
            foreach (Inventory inventory in inventories)
            {
                if (locationId.Equals(inventory.LocationID))
                {
                    storeInventory.Add(inventory);
                }
            }
            if (storeInventory.Any())
            {
                return storeInventory;
            }
            if (!inventories.Any())
            {
                throw new Exception("No inventories found");
            }
            else
            {
                throw new Exception("No matching locations found");
            }
        }

        public List<int> ReplenishInventory(string nameOfStore, List<int> productQuantity)
        {
            int i = 0;
            List<Inventory> inventories = _repo.GetAllInventories();
            List<Product> products = _repo.GetAllProducts();
            List<int> updatedInventory = new List<int>();
            Location location = _locationBL.GetLocation(nameOfStore);
            bool inventoryUpdated = false;
            foreach (Product item in products)
            {
                inventoryUpdated = false;
                if (!inventories.Any())
                {
                    Inventory newInventory = new Inventory(location.LocationID, item.ProductID, productQuantity[i]);
                    updatedInventory.Add(productQuantity[i]);
                    i++;
                    _repo.AddInventory(newInventory, location, item);
                    inventoryUpdated = true;
                }
                foreach (Inventory inventory in inventories)
                {
                    if (inventory.LocationID.Equals(location.LocationID) && inventory.ProductID.Equals(item.ProductID) && !inventoryUpdated)
                    {
                        inventory.Quantity += productQuantity[i];
                        updatedInventory.Add(inventory.Quantity);
                        i++;
                        _repo.UpdateInventory(inventory);
                        inventoryUpdated = true;
                    }
                }
                if (!inventoryUpdated)
                {
                    Inventory newInventory = new Inventory(location.LocationID, item.ProductID, productQuantity[i]);
                    updatedInventory.Add(productQuantity[i]);
                    i++;
                    _repo.AddInventory(newInventory, location, item);
                }
            }
            return updatedInventory;
        }

        public List<int> SubtractInventory(string nameOfStore, List<int> productQuantity)
        {
            int i = 0;
            List<Inventory> inventories = _repo.GetAllInventories();
            List<Product> products = _repo.GetAllProducts();
            List<int> updatedInventory = new List<int>();
            Location location = _locationBL.GetLocation(nameOfStore);
            foreach (Product item in products)
            {
                // If match found update inventory
                foreach (Inventory inventory in inventories)
                {
                    if (inventory.LocationID.Equals(location.LocationID) && inventory.ProductID.Equals(item.ProductID))
                    {
                        // If not enough inventory exists for purchase, throw exception
                        if (inventory.Quantity - productQuantity[i] < 0)
                        {
                            throw new Inventory.NotEnoughInventoryException("Not enough item in inventory");
                        }
                        inventory.Quantity -= productQuantity[i];
                        updatedInventory.Add(inventory.Quantity);
                        i++;
                        _repo.UpdateInventory(inventory);
                    }
                }
            }
            return updatedInventory;
        }
    }

    public class LineItemBL : ILineItemBL
    {
        private IRepository _repo;

        public LineItemBL(IRepository repo)
        {
            _repo = repo;
        }

        public LineItem AddLineItem(LineItem lineItem, Product product)
        {
            return _repo.AddLineItem(lineItem, product);
        }

        public LineItem DeleteLineItem(LineItem lineItem)
        {
            return _repo.DeleteLineItem(lineItem);
        }

        public List<LineItem> GetAllLineItems()
        {
            return _repo.GetAllLineItems();
        }

        public List<LineItem> GetLineItems(int orderID)
        {
            List<LineItem> lineItems = GetAllLineItems();
            List<LineItem> requestedLineItems = new List<LineItem>();
            if (lineItems.Count == 0)
            {
                throw new Exception("No Orders Have Been Placed");
            }
            else
            {
                foreach (LineItem lineItem in lineItems)
                {
                    if (orderID.Equals(lineItem.OrderID))
                    {
                        requestedLineItems.Add(lineItem);
                    }
                }
                if (!requestedLineItems.Any())
                {
                    throw new Exception("No Matching Orders Have Been Found");
                }
                else
                {
                    return requestedLineItems;
                }
            }
        }
    }

    public class LocationBL : ILocationBL
    {
        private IRepository _repo;

        public LocationBL(IRepository repo)
        {
            _repo = repo;
        }

        public Location AddLocation(Location location)
        {
            if (_repo.GetLocation(location) != null)
            {
                throw new Exception("Location Already Exists");
            }
            return _repo.AddLocation(location);
        }

        public List<Location> GetAllLocations()
        {
            return _repo.GetAllLocations();
        }

        public Location GetLocationById(int locationId)
        {
            List<Location> locations = GetAllLocations();
            if (locations.Count == 0)
            {
                throw new Exception("No Locations Found");
            }
            else
            {
                foreach (Location location in locations)
                {
                    if (locationId.Equals(location.LocationID))
                    {
                        return location;
                    }
                }
                throw new Exception("No matching locations found");
            }
        }

        public Location GetLocation(string name)
        {
            List<Location> locations = GetAllLocations();
            if (locations.Count == 0)
            {
                throw new Exception("No Locations Found");
            }
            else
            {
                foreach (Location location in locations)
                {
                    if (name.Equals(location.StoreName))
                    {
                        return location;
                    }
                }
                throw new Exception("No Matching Location Found");
            }
        }

        public Location DeleteLocation(Location location)
        {
            Location toBeDeleted = _repo.GetLocation(location);
            if (toBeDeleted != null)
            {
                return _repo.DeleteLocation(toBeDeleted);
            }
            else
            {
                throw new Exception("The Location Does Not Exist Yet");
            }
        }

        public Location EditLocation(Location location)
        {
            return _repo.EditLocation(location);
        }
    }

    public class OrderBL : IOrderBL
    {
        private IRepository _repo;

        public OrderBL(IRepository repo)
        {
            _repo = repo;
        }

        public Order AddOrder(Order order, Location location, Customer customer)
        {
            return _repo.AddOrder(order, location, customer);
        }

        public Order UpdateOrder(Order order, Location location, Customer customer)
        {
            return _repo.UpdateOrder(order, location, customer);
        }

        public List<Order> GetAllOrders()
        {
            return _repo.GetAllOrders();
        }

        public List<Order> GetCustomerOrders(int customerId)
        {
            List<Order> orders = _repo.GetAllOrders();
            List<Order> customerOrders = new List<Order>();
            foreach (Order order in orders)
            {
                if (customerId.Equals(order.CustomerID))
                {
                    customerOrders.Add(order);
                }
            }
            if (customerOrders.Any())
            {
                return customerOrders;
            }
            else
            {
                throw new Exception("No matching orders found");
            }
        }

        public List<Order> GetLocationOrders(int locationId)
        {
            List<Order> orders = _repo.GetAllOrders();
            List<Order> locationOrders = new List<Order>();
            foreach (Order order in orders)
            {
                if (locationId.Equals(order.LocationID))
                {
                    locationOrders.Add(order);
                }
            }
            if (locationOrders.Any())
            {
                return locationOrders;
            }
            else
            {
                throw new Exception("No matching orders found");
            }
        }

        public Order ViewOrder(int orderId)
        {
            List<Order> orders = _repo.GetAllOrders();
            foreach (Order order in orders)
            {
                if (orderId.Equals(order.OrderID))
                {
                    return order;
                }
            }
            throw new Exception("No matching orders found");
        }

        public Order DeleteOrder(Order order)
        {
            return _repo.DeleteOrder(order);
        }
    }

    public class ProductBL : IProductBL
    {
        private IRepository _repo;

        public ProductBL(IRepository repo)
        {
            _repo = repo;
        }

        public Product AddProduct(Product product)
        {
            if (_repo.GetProduct(product) != null)
            {
                throw new Exception("Product already added");
            }
            return _repo.AddProduct(product);
        }

        public Product DeleteProduct(Product product)
        {
            Product toBeDeleted = _repo.GetProduct(product);
            if (toBeDeleted != null)
            {
                return _repo.DeleteProduct(toBeDeleted);
            }
            else
            {
                throw new Exception("Product not in system");
            }
        }

        public Product EditProduct(Product product)
        {
            return _repo.EditProduct(product);
        }

        public List<Product> GetAllProducts()
        {
            return _repo.GetAllProducts();
        }

        public Product GetProductById(int productId)
        {
            List<Product> products = GetAllProducts();
            if (products.Count == 0)
            {
                throw new Exception("Product not Found");
            }
            else
            {
                foreach (Product item in products)
                {
                    if (productId.Equals(item.ProductID))
                    {
                        return item;
                    }
                }
                throw new Exception("Product not found");
            }
        }

        public double GetTotal(List<int> quantity)
        {
            List<Product> products = GetAllProducts();
            int i = 0;
            double total = 0;
            foreach (Product item in products)
            {
                total += quantity[i] * item.Price;
                i++;
            }
            total = Math.Round(total, 2);
            return total;
        }
    }
}