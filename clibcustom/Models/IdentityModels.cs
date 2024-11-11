

using AngularJSAuthentication.API.Custom;
using AngularJSAuthentication.API.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
 

namespace AngularJSAuthentication.API.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Ruc { get; set; }

        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [NotMapped]
        public string Password { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here

            

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("AuthContext")
        {
            this.Configuration.LazyLoadingEnabled = false;

        }


        public DbSet<Client> Clients { get; set; }

        public DbSet<Menu> Menues { get; set; }

        public DbSet<Submenu> Submenues { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}