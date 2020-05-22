using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

using COLONY_THE_BUGTRACKER.Models;
using COLONY_THE_BUGTRACKER.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace COLONY_THE_BUGTRACKER.Models

{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public ApplicationUser()
        {
            TicketComments = new HashSet<TicketComment>();
            TicketAttachments = new HashSet<TicketAttachment>();
           /* Notifications = new HashSet<TicketNotifications>();*/ // you need one of these everytime you have an ICollection
            Projects = new HashSet<Project>();
            History = new HashSet<TicketHistory>();
        }

        private RoleHelper roleHelper = new RoleHelper();


        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        public string LastName { get; set; }
      

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}"; //this is saying for the string FullName return FIrstName and LastName by using string interpolation
            }
        }
        [NotMapped]
        public string CustomDisplay
        { 
            get
            {
                return $"{Email} => {FullName}"; //this is saying for the string FullName return FIrstName and LastName by using string interpolation
            }
        }

        public string AvatarPath { get; set; }


        public virtual ICollection<TicketHistory> History { get; set; } //  function is a member function in the base class that you redefine in a derived class. It is declared using the virtual keyword. It is used to tell the compiler to perform dynamic linkage or late binding on the function. 
        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        //public virtual ICollection<TicketNotifications> Notifications { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketPriority> Priorities { get; set; }
        public DbSet<TicketStatus> Statuses { get; set; }
        public DbSet<TicketAttachment> Attachments { get; set; }
        public DbSet<TicketComment> Comments { get; set; }
        public DbSet<TicketHistory> Histories { get; set; }
        public DbSet<TicketType> Types { get; set; }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<COLONY_THE_BUGTRACKER.Models.TicketNotifications> TicketNotifications { get; set; }


        //public System.Data.Entity.DbSet<COLONY_THE_BUGTRACKER.Models.ApplicationUser> Users { get; set; }
    }
}