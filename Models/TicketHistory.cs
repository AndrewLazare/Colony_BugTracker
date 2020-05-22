using COLONY_THE_BUGTRACKER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public int TicketId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UpdaterId { get; set; }
        public string Property { get; set; }


        public Ticket Ticket { get; set; }
        public ApplicationUser Updater { get; set; }

    }
}