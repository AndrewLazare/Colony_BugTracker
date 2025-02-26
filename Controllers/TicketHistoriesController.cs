﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using COLONY_THE_BUGTRACKER.Models;

namespace COLONY_THE_BUGTRACKER.Controllers
{
    public class TicketHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketHistories
        [Authorize]
        public ActionResult Index()
        {
            var ticketHistories = db.Histories.Include(t => t.Ticket).Include(t => t.Updater);
            return View(ticketHistories.ToList());
        }

        // GET: TicketHistories/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistory ticketHistory = db.Histories.Find(id);
            if (ticketHistory == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistory);
        }

        // GET: TicketHistories/Create
        [Authorize]
        public ActionResult Create()
        {
            
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            ViewBag.UpdaterId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UpdateDate,TicketId,OldValue,NewValue,UpdaterId")] TicketHistory ticketHistory)
        {
            
            if (ModelState.IsValid)
            {
               
                db.Histories.Add(ticketHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistory.TicketId);
            ViewBag.UpdaterId = new SelectList(db.Users, "Id", "FirstName", ticketHistory.UpdaterId);
            return View(ticketHistory);
        }

        // GET: TicketHistories/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistory ticketHistory = db.Histories.Find(id);
            if (ticketHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistory.TicketId);
            ViewBag.UpdaterId = new SelectList(db.Users, "Id", "FirstName", ticketHistory.UpdaterId);
            return View(ticketHistory);
        }

        // POST: TicketHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UpdateDate,TicketId,OldValue,NewValue,UpdaterId")] TicketHistory ticketHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistory.TicketId);
            ViewBag.UpdaterId = new SelectList(db.Users, "Id", "FirstName", ticketHistory.UpdaterId);
            return View(ticketHistory);
        }

        // GET: TicketHistories/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistory ticketHistory = db.Histories.Find(id);
            if (ticketHistory == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistory);
        }

        // POST: TicketHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketHistory ticketHistory = db.Histories.Find(id);
            db.Histories.Remove(ticketHistory);
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
