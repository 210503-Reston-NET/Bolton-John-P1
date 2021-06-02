using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreWebUI.Models
{
    public class ProductVM
    {
        public ProductVM(Product product)
        {
            ProductID = product.ProductID;
            ItemName = product.ItemName;
            Price = product.Price;
            Description = product.Description;
        }

        public ProductVM()
        {

        }



        public int ProductID { get; set; }

        [Required]
        [DisplayName("Product Name")]
        public string ItemName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
