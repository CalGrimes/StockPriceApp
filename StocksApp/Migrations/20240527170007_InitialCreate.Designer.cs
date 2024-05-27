﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StocksApp.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    [Migration("20240527170007_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("orders")
                .HasAnnotation("ProductVersion", "7.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ServiceContracts.DTO.BuyOrder", b =>
                {
                    b.Property<Guid>("BuyOrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateAndTimeOfOrder")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<string>("StockName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StockSymbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("BuyOrderID");

                    b.ToTable("BuyOrders", "orders");
                });

            modelBuilder.Entity("ServiceContracts.DTO.SellOrder", b =>
                {
                    b.Property<Guid>("SellOrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateAndTimeOfOrder")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.Property<string>("StockName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StockSymbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SellOrderID");

                    b.ToTable("SellOrders", "orders");
                });
#pragma warning restore 612, 618
        }
    }
}
