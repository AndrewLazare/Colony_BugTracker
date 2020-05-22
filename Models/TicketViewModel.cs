using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Models
{
    public class TicketViewModel
    {
       public Ticket Ticket { get; set; }

        public List<ApplicationUser> Developers { get; set; }
        public List<ApplicationUser> Submitters { get; set; }
    }
}