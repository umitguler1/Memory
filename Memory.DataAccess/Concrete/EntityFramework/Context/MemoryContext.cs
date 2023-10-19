using Memory.DataAccess.Concrete.EntityFramework.Mappings;
using Memory.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Memory.DataAccess.Concrete.EntityFramework.Context
{
    public class MemoryContext : IdentityDbContext<AppIdentityUser,AppIdentityRole,int>
    {
        //public MemoryContext(DbContextOptions options):base(options)
        //{
                
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = DESKTOP-CHSJJ4J\\SQLEXPRESS;Initial Catalog=MemoryDb;Integrated Security=true;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CityMap());
            modelBuilder.ApplyConfiguration(new NotebookMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Notebook> Notebooks { get; set; }
    }
}
