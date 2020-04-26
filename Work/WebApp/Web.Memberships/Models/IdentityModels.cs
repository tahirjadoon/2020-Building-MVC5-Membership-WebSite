using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Web.Memberships.Entities;

namespace Web.Memberships.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }

        [DefaultValue(false)]
        [Required]
        public bool IsActive { get; set; }

        [DefaultValue(typeof(DateTime), "")]
        [Required]
        public DateTime RegistrationDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //We need to add property for each class to create the table for as DbSet
        //also the property name will be used to query the table using linq. 

        //Items
        public DbSet<Section> Sections { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Item> Items { get; set; }

        //Products
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductLinkText> ProductLinkTexts { get; set; }

        //Subscription
        public DbSet<Subscription> Subscriptions { get; set; }

        //ProductItem
        public DbSet<ProductItem> ProductItems { get; set; }

        //SubscriptionProduct
        public DbSet<SubscriptionProduct> SubscriptionProducts { get; set; }

        //UserSubscription
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
    }
}