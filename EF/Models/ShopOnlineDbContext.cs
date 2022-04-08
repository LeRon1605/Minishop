using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EF.Models
{
    public class ShopOnlineDbContext: DbContext
    {
        private string connectionString = "Server=localhost,1433;Database=ShopOnline;UID=sa;PWD=ronle75";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
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