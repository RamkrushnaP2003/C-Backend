// Modeds/Person.cs

namespace EFCoreManyToMany.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }

        // Navigation property for the related addresses
        public ICollection<PersonAddress> PersonAddresses { get; set; } = new List<PersonAddress>();
    }
}
