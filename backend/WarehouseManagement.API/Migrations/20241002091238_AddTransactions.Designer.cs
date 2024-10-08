﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WarehouseManagement.API.Data;

#nullable disable

namespace WarehouseManagement.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241002091238_AddTransactions")]
    partial class AddTransactions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WarehouseManagement.API.Models.Domain.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Hazardous")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitSize")
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WarehouseManagement.API.Models.Domain.Transaction", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<Guid>("ProductId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SourceWHId")
                        .HasColumnType("int");

                    b.Property<string>("SourceWarehouse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TargetWHId")
                        .HasColumnType("int");

                    b.Property<Guid?>("TargetWarehouseWarehouseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TransactionId");

                    b.HasIndex("ProductId1");

                    b.HasIndex("TargetWarehouseWarehouseId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("WarehouseManagement.API.Models.Domain.Warehouse", b =>
                {
                    b.Property<Guid>("WarehouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrentCapacity")
                        .HasColumnType("int");

                    b.Property<int>("MaxCapacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zipcode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WarehouseId");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("WarehouseManagement.API.Models.Domain.Transaction", b =>
                {
                    b.HasOne("WarehouseManagement.API.Models.Domain.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WarehouseManagement.API.Models.Domain.Warehouse", "TargetWarehouse")
                        .WithMany()
                        .HasForeignKey("TargetWarehouseWarehouseId");

                    b.Navigation("Product");

                    b.Navigation("TargetWarehouse");
                });
#pragma warning restore 612, 618
        }
    }
}
