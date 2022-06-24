using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.DTO;

namespace Models.DAL
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

            builder.Entity<CartProduct>(entity =>
            {
                entity.HasIndex(cartProduct => new { cartProduct.CartID, cartProduct.ProductID })
                      .IsUnique();
            });

            builder.Entity<ImportBillDetail>(entity =>
            {
                entity.HasOne(detail => detail.ImportBill)
                      .WithMany(bill => bill.ImportBillDetails)
                      .HasForeignKey(detail => detail.ImportBillID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bill => bill.Product)
                      .WithMany(detail => detail.ImportBillDetails)
                      .HasForeignKey(detail => detail.ProductID)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(x => new { x.ProductID, x.ImportBillID })
                      .IsUnique();
            });

            builder.Entity<ProductOrder>(entity =>
            {
                entity.HasOne(productOrder => productOrder.Product)
                      .WithMany(product => product.ProductOrder)
                      .HasForeignKey(productOrder => productOrder.ProductID)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(productOrder => productOrder.Order)
                      .WithMany(order => order.ProductOrder)
                      .HasForeignKey(productOrder => productOrder.OrderID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(productOrder => new { productOrder.ProductID, productOrder.OrderID })
                      .IsUnique();
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasOne(order => order.Voucher)
                      .WithMany(voucher => voucher.Orders)
                      .HasForeignKey(order => order.VoucherID)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasKey(comment => comment.ID);

                entity.HasOne(comment => comment.ProductOrder)
                      .WithOne(productOrder => productOrder.Comment)
                      .HasForeignKey<Comment>(comment => comment.ID)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ImportBill> ImportBills { get; set; }
        public DbSet<ImportBillDetail> ImportBillDetails { get; set; }
        public DbSet<ProductOrder> ProductOrder { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProduct { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<StateOrder> StateOrder { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}