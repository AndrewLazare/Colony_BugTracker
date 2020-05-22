using COLONY_THE_BUGTRACKER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Models
{
    public class TicketNotifications
    {
        public int Id { get; set; }
        public int TIcketId { get; set; }
        public int ProjectId { get; set; }
        public string NotificationBody { get; set; }
        public DateTime CreatedDate { get; set; }
        public string SenderId { get; set; }
        public string RecipientID { get; set; }
        public string Title { get; set; }
        public bool IsRead { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
        public virtual ApplicationUser Sender { get; set; }

    }
}