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
    public class TicketNotificationsController : Controller
    
    {
        private TicketNotificationsHelper notificationsHelper = new TicketNotificationsHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Dismiss(int id) 
        {
            var notification = db.TicketNotifications.Find(id);
            notification.IsRead = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        // GET: TicketNotifications
        [Authorize]
        public ActionResult Index()
        {
            var ticketNotifications = db.TicketNotifications.Include(t => t.Recipient).Include(t => t.Ticket);
            return View(ticketNotifications.ToList());
        }

        // GET: TicketNotifications/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Create
        [Authorize]
        public ActionResult Create()
        {
            
            ViewBag.RecipientID = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TIcketId = new SelectList(db.Tickets, "Id", "Title");
            return View();
        }

        // POST: TicketNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TIcketId,NotificationBody,Title,CreatedDate,SenderId,RecipientID,IsRead")] TicketNotifications ticketNotifications)
        {
            if (ModelState.IsValid)
            {
                ticketNotifications.SenderId = User.Identity.GetUserId();
                ticketNotifications.CreatedDate = DateTime.Now;
                db.TicketNotifications.Add(ticketNotifications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.RecipientID = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.RecipientID);
            ViewBag.TIcketId = new SelectList(db.Tickets, "Id", "Title", ticketNotifications.TIcketId);
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecipientID = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.RecipientID);
            ViewBag.TIcketId = new SelectList(db.Tickets, "Id", "Title", ticketNotifications.TIcketId);
            return View(ticketNotifications);
        }

        // POST: TicketNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TIcketId,NotificationBody,CreatedDate,RecipientID,IsRead")] TicketNotifications ticketNotifications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketNotifications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RecipientID = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.RecipientID);
            ViewBag.TIcketId = new SelectList(db.Tickets, "Id", "Title", ticketNotifications.TIcketId);
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotifications);
        }

        // POST: TicketNotifications/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            db.TicketNotifications.Remove(ticketNotifications);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public void AjaxDelete(int id)
        {
            var notification = db.TicketNotifications.Find(id);
            notification.IsRead = true;
            db.SaveChanges();
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
