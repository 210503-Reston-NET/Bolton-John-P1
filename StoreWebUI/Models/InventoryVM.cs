using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using StoreModels;

namespace StoreWebUI.Models
{
    public class InventoryVM
    {
        public InventoryVM()
        {
        }

        public InventoryVM(Inventory inventory)
        {
            InventoryID = inventory.InventoryID;
            LocationID = inventory.LocationID;
            ProductID = inventory.ProductID;
            Quantity = inventory.Quantity;
        }

        public int InventoryID { get; set; }

        [DisplayName("Store Location")] public int LocationID { get; set; }

        [DisplayName("Product")] public int ProductID { get; set; }

        [Required] public int Quantity { get; set; }
    }
}