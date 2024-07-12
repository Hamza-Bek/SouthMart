using Domain.Models.Authentication;
using Domain.Models.ProductEntity;
using Domain.Models.SellerEntity;
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
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<SellerAccount> SellerAccounts { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);     

            builder.Entity<ApplicationUser>()
            .HasOne(a => a.SellerAccount)
            .WithOne(b => b.Seller)
            .HasForeignKey<SellerAccount>(b => b.SellerId);

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
        }
    }
}
