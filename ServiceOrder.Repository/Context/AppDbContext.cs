using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.Repository.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Domain.Entities.Order> Orders { get; set; }
        public DbSet<Domain.Entities.Client> Clients { get; set; }
        public DbSet<Domain.Entities.ElectricCompany> ElectricCompanies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de Order
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
                .Property(o => o.Enabled)
                .HasDefaultValue(true);

            modelBuilder.Entity<Order>()
                .Property(o => o.CreatedDate)
                .HasDefaultValue(DateTime.Now);

            // Configuração de Client
            modelBuilder.Entity<Client>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Client>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Client>()
                .Property(o => o.Enabled)
                .HasDefaultValue(true);

            modelBuilder.Entity<Client>()
                .Property(o => o.CreatedDate)
                .HasDefaultValue(DateTime.Now);

            // Configuração de ElectricCompany
            modelBuilder.Entity<ElectricCompany>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<ElectricCompany>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ElectricCompany>()
                .Property(o => o.Enabled)
                .HasDefaultValue(true);

            modelBuilder.Entity<ElectricCompany>()
                .Property(o => o.CreatedDate)
                .HasDefaultValue(DateTime.Now);
        }
    }
}
