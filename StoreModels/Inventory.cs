using System;
namespace StoreModels
{
    public class Inventory
    {

        public Inventory(int locationId, int productId, int quantity)
        {
            this.LocationID = locationId;
            this.ProductID = productId;
            this.Quantity = quantity;
        }

        public Inventory()
        {

        }

        public Inventory(int inventoryId, int locationId, int productId, int quantity) : this(locationId, productId, quantity)
        {
            this.InventoryID = inventoryId;
        }

        [Serializable]
        public class NotEnoughInventoryException : Exception
        {
            public NotEnoughInventoryException(string message) : base(message) { }
        }

        public int InventoryID { get; set; }
        public int LocationID { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }

        public override string ToString()
        {
            return $"{Quantity}";
        }
    }
}