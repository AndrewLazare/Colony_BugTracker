using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Models
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public DateTime CreatedDate { get; set;}
        public List<ApplicationUser> Developers { get; set; }
        public List<ApplicationUser> Submitters { get; set; }
    }
}