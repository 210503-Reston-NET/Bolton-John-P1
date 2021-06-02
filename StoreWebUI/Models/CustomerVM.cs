using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreWebUI.Models
{
    public class CustomerVM
    {
        /// <summary>
		/// Customer View Model
		/// </summary>
        public CustomerVM()
        {


        }

        public CustomerVM(Customer customer)
        {
            CustomerID = customer.CustomerID;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            PhoneNumber = customer.PhoneNumber;
            Email = customer.Email;
        }


        public int CustomerID { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }




        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }


        [Required]
        [DisplayName("E-Mail")]
        public string Email { get; set; }

    }
}