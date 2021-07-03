namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string firstname, string lastname, string street, 
                        string city, string state, string zipCode)
        {
            Firstname = firstname;
            Lastname = lastname;
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }        
    }
}