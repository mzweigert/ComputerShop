using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ComputerShop.Managers;

namespace ComputerShop.Models
{
    public partial class Address
    {
        public long Id { get; set; }

        [Required]
        [RegularExpression("^([a-zA-Z0-9 .&'-])", ErrorMessage = "Invalid street name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Street { get; set; }
        [Required]
        [RegularExpression("^([a-zA-Z -])", ErrorMessage = "Invalid city name.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string City { get; set; }
        [Required]
        [RegularExpression("^([a-zA-Z0-9/])", ErrorMessage = "Invalid number house.")]
        [Display(Name = "Zip code")]
        public string Number { get; set; }
        [Required]
        [RegularExpression("^([0-9-])", ErrorMessage = "Invalid zip-code.")]
        [Display(Name = "Zip code")]
        [StringLength(6, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string ZipCode { get; set; }
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

        [Required]
        [Display(Name = "Product name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string  ProductName { get; set; }

        [Required]
        [Display(Name = "Product type")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string ProductType { get; set; }

        [Required(ErrorMessage = "Please add a price.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Production date")]
        [DataType(DataType.Date, ErrorMessage = "Incorrect date format")]
        
        public System.DateTime ProductionDate { get; set; }

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
        public System.DateTime PurchaseDate { get; set; }
        public bool IsConfirmed { get; set; }

        public virtual ICollection<Basket> Baskets { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
    public partial class Basket
    {
        [Key]
        public long PurchaseId { get; set; }
        [Key]
        public long ProductId { get; set; }
        public long Quantity { get; set; }

        public virtual Purchase Purchase { get; set; }
        public virtual Product Product { get; set; }
    }

    
}