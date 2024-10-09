using System.Collections.Generic;

namespace PersonOneToMany.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? PersonEmail { get; set; }
        public string? PersonContact { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>(); // Initialize the list

        public Person() { } // Default constructor
    }
}
