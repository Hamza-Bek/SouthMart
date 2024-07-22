using Domain.Models.Authentication;
using Domain.Models.ProductEntity;
using Domain.Models.SellerEntity;
using Domain.Models.UserEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<SellerAccount> SellerAccounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);     

            builder.Entity<ApplicationUser>()
            .HasOne(a => a.SellerAccount)
            .WithOne(b => b.Seller)
            .HasForeignKey<SellerAccount>(b => b.SellerId);
            //
            builder.Entity<ApplicationUser>()
                .HasOne(a => a.Cart)
                .WithOne(c => c.CartOwner)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Location>()
                .HasOne(l => l.LocationUser)
                .WithOne()
                .HasForeignKey<Location>(l => l.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);
        

            builder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
                .HasOne(o => o.Location)
                .WithMany(l => l.Orders)
                .HasForeignKey(o => o.LocationId);

            builder.Entity<OrderDetails>()
                .HasKey(od => od.OrderDetailId);

            builder.Entity<ApplicationUser>()
                  .HasMany(au => au.Notifications)
                  .WithOne(n => n.User)
                  .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.NoAction);


            //
            builder.Entity<ApplicationUser>()
            .HasMany(au => au.Products)
            .WithOne(p => p.Seller)
            .HasForeignKey(p => p.SellerId);

            builder.Entity<Product>()
                 .HasKey(p => p.Id);

            builder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(au => au.Products)
                .HasForeignKey(p => p.SellerId);


            builder.Entity<Comment>()
                .HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                     .HasForeignKey(c => c.UserId)
                        .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
