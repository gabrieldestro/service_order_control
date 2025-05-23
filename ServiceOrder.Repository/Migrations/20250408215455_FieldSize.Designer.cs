﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiceOrder.Repository.Context;

#nullable disable

namespace ServiceOrder.Repository.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250408215455_FieldSize")]
    partial class FieldSize
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.12");

            modelBuilder.Entity("ServiceOrder.Domain.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cnpj")
                        .HasMaxLength(18)
                        .HasColumnType("TEXT");

                    b.Property<string>("Cpf")
                        .HasMaxLength(14)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(3452));

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ServiceOrder.Domain.Entities.ElectricCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cnpj")
                        .HasMaxLength(18)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(3853));

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ElectricCompanies");
                });

            modelBuilder.Entity("ServiceOrder.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(2992));

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DocumentReceivedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DocumentSentDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ElectricCompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<int>("FinalClientId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("FinalizationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("InspectionRequestDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderName")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ProjectApprovalDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ProjectRegistrationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ProjectSubmissionDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("ProjectValue")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReceivedDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ElectricCompanyId");

                    b.HasIndex("FinalClientId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ServiceOrder.Domain.Entities.OrderDeadline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValue(new DateTime(2025, 4, 8, 18, 54, 55, 668, DateTimeKind.Local).AddTicks(4221));

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<int?>("DocumentReceivedDays")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DocumentSentDays")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<int?>("FinalizationDays")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("InspectionRequestDays")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderId")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<int?>("PaymentDays")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProjectApprovalDays")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProjectRegistrationDays")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProjectSubmissionDays")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReceivedDays")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("OrderDeadlines");
                });

            modelBuilder.Entity("ServiceOrder.Domain.Entities.Order", b =>
                {
                    b.HasOne("ServiceOrder.Domain.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceOrder.Domain.Entities.ElectricCompany", "ElectricCompany")
                        .WithMany()
                        .HasForeignKey("ElectricCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServiceOrder.Domain.Entities.Client", "FinalClient")
                        .WithMany()
                        .HasForeignKey("FinalClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("ElectricCompany");

                    b.Navigation("FinalClient");
                });
#pragma warning restore 612, 618
        }
    }
}
