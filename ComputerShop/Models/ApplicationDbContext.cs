using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ComputerShop.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, long, UserLogin, UserRole, UserClaim>
    {
        public ApplicationDbContext()
            : base("CSConnection")
        {
            
           
        }
       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Basket> Basket { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            // Map Entities to their tables.
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<UserRole>().ToTable("RoleAsingment");
            modelBuilder.Entity<UserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<UserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<Purchase>().ToTable("Purchase");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Basket>().HasKey(b => new { b.PurchaseId, b.ProductId }).ToTable("Basket");
            modelBuilder.Entity<Basket>()
            .HasRequired(b => b.Purchase)
            .WithMany(t => t.Baskets)
            .HasForeignKey(t => t.PurchaseId);
            modelBuilder.Entity<Basket>()
           .HasRequired(b => b.Product)
           .WithMany(t => t.Baskets)
           .HasForeignKey(t => t.ProductId);


            // Set AutoIncrement-Properties
            modelBuilder.Entity<Address>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<UserClaim>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Role>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Product>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Purchase>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            
        }
    }
}