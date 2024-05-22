using CQRSWithDocker_Identity.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CQRSWithDocker_Identity.Data
{
    public class DataContext:IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }

        public DbSet<Users> User { get; set; }
        public DbSet<Loggers> Logger { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().HasKey(p => p.Id);
            modelBuilder.Entity<Loggers>().HasKey(p=>p.Id);
            modelBuilder.Entity<Users>().HasData(
                new Users("Vikas", "Dev", 999),
                new Users("Avdesh", "IT", 1899)
              
            );
        }
        
    }
}
