using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using COLONY_THE_BUGTRACKER.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace COLONY_THE_BUGTRACKER.Utilities
{
    public class VerificationHelper
    {
        public static bool CheckPassword(string password)
        {
            using (var db = new ApplicationDbContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)); 
                if (HttpContext.Current.Request.IsAuthenticated) 
                {
                    var user = UserManager.FindById(HttpContext.Current.User.Identity.GetUserId());
                    if (UserManager.CheckPassword(user, password))
                        return true;
                }
            }
            return false;
        }



    }
}