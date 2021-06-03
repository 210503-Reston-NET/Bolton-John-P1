using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// Product Model
    /// </summary>
    public class Product
    {
        public Product(string itemName, double price, string description)
        {
            this.ItemName = itemName;
            this.Price = price;
            this.Description = description;
        }

        public Product()
        {
        }

        public Product(int productId, string itemName, double price, string description) : this(itemName, price, description)
        {
            this.ProductID = productId;
        }

        public int ProductID { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ItemName { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<LineItem> LineItems { get; set; }

        public override string ToString()
        {
            return $"Item: {ItemName} \nPrice: ${Price} \nDescription: {Description}\n";
        }

        public bool Equals(Product product)
        {
            return this.ItemName.Equals(product.ItemName);
        }
    }
}