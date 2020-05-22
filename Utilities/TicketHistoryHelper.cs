using COLONY_THE_BUGTRACKER.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Utilities
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// This a helper to compare a ticket before and after adding a user or editing the ticket.
        /// If the data enters one of the if statement is creates the variable, runs through the statement and saves at the data base at the end.
        /// </summary>
        /// <param name="oldTicket">this is the the previous information in the db concerning this ticket</param>
        /// <param name="newTicket">this compares the sae information after the edit to see what was changed</param>
        public void RecordHistoricalChanges(Ticket oldTicket, Ticket newTicket) 
        {

            
            if(oldTicket.StatusId != newTicket.StatusId) 
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "Status",
                    OldValue = oldTicket.StatusId.ToString(),
                    NewValue = newTicket.StatusId.ToString(),
                    UpdateDate = DateTime.Now,
                    TicketId = newTicket.Id,
                    UpdaterId = HttpContext.Current.User.Identity.GetUserId()
                  
            };

                db.Histories.Add(newHistoryRecord);
            }
            if (oldTicket.PriorityId != newTicket.PriorityId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "Priority",
                    OldValue = oldTicket.PriorityId.ToString(),
                    NewValue = newTicket.PriorityId.ToString(),
                    UpdateDate = DateTime.Now,
                    TicketId = newTicket.Id,
                    UpdaterId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.Histories.Add(newHistoryRecord);
            }
            if (oldTicket.TypeId != newTicket.TypeId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "Type",
                    OldValue = oldTicket.TypeId.ToString(),
                    NewValue = newTicket.TypeId.ToString(),
                    UpdateDate = DateTime.Now,
                    TicketId = newTicket.Id,
                    UpdaterId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.Histories.Add(newHistoryRecord);
            }
            if (oldTicket.Summary != newTicket.Summary)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "Summary",
                    OldValue = oldTicket.Summary,
                    NewValue = newTicket.Summary,
                    UpdateDate = DateTime.Now,
                    TicketId = newTicket.Id,
                    UpdaterId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.Histories.Add(newHistoryRecord);
            }
            if (oldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "AssignedToUserId",
                    OldValue = oldTicket.AssignedToUser == null ? "UnAssignedFullName": oldTicket.AssignedToUser.FullName,
                    NewValue = newTicket.AssignedToUser == null ? "UnAssignedFullName" : newTicket.AssignedToUser.FullName,
                    UpdateDate = DateTime.Now,
                    TicketId = newTicket.Id,
                    UpdaterId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.Histories.Add(newHistoryRecord);
            }
            db.SaveChanges();  
           







        }


       
    }
}

 