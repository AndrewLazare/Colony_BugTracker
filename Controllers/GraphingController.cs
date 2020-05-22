using COLONY_THE_BUGTRACKER.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COLONY_THE_BUGTRACKER.Controllers
{

    public class GraphingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Graphing
        public JsonResult ProduceChartData1()
        {
            var myData = new List<MorrisGraphData>();
            MorrisGraphData data = null;
            foreach (var priority in db.Priorities.ToList())
            {
                data = new MorrisGraphData();
                data.label = priority.Name;
                data.value = db.Tickets.Where(t => t.Priority.Name == priority.Name).Count();
                myData.Add(data);
            }

            return Json(myData);
        }
        public JsonResult ProduceChartData2()
        {
            var myData = new List<MorrisGraphData>();
            MorrisGraphData data = null;


            foreach (var status in db.Statuses.ToList())
            {
                data = new MorrisGraphData();
                data.label = status.Name;
                data.value = db.Tickets.Where(t => t.Status.Name == status.Name).Count();
                myData.Add(data);
            }

            return Json(myData);
        }
        public JsonResult ProduceBarData()
        {
            
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.ToList();
            var myData = new List<MorrisGraphData>();
            MorrisGraphData data = null;

            if (User.IsInRole("Admin") || User.IsInRole("Manager"))
            {
                foreach (var priority in db.Priorities.ToList())
                {
                    data = new MorrisGraphData();
                    data.label = priority.Name;
                    data.value = tickets.Where(t => t.Priority.Name == priority.Name).Count();
                    myData.Add(data);
                }
            }
            else if(User.IsInRole("Developer"))
            {
                foreach (var priority in db.Priorities.ToList())
                {
                    data = new MorrisGraphData();
                    data.label = priority.Name;
                    data.value = tickets.Where(t => t.AssignedToUserId == userId).Where(t => t.Priority.Name == priority.Name).Count();
                    myData.Add(data);
                }


            }
            else if (User.IsInRole("Submitter"))
            {
                foreach (var priority in db.Priorities.ToList())
                {
                    data = new MorrisGraphData();
                    data.label = priority.Name;
                    data.value = tickets.Where(t => t.OwnerId == userId).Where(t => t.Priority.Name == priority.Name).Count();
                    myData.Add(data);
                }


            }

            return Json(myData);
        }


    }
}


//data = new MorrisBarData();
//data.label = project.Name;
//                data.value = db.Projects.Find(project.Id == pr)
//                myData.Add(data);