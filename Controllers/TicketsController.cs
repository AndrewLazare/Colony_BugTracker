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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private TicketHistoryHelper auditHelper = new TicketHistoryHelper();
        private TicketNotificationsHelper notificationsHelper = new TicketNotificationsHelper();
        private TicketHelper tickHelper = new TicketHelper();


        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var allTickets = db.Tickets;
            var user = User.Identity.GetUserId();
            var subTickets = allTickets.Include(t => t.Owner).Include(t => t.Priority).Include(t => t.Project).Include(t => t.Status).Include(t => t.Type).Where(t => t.OwnerId == user);
            var devTickets = allTickets.Include(t => t.Owner).Include(t => t.Priority).Include(t => t.Project).Include(t => t.Status).Include(t => t.Type).Where(t => t.AssignedToUserId == user);
            switch (roleHelper.ListUserRoles(User.Identity.GetUserId()).First())
            {
                case "Admin":
                    return View(allTickets.ToList());
                case "Manager":
                    return View(allTickets.ToList());
                case "Developer":
                    return View(devTickets.ToList());
                case "Submitter":
                    return View(subTickets.ToList());
            }

            return View(allTickets.ToList());
        }









        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            TicketViewModel model = new TicketViewModel();



            Ticket ticket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == id);
            model.Ticket = ticket;
            var devs = roleHelper.UsersInRole("Developer").ToList();
            var subs = roleHelper.UsersInRole("Submitters").ToList();
            model.Developers = devs;
            //var ticketUsers = tickHelper.ListUsersOnTicket(ticket.Id);
            model.Submitters = subs;
            //ViewBag.RemoveDev = new SelectList(devs, "Id", "FirstName", ticket.AssignedToUserId);
            //ViewBag.Developer = new SelectList(devs, "Id", "FirstName");
            //ViewBag.UsersOnTickets = new MultiSelectList(ticketUsers, "Id", "FirstName");
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,Title,Summary,CreatedDate,UpdatedDate,OwnerId,PriorityId,StatusId,TypeId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.OwnerId = User.Identity.GetUserId();
                ticket.CreatedDate = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerId);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var devs = roleHelper.UsersInRole("Developer").ToList();
            ViewBag.Developers = new SelectList(devs, "Id", "FirstName", ticket);
            ViewBag.AssignedToUserId = new SelectList(devs, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerId);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,Title,Summary,CreatedDate,UpdatedDate,OwnerId,PriorityId,StatusId,TypeId")] Ticket ticket)
        {
            var ticketNotificationsHelper = new TicketNotificationsHelper();
            if (ModelState.IsValid)
            {

                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                ticket.UpdatedDate = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                

                notificationsHelper.SendTicketNotification(newTicket.Id);
                auditHelper.RecordHistoricalChanges(oldTicket, ticket);
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerId);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
