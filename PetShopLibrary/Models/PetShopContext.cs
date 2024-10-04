using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PetShopLibrary.Models;

public partial class PetShopContext : DbContext
{
    public PetShopContext()
    {
    }

    public PetShopContext(DbContextOptions<PetShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductOrder> ProductOrders { get; set; }

    public virtual DbSet<ProductOrderDetail> ProductOrderDetails { get; set; }

    public virtual DbSet<ServiceSchedule> ServiceSchedules { get; set; }

    public virtual DbSet<ShopService> ShopServices { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
		var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

		optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnect"));
	}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED20AA969B");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__2E1BDC42");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__19093A2BAFAF2E98");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__ProductO__C3905BAF9FBCF325");

            entity.ToTable("ProductOrder");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ProductOrders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProductOr__UserI__30F848ED");
        });

        modelBuilder.Entity<ProductOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__ProductO__D3B9D30C2815AA98");

            entity.ToTable("ProductOrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.ProductOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__ProductOr__Order__33D4B598");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductOr__Produ__34C8D9D1");
        });

        modelBuilder.Entity<ServiceSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__ServiceS__9C8A5B69EB7725F2");

            entity.ToTable("ServiceSchedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceSchedules)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServiceSc__Servi__29572725");

            entity.HasOne(d => d.User).WithMany(p => p.ServiceSchedules)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ServiceSc__UserI__286302EC");
        });

        modelBuilder.Entity<ShopService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__ShopServ__C51BB0EAB047049E");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4BB2D8B1C2");

            entity.ToTable("Transaction");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__37A5467C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC2D51969B");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RoleId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RoleID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
