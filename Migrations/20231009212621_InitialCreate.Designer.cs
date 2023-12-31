﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HHPW_SB_BE.Migrations
{
    [DbContext(typeof(HHPWDbContext))]
    [Migration("20231009212621_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HHPW_SB_BE.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Uid")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Sean Bryant"
                        });
                });

            modelBuilder.Entity("HHPW_SB_BE.Models.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("MenuItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Pizza",
                            Description = "Delicious pizza",
                            ImageUrl = "image_url_here",
                            Name = "Pepperoni Pizza",
                            Price = 12.99m
                        },
                        new
                        {
                            Id = 2,
                            Category = "Wings",
                            Description = "Crunchy chicken wings",
                            ImageUrl = "image_url_here",
                            Name = "Spicy Chicken Wings",
                            Price = 9.99m
                        });
                });

            modelBuilder.Entity("HHPW_SB_BE.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CustomerPhone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateClosed")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DatePlaced")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<string>("OrderName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("OrderPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OrderType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Review")
                        .HasColumnType("integer");

                    b.Property<decimal>("Tip")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalOrderAmount")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CustomerEmail = "john@example.com",
                            CustomerName = "John Doe",
                            CustomerPhone = "123-456-7890",
                            DateClosed = new DateTime(2023, 10, 10, 17, 26, 21, 38, DateTimeKind.Local).AddTicks(8104),
                            DatePlaced = new DateTime(2023, 10, 9, 17, 26, 21, 38, DateTimeKind.Local).AddTicks(8066),
                            EmployeeId = 1,
                            OrderName = "First Order",
                            OrderPrice = 20.00m,
                            OrderStatus = "Closed",
                            OrderType = "In-Person",
                            PaymentType = "Credit",
                            Review = 4,
                            Tip = 5.00m,
                            TotalOrderAmount = 25.00m
                        });
                });

            modelBuilder.Entity("MenuItemOrder", b =>
                {
                    b.Property<int>("MenuItemsId")
                        .HasColumnType("integer");

                    b.Property<int>("OrdersId")
                        .HasColumnType("integer");

                    b.HasKey("MenuItemsId", "OrdersId");

                    b.HasIndex("OrdersId");

                    b.ToTable("OrderMenuItems", (string)null);
                });

            modelBuilder.Entity("HHPW_SB_BE.Models.Order", b =>
                {
                    b.HasOne("HHPW_SB_BE.Models.Employee", "Employee")
                        .WithMany("Orders")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("MenuItemOrder", b =>
                {
                    b.HasOne("HHPW_SB_BE.Models.MenuItem", null)
                        .WithMany()
                        .HasForeignKey("MenuItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HHPW_SB_BE.Models.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HHPW_SB_BE.Models.Employee", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
