// Data/ApplicationDbContext.cs

using EFCoreManyToMany.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreManyToMany.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PersonAddress> PersonAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonAddress>()
                .HasKey(pa => new { pa.PersonId, pa.AddressId });

            modelBuilder.Entity<PersonAddress>()
                .HasOne(pa => pa.Person)
                .WithMany(p => p.PersonAddresses)
                .HasForeignKey(pa => pa.PersonId);

            modelBuilder.Entity<PersonAddress>()
                .HasOne(pa => pa.Address)
                .WithMany(a => a.PersonAddresses)
                .HasForeignKey(pa => pa.AddressId);
        }
    }
}
