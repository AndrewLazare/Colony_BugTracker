namespace COLONY_THE_BUGTRACKER.Migrations
{
    using COLONY_THE_BUGTRACKER.Utilities;
    using COLONY_THE_BUGTRACKER.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<COLONY_THE_BUGTRACKER.Models.ApplicationDbContext>

    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        #region ROLES>>>>
        protected override void Seed(COLONY_THE_BUGTRACKER.Models.ApplicationDbContext context)

        {
            if (!System.Diagnostics.Debugger.IsAttached)
            { 
                System.Diagnostics.Debugger.Launch();
            }
            var rolestore = new RoleStore<IdentityRole>(context);
            var rolemanager = new RoleManager<IdentityRole>(rolestore);
            var userstore = new UserStore<ApplicationUser>(context);
            var usermanager = new UserManager<ApplicationUser>(userstore);


          

            //if (!System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Launch();
            var roleManager = new RoleManager<IdentityRole>(
                              new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Manager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            #endregion  

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            #region DemoUser SEEDS

            if (!context.Users.Any(u => u.Email == "DemoAdmin@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@Mailinator.com",         
                    Email = "DemoAdmin@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin",

                }, "LearnToCodeNow!");
            }
            if (!context.Users.Any(u => u.Email == "DemoProjectManager@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoProjectManager@Mailinator.com",          
                    Email = "DemoProjectManager@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Manager",

                }, "LearnToCodeNow!");
            }
            if (!context.Users.Any(u => u.Email == "DemoDeveloper@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDeveloper@Mailinator.com",         
                    Email = "DemoDeveloper@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer",

                }, "LearnToCodeNow!");
            }
            if (!context.Users.Any(u => u.Email == "DemoSubmitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSubmitter@Mailinator.com",          /* these are property value pairs*/
                    Email = "DemoSubmitter@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter",

                }, "LearnToCodeNow!");
            }
            #endregion

            #region Admin Seeds
            if (!context.Users.Any(u => u.Email == "avlazareii@outlook.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "avlazareii@outlook.com",          /* these are property value pairs*/
                    Email = "avlazareii@outlook.com",
                    FirstName = "Andrew",
                    LastName = "Lazare",

                }, "pooPoo1987!");
            }


            #endregion
            #region PM Seeds

            if (!context.Users.Any(u => u.Email == "TestManager@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "TestManager@mailinator.com",          /* these are property value pairs*/
                    Email = "TestManager@mailinator.com",
                    FirstName = "Jim",
                    LastName = "Manager",

                }, "Abc&123!");
            }


           


            #endregion
            #region Sub Seeds

            if (!context.Users.Any(u => u.Email == "TestSubmitter@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "TestSubmitter@mailinator.com",          /* these are property value pairs*/
                    Email = "TestSubmitter@mailinator.com",
                    FirstName = "Lisa",
                    LastName = "Submitter",

                }, "Abc&123!");
            }




           


            if (!context.Users.Any(u => u.Email == "TestSubmitter3@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "TestSubmitter3@mailinator.com",          /* these are property value pairs*/
                    Email = "TestSubmitter3@mailinator.com",
                    FirstName = "Ryan",
                    LastName = "Submitter",

                }, "Abc&123!");
            }


            #endregion
            #region Dev Seeds

            if (!context.Users.Any(u => u.Email == "TestDeveloper@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "TestDeveloper@mailinator.com",          /* these are property value pairs*/
                    Email = "TestDeveloper@mailinator.com",
                    FirstName = "Roy",
                    LastName = "Developer",

                }, "Abc&123!");
            }


            if (!context.Users.Any(u => u.Email == "TestDeveloper2@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "TestDeveloper2@mailinator.com",          /* these are property value pairs*/
                    Email = "TestDeveloper2@mailinator.com",
                    FirstName = "Trent",
                    LastName = "Developer",

                }, "Abc&123!");
            }
            if (!context.Users.Any(u => u.Email == "TestDeveloper3@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "TestDeveloper3@mailinator.com",          /* these are property value pairs*/
                    Email = "TestDeveloper3@mailinator.com",
                    FirstName = "Todd",
                    LastName = "Developer",

                }, "Abc&123!");
            }

            if (!context.Users.Any(u => u.Email == "TestDeveloper4@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "TestDeveloper4@mailinator.com",          /* these are property value pairs*/
                    Email = "TestDeveloper4@mailinator.com",
                    FirstName = "Greg",
                    LastName = "Developer",

                }, "Abc&123!");
            }
            #endregion

            
            
            var adminId = userManager.FindByEmail("avlazareii@outlook.com").Id;   
            userManager.AddToRole(adminId, "Admin");

            var demoAdminId = userManager.FindByEmail("DemoAdmin@Mailinator.com").Id;
            userManager.AddToRole(demoAdminId, "Admin");


            var managerId = userManager.FindByEmail("TestManager@mailinator.com").Id;   
            userManager.AddToRole(managerId, "Manager");

            var demoManagerId = userManager.FindByEmail("DemoProjectManager@Mailinator.com").Id;
            userManager.AddToRole(demoManagerId, "Manager");
           

            var submitterId = userManager.FindByEmail("TestSubmitter@mailinator.com").Id;   
            userManager.AddToRole(submitterId, "Submitter");
            submitterId = userManager.FindByEmail("TestSubmitter3@mailinator.com").Id;    
            userManager.AddToRole(submitterId, "Submitter");
           
            var demoSubmitterId = userManager.FindByEmail("DemoSubmitter@Mailinator.com").Id;    
            userManager.AddToRole(demoSubmitterId, "Submitter");

            var developerId = userManager.FindByEmail("TestDeveloper@mailinator.com").Id;   
            userManager.AddToRole(developerId, "Developer");
            developerId = userManager.FindByEmail("TestDeveloper2@mailinator.com").Id;    
            userManager.AddToRole(developerId, "Developer");
            developerId = userManager.FindByEmail("TestDeveloper3@mailinator.com").Id;    
            userManager.AddToRole(developerId, "Developer");

            var demoDeveloperId = userManager.FindByEmail("DemoDeveloper@Mailinator.com").Id;    //it conducts a search leveraging the userManager and FindById...when its done its sitting on the record and I only want the id...so i put .id
            userManager.AddToRole(demoDeveloperId, "Developer");
            #region DEMO..PROJECT/TYPE/STATUS/PRiORITY
            context.Projects.AddOrUpdate(
             p => p.Name,
             new Project() { Name = "First Project", Description = "The first demo project seed", CreatedDate = DateTime.Now },
             new Project() { Name = "Second Project", Description = "The second demo project seed", CreatedDate = DateTime.Now },
             new Project() { Name = "Third Project", Description = "The third demo project seed", CreatedDate = DateTime.Now },
             new Project() { Name = "Fourth Project", Description = "The fourth demo project seed", CreatedDate = DateTime.Now });



            context.Types.AddOrUpdate(
             p => p.Name,
             new TicketType() { Name = "Documentation", Description = "Problem with documentation" },
             new TicketType() { Name = "Bug", Description = "There is a bug" },
             new TicketType() { Name = "Database", Description = "Probelem With Database" },
             new TicketType() { Name = "UI", Description = "Problem with UI" });

            context.Statuses.AddOrUpdate(
            p => p.Name,
            new TicketStatus() { Name = "Recieved", Description = "This ticket has been recieved" },
            new TicketStatus() { Name = "Opened", Description = "This ticket has been opened" },
            new TicketStatus() { Name = "Completed", Description = "This ticket has been completed" },
            new TicketStatus() { Name = "Closed", Description = "This ticket has been closed" });

            context.Priorities.AddOrUpdate(
            p => p.Name,
            new TicketPriority() { Name = "High", Description = "This ticket has a HIGH priority" },
            new TicketPriority() { Name = "Medium", Description = "This ticket has a MEDIUM priority" },
            new TicketPriority() { Name = "Low", Description = "This ticket has a LOW priority" });

            context.SaveChanges();

            var projects = context.Projects;
            var statuses = context.Statuses;
            var priorities = context.Priorities;
            var types = context.Types;
            var roles = context.Roles;
            var users = context.Users;


           #endregion
          


#region Ticket Status Seed






         

        }
    }
}
#endregion