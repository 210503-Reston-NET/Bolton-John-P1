using System.Collections.Generic;
namespace StoreModels
{
    /// <summary>
    /// Customer Model
    /// </summary>
    public class Customer
    {
        public Customer(string firstName, string lastName, string phoneNumber, string email)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
        }

        public Customer()
        {

        }

        public Customer(int customerId, string firstName, string lastName, string phoneNumber, string email) : this(firstName, lastName, phoneNumber, email)
        {
            this.CustomerID = customerId;
        }

        public int CustomerID { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public List<Order> Orders { get; set; }

        public override string ToString()
        {
            return $"Name: {FirstName} {LastName} \nPhone Number: {PhoneNumber} \nEmail: {Email} \n";
        }

        public bool Equals(Customer customer)
        {
            return this.FirstName.Equals(customer.FirstName) && this.LastName.Equals(customer.LastName);
        }
    }
}