// Models/PersonAddress.cs

namespace EFCoreManyToMany.Models
{
    public class PersonAddress
    {
        public int PersonAddressId { get; set; }
        public int PersonId { get; set; }
        public int AddressId { get; set; }

        // Navigation properties
        public Person Person { get; set; }
        public Address Address { get; set; }
    }
}
