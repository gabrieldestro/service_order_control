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
        public DbSet<Domain.Entities.OrderDeadline> OrderDeadlines { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).ValueGeneratedOnAdd();
                entity.Property(o => o.Enabled).HasDefaultValue(true);
                entity.Property(o => o.CreatedDate).HasDefaultValue(DateTime.Now);

                entity.Property(e => e.ReceivedDate).HasColumnType("date");
                entity.Property(e => e.DocumentSentDate).HasColumnType("date");
                entity.Property(e => e.DocumentReceivedDate).HasColumnType("date");
                entity.Property(e => e.ProjectRegistrationDate).HasColumnType("date");
                entity.Property(e => e.ProjectSubmissionDate).HasColumnType("date");
                entity.Property(e => e.ProjectApprovalDate).HasColumnType("date");
                entity.Property(e => e.InspectionRequestDate).HasColumnType("date");
                entity.Property(e => e.FinalizationDate).HasColumnType("date");
                entity.Property(e => e.PaymentDate).HasColumnType("date");

                entity.HasOne(o => o.Client)
                    .WithMany()
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(o => o.ElectricCompany)
                        .WithMany()
                        .HasForeignKey("ElectricCompanyId")
                        .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(o => o.FinalClient)
                        .WithMany()
                        .HasForeignKey("FinalClientId")
                        .OnDelete(DeleteBehavior.ClientSetNull);
            });

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

            // Configuração de OrderDeadline
            modelBuilder.Entity<OrderDeadline>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<OrderDeadline>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderDeadline>()
                .Property(o => o.Enabled)
                .HasDefaultValue(true);

            modelBuilder.Entity<OrderDeadline>()
                .Property(o => o.CreatedDate)
                .HasDefaultValue(DateTime.Now);
        }
    }
}
