using COLONY_THE_BUGTRACKER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COLONY_THE_BUGTRACKER.Models { 
public class TicketComment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
   [AllowHtml]
    public string Body { get; set; }
    public string OwnerId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CommentAttachment { get; set; }

    public virtual Ticket Tickets { get; set; }
    public virtual ApplicationUser Owner { get; set; }
}
}