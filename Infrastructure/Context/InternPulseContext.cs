using Google;
using InternPulse4.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
namespace InternPulse4.Infrastructure.Context
{
    public class InternPulseContext : DbContext
    {
        public InternPulseContext(DbContextOptions<InternPulseContext> opt) : base(opt)
        {

        }
        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()) 
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                    }
                }
            }

            modelBuilder.Entity<User>().Property<int>("Id").ValueGeneratedOnAdd();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                DateCreated = DateTime.UtcNow,
                FirstName = "Hasbiy",
                LastName = "Oyebo",
                IsDeleted = false,
                Email = "oyebohm@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("hasbiyallah"),
                Role = Core.Domain.Enums.Role.Admin,
                CreatedBy = "ManualRegistration",
            });
        }
    }
}
