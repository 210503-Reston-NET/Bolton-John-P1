using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreWebUI.Models
{

    public class LocationVM
    {

        public LocationVM(Location location)
        {
            LocationID = location.LocationID;
            StoreName = location.StoreName;
            City = location.City;
            State = location.State;
            Address = location.Address;
        }

        public LocationVM()
        {
        }


        public int LocationID { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [DisplayName("State or Province")]
        public string State { get; set; }

        [Required]
        [DisplayName("Store Name")]
        public string StoreName { get; set; }
    }
}
