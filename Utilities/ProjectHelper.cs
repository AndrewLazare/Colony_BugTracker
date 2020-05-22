using COLONY_THE_BUGTRACKER.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace COLONY_THE_BUGTRACKER.Utilities
{
    public class ProjectHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
           new UserStore<ApplicationUser>(
             new ApplicationDbContext()));

        ApplicationDbContext db = new ApplicationDbContext();
        RoleHelper roleHelper = new RoleHelper();
        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return (flag);
        }
        public ICollection<Project> ListUserProjects(string userId)
        {
            var myProjects = new List<Project>();
            var user = db.Users.Find(userId);
            var myRole = roleHelper.ListUserRoles(HttpContext.Current.User.Identity.GetUserId()).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    myProjects.AddRange(db.Projects);
                    break;
                case "Manager":
                case "Developer":
                case "Submitter":
                    myProjects.AddRange(user.Projects);
                    break;
                default:
                    break;
            }
            return (myProjects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);
                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }
        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUser = db.Users.Find(userId);
                proj.Users.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified; // just saves this obj instance.
                db.SaveChanges();
            }
        }
        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }
        public ICollection<ApplicationUser> UsersOnProject(int projectId, string role)
        {
            return db.Projects.Find(projectId).Users.Where(r => roleHelper.ListUserRoles(r.Id).FirstOrDefault() == role).ToList();
        }
        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }

        public string GetProjectManagerDisplayName(int projectId)
        {
          foreach (var user in db.Projects.Find(projectId).Users.ToList())
            {
                if (roleHelper.IsUserInRole(user.Id, "Manager"))
                    return $"{user.LastName}, {user.FirstName}";
            }
            return "n/a";
        }
    }
}
