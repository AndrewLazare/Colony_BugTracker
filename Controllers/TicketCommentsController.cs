using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using COLONY_THE_BUGTRACKER.Models;
using COLONY_THE_BUGTRACKER.Utilities;
using Microsoft.AspNet.Identity;

namespace COLONY_THE_BUGTRACKER.Controllers
{
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketComments
        [Authorize]
        public ActionResult Index()
        {
            var ticketComments = db.Comments.Include(t => t.Owner).Include(t => t.Tickets);
            return View(ticketComments.ToList());
        }

        // GET: TicketComments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.Comments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // GET: TicketComments/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,Body,TicketAttachments")] TicketComment ticketComment, TicketAttachment ticketAttach, HttpPostedFileBase filePath)
        {
            if (ModelState.IsValid)
            {
                if (filePath != null)
                {

                    if (FileUploadHelper.IsWebFriendlyImage(filePath))
                    {

                        var fileName = Path.GetFileName(filePath.FileName);
                        fileName = fileName.Replace(' ', '_');
                        filePath.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                        ticketAttach.FilePath = "/Uploads/" + fileName; //assiging value of ticketAttach.FilePath to incomeing upload
                        
                        ticketAttach.UploadDate = DateTime.Now;
                        ticketAttach.UserId = User.Identity.GetUserId(); //property of this specific attachment is on left ...the right side of the = is what i am assigning to the property
                        db.Attachments.Add(ticketAttach);

                    }
                }


                ticketComment.CommentAttachment = ticketAttach.FilePath; //ticketComment. COmmentAttachment = to the ticketAttach.FilePath
                ticketComment.OwnerId = User.Identity.GetUserId(); //sending user id to DB
                ticketComment.CreatedDate = DateTime.Now; //sending current DATETIME to DB
                db.Comments.Add(ticketComment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId }); // to go details page of whatever you want you need its ID.  Details action requires ID.  give it to it so it doesnt blow up.  object route value

            }


            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticketComment.OwnerId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // GET: TicketComments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.Comments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticketComment.OwnerId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,Body,OwnerID,CreatedDate")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "FirstName", ticketComment.OwnerId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // GET: TicketComments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.Comments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComment ticketComment = db.Comments.Find(id);
            db.Comments.Remove(ticketComment);
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

