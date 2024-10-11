// Models/Address.cs

namespace EFCoreManyToMany.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        // Navigation property to reference the person addresses
        public ICollection<PersonAddress> PersonAddresses { get; set; } = new List<PersonAddress>();
    }
}
