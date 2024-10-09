namespace PersonOneToMany.Models
{
    public class Address
    {
        public string Street { get; set; } = string.Empty; // Initialize with an empty string
        public string City { get; set; } = string.Empty;   // Initialize with an empty string
        public string ZipCode { get; set; } = string.Empty; // Initialize with an empty string

        public Address() { } // Default constructor
    }
}
