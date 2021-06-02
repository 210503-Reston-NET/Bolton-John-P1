using Microsoft.EntityFrameworkCore;
using StoreDL;
using System.Linq;
using Xunit;
using Model = StoreModels;

namespace StoreTests
{
    public class RepoTest
    {
        private readonly DbContextOptions<WidgetStoreDBContext> options;
        public RepoTest()
        {
            options = new DbContextOptionsBuilder<WidgetStoreDBContext>().UseSqlite("Filename=Test.db").Options;
            Seed();
        }
        private void Seed()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Products.AddRange(
                    new Model.Product
                    {
                        ProductID = 1,
                        ItemName = "fake",
                        Price = 5.99,
                        Description = "hmm"
                    },

                );

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Customers.AddRange(
                    new Model.Customer
                    {
                        CustomerID = 1,
                        FirstName = "test",
                        LastName = "customer",
                        PhoneNumber = "5555555555",
                        Email = "no@email.com",
                    },

                    );

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Locations.AddRange(
                    new Model.Location
                    {
                        LocationID = 1,
                        StoreName = "unit1",
                        Address = "456 sdf",
                        City = "adsf",
                        State = "TX"
                    },

                    );

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Inventories.AddRange(
                    new Model.Inventory
                    {
                        InventoryID = 1,
                        LocationID = 1,
                        ProductID = 1,
                        Quantity = 1
                    },

                    );

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Orders.AddRange(
                    new Model.Order
                    {
                        OrderID = 1,
                        CustomerID = 1,
                        LocationID = 1,
                        Total = 5.49,
                        OrderDate = "Today"
                    },

                    );

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.LineItems.AddRange(
                    new Model.LineItem
                    {
                        LineItemID = 1,
                        ProductID = 1,
                        Quantity = 1,
                        OrderID = 1
                    },

                    );
                context.SaveChanges();
            }
        }
        [Fact]
        public void GetAllProductsShouldReturnAllProducts()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                IRepository _repo = new RepoDB(context);
                var products = _repo.GetAllProducts();
                Assert.Equal(1, products.Count);
            }
        }

        [Fact]
        public void GetAllCustomersShouldReturnAllCustomers()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                IRepository _repo = new RepoDB(context);
                var customers = _repo.GetAllCustomers();
                Assert.Equal(1, customers.Count);
            }
        }

        [Fact]
        public void GetAllLocationsShouldReturnAllLocations()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                IRepository _repo = new RepoDB(context);
                var locations = _repo.GetAllLocations();
                Assert.Equal(1, locations.Count);
            }
        }

        [Fact]
        public void GetAllInventoriesShouldReturnAllInventories()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                IRepository _repo = new RepoDB(context);
                var inventories = _repo.GetAllInventories();
                Assert.Equal(1, inventories.Count);
            }
        }

        [Fact]
        public void GetAllOrdersShouldReturnAllOrders()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                IRepository _repo = new RepoDB(context);
                var orders = _repo.GetAllOrders();
                Assert.Equal(1, orders.Count);
            }
        }

        [Fact]
        public void GetAllLineItemsShouldReturnAllLineItems()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                IRepository _repo = new RepoDB(context);
                var lineItems = _repo.GetAllLineItems();
                Assert.Equal(1, lineItems.Count);
            }
        }

 

        [Fact]
        public void AddCustomerShouldAddCustomer()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                IRepository _repo = new RepoDB(context);
                _repo.AddCustomer(new Model.Customer("firstName", "lastName", "phone#", "email"));
            }

            using (var assertContext = new WidgetStoreDBContext(options))
            {
                var result = assertContext.Customers.FirstOrDefault(custo => custo.CustomerID == 3);
                Assert.NotNull(result);
                Assert.Equal("firstName", result.FirstName);
            }
        }


        [Fact]
        public void AddLocationShouldAddLocation()
        {
            using (var context = new WidgetStoreDBContext(options))
            {
                IRepository _repo = new RepoDB(context);
                _repo.AddLocation(new Model.Location("name", "address", "city", "state"));
            }

            using (var assertContext = new WidgetStoreDBContext(options))
            {
                var result = assertContext.Locations.FirstOrDefault(loca => loca.LocationID == 3);
                Assert.NotNull(result);
                Assert.Equal("name", result.StoreName);
            }
        }

    }
}
