﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BSE_Pro4.Models;

namespace BSE_Pro4.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTransactionStatusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminTransactionStatus
        public ActionResult Index()
        {
            return View(db.TransactionStatus.ToList());
        }

        // GET: AdminTransactionStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionStatus transactionStatus = db.TransactionStatus.Find(id);
            if (transactionStatus == null)
            {
                return HttpNotFound();
            }
            return View(transactionStatus);
        }

        // GET: AdminTransactionStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminTransactionStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionStatusId,Description")] TransactionStatus transactionStatus)
        {
            if (ModelState.IsValid)
            {
                db.TransactionStatus.Add(transactionStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transactionStatus);
        }

        // GET: AdminTransactionStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionStatus transactionStatus = db.TransactionStatus.Find(id);
            if (transactionStatus == null)
            {
                return HttpNotFound();
            }
            return View(transactionStatus);
        }

        // POST: AdminTransactionStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionStatusId,Description")] TransactionStatus transactionStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transactionStatus);
        }

        // GET: AdminTransactionStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionStatus transactionStatus = db.TransactionStatus.Find(id);
            if (transactionStatus == null)
            {
                return HttpNotFound();
            }
            return View(transactionStatus);
        }

        // POST: AdminTransactionStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransactionStatus transactionStatus = db.TransactionStatus.Find(id);
            db.TransactionStatus.Remove(transactionStatus);
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
