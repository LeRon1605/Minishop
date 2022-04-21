﻿using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class ShopOnlineDbContext: DbContext
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ShopOnline"].ConnectionString;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasOne(product => product.Category)
                      .WithMany(category => category.Products)
                      .HasForeignKey("CategoryID")
                      .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<User>(entity =>
            {
                entity.HasOne(user => user.Role)
                      .WithMany(role => role.Users)
                      .HasForeignKey("RoleID")
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        // public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductOrder> ProductOrder { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<ProductVoucher> ProductVoucher { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProduct { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<StateOrder> StateOrder { get; set; }
    }
}