﻿using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Database;

public class NorthwindContext : DbContext
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetails> OrderDetails { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // database configuration
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder
            .LogTo(Console.Out.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql($"host=localhost;db=northwind;uid=postgres;pwd=2002");
    }

    //creating models/tables
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // category table
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Category>()
            .Property(x => x.Id).HasColumnName("categoryid");
        modelBuilder.Entity<Category>()
            .Property(x => x.Name).HasColumnName("categoryname");
        modelBuilder.Entity<Category>()
            .Property(x => x.Description).HasColumnName("description");

        // product table
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Product>()
            .HasKey(p => new { p.CategoryId });
        modelBuilder.Entity<Product>()
            .Property(x => x.Id).HasColumnName("productid");
        modelBuilder.Entity<Product>()
            .Property(x => x.Name).HasColumnName("productname");
        modelBuilder.Entity<Product>()
            .Property(x => x.CategoryId).HasColumnName("categoryid");
        modelBuilder.Entity<Product>()
            .Property(x => x.UnitPrice).HasColumnName("unitprice");
        modelBuilder.Entity<Product>()
            .Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
        modelBuilder.Entity<Product>()
            .Property(x => x.UnitsInStock).HasColumnName("unitinstock");

        // order table
        modelBuilder.Entity<Order>().ToTable("orders");
        modelBuilder.Entity<Order>()
            .Property(x => x.Id).HasColumnName("orderid");
        modelBuilder.Entity<Order>()
            .Property(x => x.Date).HasColumnName("date");
        modelBuilder.Entity<Order>()
            .Property(x => x.Required).HasColumnName("requireddate");
        modelBuilder.Entity<Order>()
            .Property(x => x.Shipped).HasColumnName("shipped");
        modelBuilder.Entity<Order>()
            .Property(x => x.ShipName).HasColumnName("shipname");
        modelBuilder.Entity<Order>()
            .Property(x => x.ShipCity).HasColumnName("shipcity");


        //order details table
        modelBuilder.Entity<OrderDetails>().ToTable("orderdetails");
        modelBuilder.Entity<OrderDetails>()
            .HasKey(od => new { od.ProductId, od.OrderId }); // foreign key, od stands for order details table
        modelBuilder.Entity<OrderDetails>()
            .Property(x => x.ProductId).HasColumnName("productid");
        modelBuilder.Entity<OrderDetails>()
            .Property(x => x.OrderId).HasColumnName("orderid");
        modelBuilder.Entity<OrderDetails>()
            .Property(x => x.UnitPrice).HasColumnName("unitprice");
        modelBuilder.Entity<OrderDetails>()
            .Property(x => x.Quantity).HasColumnName("quantity");
        modelBuilder.Entity<OrderDetails>()
            .Property(x => x.Discount).HasColumnName("discount");

    }
}

