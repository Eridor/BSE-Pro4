using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BSE_Pro4.Models;
using Microsoft.AspNet.Identity;

namespace BSE_Pro4.Controllers
{
    [Authorize]
    public class UserShipmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminUserShipments
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            var userShipments = db.UserShipments.Include(u => u.User).Where(t => t.UserID == userid);
            return View(userShipments.ToList());
        }

        // GET: AdminUserShipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userid = User.Identity.GetUserId();
            UserShipment userShipment = db.UserShipments.Include(t=>t.User).SingleOrDefault(t=> t.UserShipId == id);
            if (userShipment == null || userShipment.UserID != userid)
            {
                return HttpNotFound();
            }
            return View(userShipment);
        }

        // GET: AdminUserShipments/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users.Where(t=> t.Id == User.Identity.GetUserId()), "Id", "Email");
            return View();
        }

        // POST: AdminUserShipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserShipId,UserID,Invoice,Name,Surname,Country,City,ZipCode,Street,NumberHouse,NumberFlat,Telephone,AdditionalInfo")] UserShipment userShipment)
        {
            if (ModelState.IsValid)
            {
                var us = userShipment;
                us.UserID = User.Identity.GetUserId();
                db.UserShipments.Add(us);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users.Where(t => t.Id == User.Identity.GetUserId()), "Id", "Email", userShipment.UserID);
            return View(userShipment);
        }

        // GET: AdminUserShipments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserShipment userShipment = db.UserShipments.Find(id);
            string userid = User.Identity.GetUserId();
            if (userShipment == null || userShipment.UserID != userid)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "Email", userShipment.UserID);
            return View(userShipment);
        }

        // POST: AdminUserShipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserShipId,UserID,Invoice,Name,Surname,Country,City,ZipCode,Street,NumberHouse,NumberFlat,Telephone,AdditionalInfo")] UserShipment userShipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userShipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "Email", userShipment.UserID);
            return View(userShipment);
        }

        // GET: AdminUserShipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserShipment userShipment = db.UserShipments.Find(id);
            string userid = User.Identity.GetUserId();
            if (userShipment == null || userShipment.UserID != userid)
            {
                return HttpNotFound();
            }
            return View(userShipment);
        }

        // POST: AdminUserShipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserShipment userShipment = db.UserShipments.Find(id);
            db.UserShipments.Remove(userShipment);
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
