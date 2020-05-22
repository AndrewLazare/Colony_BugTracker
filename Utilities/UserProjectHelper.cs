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
    public class UserProjectHelper
    {
             private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(new ApplicationDbContext()));
            private ApplicationDbContext db = new ApplicationDbContext();
            public bool IsOnProject(string userId, int projectId)
            {
                if (db.Projects.Find(projectId).Users.Contains(db.Users.Find(userId)))
                {
                    return true;

                }
                return false;


            }

            public void AddUserToProject(string userId, int projectId)
            {
                if (!IsOnProject(userId, projectId))
                {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();

            }
        }
            public void RemoveUserFromProject(string userId, int projectId)
            {
                if (IsOnProject(userId, projectId))
                {
                    var project = db.Projects.Find(projectId);
                    project.Users.Remove(db.Users.Find(userId));
                    db.Entry(project).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
        public ICollection<ApplicationUser> UsersOnProject(int? projectId) 
            {
                return db.Projects.Find(projectId).Users.ToList();
            }
            
        public ICollection<Project> ListProjectsForUser(string userId) 
            {
                return db.Users.Find(userId).Projects;
            }
        public ICollection<ApplicationUser> ListUsersNotOnProject(int projectId) 
            {
                return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
            }


        
        
        
        
    }
}
  