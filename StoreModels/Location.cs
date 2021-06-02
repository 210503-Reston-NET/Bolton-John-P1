using System.Collections.Generic;
namespace StoreModels
{
    /// <summary>
    /// Location Model
    /// </summary>
    public class Location
    {
        public Location(string storeName, string address, string city, string state)
        {
            this.StoreName = storeName;
            this.Address = address;
            this.City = city;
            this.State = state;
        }

        public Location()
        {

        }

        public Location(int locationId, string storeName, string address, string city, string state) : this(storeName, address, city, state)
        {
            this.LocationID = locationId;
        }
        public string City { get; set; }
        public string State { get; set; }
        public int LocationID { get; set; }
        public string Address { get; set; }
        public string StoreName { get; set; }
        public List<Order> Orders { get; set; }
        public List<Inventory> Inventories { get; set; }


        public override string ToString()
        {
            return $"Name: {StoreName} \nAdress: {Address} \nCity: {City} \nState: {State}";
        }

        public bool Equals(Location location)
        {
            return this.StoreName.Equals(location.StoreName);
        }
    }
}