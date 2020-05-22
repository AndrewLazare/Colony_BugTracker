using COLONY_THE_BUGTRACKER.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Utilities
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper(); // you cant use var at class level.  has to be method

        public bool IsOnTicket(string userId, int ticketId)
        {
            if (db.Tickets.Find(ticketId).Project.Users.Contains(db.Users.Find(userId)))
            {
                return true;

            }
            return false;


        }
        public bool IsUserOnTicket(string userId, int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);
            var flag = ticket.Project.Users.Any(u => u.Id == userId);
            return (flag);
        }
        public void AddUserToTicket(string userId, int ticketId)
        {
            if (!IsUserOnTicket(userId, ticketId))
            {
                var ticket = db.Tickets.Find(ticketId);
                ticket.Project.Users.Add(db.Users.Find(userId));
                db.Entry(ticket).State = EntityState.Modified; //saves the instance.
                db.SaveChanges();
            }
        }
        public void RemoveUserFromTicket(string userId, int ticketId)
        {
            if (IsUserOnTicket(userId, ticketId))
            {
                var ticket = db.Tickets.Find(ticketId);
                ticket.Project.Users.Remove(db.Users.Find(userId));
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

            }
        }
        public int SetDefaultTicketStatus() 
        {
            return db.Statuses.FirstOrDefault(ts => ts.Name == "Open").Id;
        }
        /// <summary>
        ///STEP 1 Determine my role *** myRole is the variable name *** roleHelper has a method named ListUserRole ****  HttpContext.Current.User.Identity this isnt coming in through controller.  
        ///  we get identity and user Id...once its evaluated it grabs the role of that user.  we only want one....so first and default
        ///STEP 2 use that role to build the appropriate set of tickets
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public ICollection<ApplicationUser> ListUsersOnTicket(int ticketId)
        {
            return db.Tickets.Find(ticketId).Project.Users.ToList();
        }
        public List<Ticket> ListMyTickets()
        {
            var myTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myRole = roleHelper.ListUserRoles(HttpContext.Current.User.Identity.GetUserId()).FirstOrDefault();

            switch (myRole) 
            {
                case "Admin":
                    myTickets.AddRange(db.Tickets);
                    break;
                case "Manager":
                    myTickets.AddRange(user.Projects.SelectMany(m => m.Tickets));
                    break;
                case "Developer":
                    myTickets.AddRange(db.Tickets.Where(t => t.AssignedToUserId == userId));
                    break;
                case "Submitter":
                    myTickets.AddRange(db.Tickets.Where(t => t.OwnerId == userId));
                    break;
                default:
                    break;
            
            }

            return myTickets;
        }


    }
}