using COLONY_THE_BUGTRACKER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Models
{
    public class TicketPriority
    {
        public TicketPriority() //Making sure the tickets are not null, but empty.  So it wont error out
        {
            Tickets = new HashSet<Ticket>();

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Property { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}