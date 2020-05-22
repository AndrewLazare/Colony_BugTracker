using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using COLONY_THE_BUGTRACKER.Models;
using COLONY_THE_BUGTRACKER.Utilities;
using Microsoft.AspNet.Identity;

namespace COLONY_THE_BUGTRACKER.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectHelper userPHelper = new UserProjectHelper();
        private RoleHelper roleHelper = new RoleHelper();

        // GET: Projects
        [Authorize]
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var myProjects = userPHelper.ListProjectsForUser(user);

            if (User.IsInRole("Admin"))
            {
                return View(db.Projects.ToList());
            }
            else 
            {
                return View(myProjects.ToList());
            }
        }

        // GET: Projects/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var users = roleHelper.UsersInRole("Manager");
            ViewBag.UsersOnProj = new MultiSelectList(users, "Id", "FullName");
            return View(project);
        }

        // GET: Projects/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,CreatedDate,UpdatedDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.CreatedDate = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,CreatedDate,")] Project project)
        {
            
            if (ModelState.IsValid)
            {

                project.UpdatedDate = DateTime.Now;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
            return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
