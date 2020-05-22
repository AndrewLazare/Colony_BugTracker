using COLONY_THE_BUGTRACKER.Models;
using COLONY_THE_BUGTRACKER.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COLONY_THE_BUGTRACKER.Controllers
{
    /// <summary>
    /// Project project, List<string> UsersOnProj  
    ///  =====passing in the project model to help with redirecting bCK to the details page and for removing user from project. 
    /// via the helper method.  Useronproj are users in list box
    /// </summary>
    public class AdminController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private UserProjectHelper userProjectHelper = new UserProjectHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private TicketNotificationsHelper notifyHelper = new TicketNotificationsHelper();
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUser(Project project, List<string> UsersOnProj)
        {
           
           
            foreach (var user in UsersOnProj)
            {
                userProjectHelper.RemoveUserFromProject(user, project.Id);
            }
            return RedirectToAction("Details", "Projects", new { project.Id }); 

        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUserFromTicket(Ticket ticket, string removeDev)
        {
            var ticketInfo = db.Tickets.Find(ticket.Id);
            ticketInfo.AssignedToUserId = null;
            db.SaveChanges();
            notifyHelper.SendTicketNotification(ticketInfo.Id);

            return RedirectToAction("Details", "Tickets", new { ticketInfo.Id });


        }
      
        public ActionResult AjaxRemoveUserFromTicket(int ticketId, string userId)
        {
            var ticketInfo = db.Tickets.Find(ticketId);
            ticketInfo.AssignedToUserId = null; 
            db.SaveChanges();
            notifyHelper.SendTicketNotification(ticketInfo.Id);
            return PartialView("~/Views/Shared/_DevAssignedToTicket.cshtml", db.Tickets.Find(ticketId));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddToTicket(Ticket ticket, string Developer)
        {
            var ticketInfo = db.Tickets.Find(ticket.Id);
            ticketHelper.AddUserToTicket(Developer, ticketInfo.Id);
            ticketInfo.AssignedToUserId = db.Users.Find(Developer).Id;
            db.SaveChanges();
            notifyHelper.SendTicketNotification(ticketInfo.Id);

            return RedirectToAction("Details", "Tickets", new { ticketInfo.Id });
        }

        public ActionResult AjaxAddToTicket(string id, int ticketId)
        {
            var ticketInfo = db.Tickets.Find(ticketId);
            ticketInfo.AssignedToUserId = id;
            db.SaveChanges();
            notifyHelper.SendTicketNotification(ticketInfo.Id);
            return PartialView("~/Views/Shared/_DevAssignedToTicket.cshtml", db.Tickets.Find(ticketId));

        }
        [Authorize]
        public ActionResult EditProfile(string Id)  //just returning to view.  I need this
        {
            var sourceUser = db.Users.Find(User.Identity.GetUserId());
            
            var userVm = new UserProfileViewModel();
            userVm.FirstName = sourceUser.FirstName;
            userVm.Id = sourceUser.Id;

            return PartialView("_EditProfilePartial", userVm);
        }

        

        private IDisposable ApplicationDbContext()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileViewModel model, HttpPostedFileBase avatar)
        {

            if (!VerificationHelper.CheckPassword(model.OldPassword)) 
            {
                return RedirectToAction("Index", "Home");
            } 
                
            var user = db.Users.Find(model.Id);
            if (model.FirstName != null || model.LastName != null || avatar != null)
            {
                if(user.FirstName != null || user.LastName != null)
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;


                if (ImageUploadValidator.IsWebFriendlyImage(avatar))
                {
                    var fileName = Path.GetFileName(avatar.FileName);
                    var justFileName = Path.GetFileNameWithoutExtension(fileName);
                    justFileName = StringUtilities.URLFriendly(justFileName);
                    fileName = $"{justFileName}_{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                    avatar.SaveAs(Path.Combine(Server.MapPath("~/Avatars/"), fileName));
                    user.AvatarPath = "/Avatars/" + fileName;


                }
                db.SaveChanges();

            }

            return RedirectToAction("Index", "Home");

        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjects(Project project, List<string> UsersOnProj)
        {
            foreach (var user in UsersOnProj)
            {
                userProjectHelper.AddUserToProject(user, project.Id);
            }
            return RedirectToAction("Details", "Projects", new { project.Id });
        }
        public ActionResult AjaxAddManager(string id, int projectId)
        {
           
            var managersInProject = db.Projects.Find(projectId).GetManagersOnProject();
            foreach (var manager in managersInProject)
            {
                userProjectHelper.RemoveUserFromProject(manager.Id, projectId);
            }

            userProjectHelper.AddUserToProject(id, projectId);
            db.SaveChanges();

            return PartialView("~/Views/Shared/_ManAssignedToProject.cshtml", db.Projects.Find(projectId));
        }



        public ActionResult ManageRoles()
        {

            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "FullName"); //Third argument is what we want to see in the multi list.  second one is the name of the colomn that actualyy gets admited to the post.  1st is calling data from DB
            ViewBag.Role = new SelectList(db.Roles, "Name", "Name"); //I want to display all the roles in the system here.  show and pass name here.

            var users = new List<ManageRolesViewModel>();
            foreach (var user in db.Users.ToList())
            {
                users.Add(new ManageRolesViewModel
                {
                    UserName = $"{user.LastName}, {user.FirstName}",  //string interpilation
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault()

                });
            }

            return View(users);
        }
        /// <summary>
        ///  Step1: unassign any user from any role they could be assigned. looks at al the people i have selected in multilist.
        ///  What is the role of this person? finds them one at a time to assign them their one and only role. if do occupy a role.
        ///  remove them from that role. Step2: add them back to selected role.
        ///  Third argument is what we want to see in the multi list.  second one is the name of the colomn that actualyy gets admited to the post.
        ///  1st is calling data from DB. I want to display all the roles in the system here.  show and pass name here.
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string role)
        {
           

            foreach (var userId in userIds) 
            {
                
                var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault(); 
                if (userRole != null) 
                {
                    roleHelper.RemoveUserFromRole(userId, userRole);
                }
            }


            
            if (!string.IsNullOrEmpty(role))
            {
                foreach (var userId in userIds)
                {
                    roleHelper.AddUserToRole(userId, role);
                }

            }

            return RedirectToAction("ManageRoles", "Admin");
        }
        [Authorize]
        public ActionResult ManageUsers()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email"); 
            ViewBag.ManageUsers = new SelectList(db.Projects, "Id", "Name"); 

            var users = new List<ManageProjectsViewModel>();
            foreach (var user in db.Users.ToList())
            {
                var projectName = "N/A";
                if (projectHelper.ListUserProjects(user.Id).FirstOrDefault() != null)
                {
                    projectName = projectHelper.ListUserProjects(user.Id).FirstOrDefault().Name;
                }
                users.Add(new ManageProjectsViewModel
                {
                    Id = user.Id,
                    Name = projectName
                });
            }

            return View(users);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUsers(List<string> userIds, int manageUsers) //Every id except user is a int
        {
            //  Step1: unassign any user from any role they could be assigned.

            foreach (var userId in userIds) //looks at al the people i have selected in multilist
            {
                //What is the role of this person?
                var userProject = projectHelper.ListUserProjects(userId).FirstOrDefault(); //finds them one at a time to assign them their one and only role
                if (userProject != null) //if do occupy a role 
                {
                    projectHelper.RemoveUserFromProject(userId, manageUsers); //remove them from that role
                }
            }


            //Step2: add them back to selected role
            if (manageUsers != 0)
            {
                foreach (var userId in userIds)
                {
                    projectHelper.AddUserToProject(userId, manageUsers);
                }

            }

            return RedirectToAction("ManageUsers", "Admin");
        }

    }
}

