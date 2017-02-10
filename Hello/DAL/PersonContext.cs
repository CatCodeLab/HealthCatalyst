using Hello.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hello.DAL
{
    public class PersonContext : DbContext
    {

        public PersonContext() : base("PersonContext")
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}