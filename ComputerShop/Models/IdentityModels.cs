using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComputerShop.Models
{
    public partial class Address
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        public string Zip_code { get; set; }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<long, UserLogin, UserRole, UserClaim>
    {
        public Nullable<long> AddressId { get; set; }

        public virtual Address Address { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager userManager)
        {
            var userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class UserLogin : IdentityUserLogin<long> { }

    public class UserRole : IdentityUserRole<long> { }

    public class Role : IdentityRole<long, UserRole> { }

    public class UserClaim : IdentityUserClaim<long> { }

    public partial class Product
    {
        public Product()
        {
            this.Baskets = new HashSet<Basket>();
        }

        public long Id { get; set; }
        public string product_type { get; set; }
        public System.DateTime production_date { get; set; }

        public virtual ICollection<Basket> Baskets { get; set; }
    }
    public partial class Purchase
    {
        public Purchase()
        {
            this.Baskets = new HashSet<Basket>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public System.DateTime purchase_date { get; set; }

        public virtual ICollection<Basket> Baskets { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public partial class Basket
    {
        [Key]
        public long PurchaseId { get; set; }
        [Key]
        public long ProductId { get; set; }
        public long quantity { get; set; }

        public virtual Purchase Purchase { get; set; }
        public virtual Product Product { get; set; }
    }

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
            // Override some column mappings that do not match our default

        }
        
    

    }
}