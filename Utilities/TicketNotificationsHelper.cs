using COLONY_THE_BUGTRACKER.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Utilities
{
    /// <summary>
    /// ApplicationDbContext in a static method.  It itself has to be static.
    /// </summary>
    public class TicketNotificationsHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        RoleHelper roleHelper = new RoleHelper();
        public void SendTicketNotification(int ticketId)
        {
            var ticket = db.Tickets.Find(ticketId);

            var users = new List<ApplicationUser>();
            var assignedDev = db.Users.Find(ticket.AssignedToUserId);
            if (assignedDev != null)
            {
                users.Add(assignedDev);
            }
           
            users.Add(ticket.Owner);
           
            //List<string> emails = new List<string>();
            foreach (var user in users)
            {
                //emails.Add(user.Email);
                if (ticket.Priority != null)
                {

                    db.TicketNotifications.Add(new TicketNotifications()
                    {
                        SenderId = HttpContext.Current.User.Identity.GetUserId(),
                        Title = ticket.Title,
                        NotificationBody = $"The Priority of ticket {ticket.Title } has been changed to {ticket.Priority.Name}",
                        TIcketId = ticketId,
                        RecipientID = user.Id,
                        CreatedDate = DateTime.Now
                    });
                }
                if (ticket.Status != null)
                {

                    db.TicketNotifications.Add(new TicketNotifications()
                    {
                        SenderId = HttpContext.Current.User.Identity.GetUserId(),
                        Title = ticket.Title,
                        NotificationBody = $"The Status of ticket {ticket.Title } has been changed to {ticket.Status.Name}",
                        TIcketId = ticketId,
                        RecipientID = user.Id,
                        CreatedDate = DateTime.Now
                    });
                }
                if (ticket.Type != null)
                {

                    db.TicketNotifications.Add(new TicketNotifications()
                    {
                        SenderId = HttpContext.Current.User.Identity.GetUserId(),
                        Title = ticket.Title,
                        NotificationBody = $"The Type of ticket {ticket.Title } has been changed to {ticket.Type.Name}",
                        TIcketId = ticketId,
                        RecipientID = user.Id,
                        CreatedDate = DateTime.Now
                    });
                }
                if (assignedDev != null)
                {

                    db.TicketNotifications.Add(new TicketNotifications()
                    {
                        SenderId = HttpContext.Current.User.Identity.GetUserId(),
                        Title = ticket.Title,
                        NotificationBody = $"You have been assigned to {ticket.Title}",
                        TIcketId = ticketId,
                        RecipientID = user.Id,
                        CreatedDate = DateTime.Now
                    });
                }
                db.SaveChanges();



            }
        }


        /// <summary>
        /// THis is going to determine who I am (currentUserId) 
        /// Returning all of the notifications where the RecipientID is ME.  
        /// not all of them...but ones that havent been read..!t.IsRead.
        /// </summary>
        /// <returns></returns>
        public static List<TicketNotifications> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(currentUserId);
            return db.TicketNotifications.Include("Sender").Include("Recipient").Where(t => t.RecipientID == currentUserId && !t.IsRead).ToList();

        }



    }


}
