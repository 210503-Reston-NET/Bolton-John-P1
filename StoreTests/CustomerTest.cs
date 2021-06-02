using StoreModels;
using Xunit;
namespace StoreTests
{
    public class CustomerTest
    {
        [Fact]
        public void CustomerShouldSetValidData()
        {
            string firstName = "JOHN";
            Customer test = new Customer("firstName", "lastName", "phone", "email");

            test.FirstName = firstName;

            Assert.Equal(firstName, test.FirstName);
        }



        [Fact]
        public void InventoryShouldSetValidData()
        {
            int quantity = 1;
            Inventory test = new Inventory(1, 1, 1);

            test.Quantity = quantity;

            Assert.Equal(quantity, test.Quantity);
        }

        [Fact]
        public void LineItemShouldSetValidData()
        {
            int quantity = 1;
            LineItem test = new LineItem(1, 1, 1);

            test.Quantity = quantity;

            Assert.Equal(quantity, test.Quantity);
        }

        [Fact]
        public void LocationShouldSetValidData()
        {
            string city = "fakecity";
            Location test = new Location("name", "address", "city", "state");

            test.City = city;

            Assert.Equal(city, test.City);
        }


        [Fact]
        public void PriceShouldsetValidData()
        {
            double price = 5.99;
            Product test = new Product("name", 5.99, "description");

            test.Price = price;

            Assert.Equal(price, test.Price);
        }
    }
}
