using COLONY_THE_BUGTRACKER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COLONY_THE_BUGTRACKER.Models
{
    public class Ticket
    {
        public Ticket() //Making sure the tickets are not null, but empty.  So it wont error out
        {
            Comments = new HashSet<TicketComment>();
            Attachments = new HashSet<TicketAttachment>();
            History = new HashSet<TicketHistory>();
         
        }
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Summary { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } //nullable because it wont have value untill updated
        public string OwnerId { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }
        public string AssignedToUserId { get; set; }

        public virtual Project Project { get; set; }
        public virtual TicketType Type { get; set; }
        public virtual TicketStatus Status { get; set; }
        public virtual TicketPriority Priority { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketHistory> History { get; set; }// virtual give the child the ability to override its parent
        


    }
}