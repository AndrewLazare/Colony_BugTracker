using COLONY_THE_BUGTRACKER.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COLONY_THE_BUGTRACKER.Controllers
{   [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()

        {
            var roleHelper = new Utilities.RoleHelper();
            switch (roleHelper.ListUserRoles(User.Identity.GetUserId()).FirstOrDefault())
            {
                case "Admin":
                    return View("Index");
                case "Manager":
                    return View("ManagerIndex");
                case "Developer":
                    return View("DeveloperIndex");
                case "Submitter": 
                    return View("SubmitterIndex");
            }
            return View();
        }
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);
        }
        
       
    }
}