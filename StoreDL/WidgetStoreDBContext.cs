using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class WidgetStoreDBContext : DbContext
    {
        public WidgetStoreDBContext(DbContextOptions options) : base(options)
        {

        }

        protected WidgetStoreDBContext()
        {

        }

        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().Property(customer => customer.CustomerID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Inventory>().Property(inventory => inventory.InventoryID).ValueGeneratedOnAdd();
            modelBuilder.Entity<LineItem>().Property(lineItem => lineItem.LineItemID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Location>().Property(location => location.LocationID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>().Property(order => order.OrderID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(product => product.ProductID).ValueGeneratedOnAdd();
        }
    }
}