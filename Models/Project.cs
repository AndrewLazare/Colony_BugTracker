using COLONY_THE_BUGTRACKER.Models;
using COLONY_THE_BUGTRACKER.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COLONY_THE_BUGTRACKER.Models
{
    public class Project
    {
        private RoleHelper roleHelper = new RoleHelper();
        public Project() //Making sure the tickets are not null, but empty.  So it wont error out
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }



        public virtual ICollection<Ticket> Tickets { get; set; } //All the children of tickets will go here. A variable to store it in.
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public ICollection<ApplicationUser> GetManagersOnProject()
        {
            var list = Users.Where(u => roleHelper.ListUserRoles(u.Id).FirstOrDefault() == "Manager").ToList();
            return list;
        }


    }
}
