using System;
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
    public class AdminTransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminTransactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.TransactionStatus).Include(t => t.User).Include(t => t.UserInvoice).Include(t => t.UserShipment);
            return View(transactions.ToList());
        }

        // GET: AdminTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: AdminTransactions/Create
        public ActionResult Create()
        {
            ViewBag.TransactionStatusId = new SelectList(db.TransactionStatus, "TransactionStatusId", "Description");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.UserInvoiceId = new SelectList(db.UserShipments, "UserShipId", "UserID");
            ViewBag.UserShipmentId = new SelectList(db.UserShipments, "UserShipId", "UserID");
            return View();
        }

        // POST: AdminTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionId,UserId,UserShipmentId,UserInvoiceId,TotalCost,TransactionStatusId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TransactionStatusId = new SelectList(db.TransactionStatus, "TransactionStatusId", "Description", transaction.TransactionStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", transaction.UserId);
            ViewBag.UserInvoiceId = new SelectList(db.UserShipments, "UserShipId", "UserID", transaction.UserInvoiceId);
            ViewBag.UserShipmentId = new SelectList(db.UserShipments, "UserShipId", "UserID", transaction.UserShipmentId);
            return View(transaction);
        }

        // GET: AdminTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.TransactionStatusId = new SelectList(db.TransactionStatus, "TransactionStatusId", "Description", transaction.TransactionStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", transaction.UserId);
            ViewBag.UserInvoiceId = new SelectList(db.UserShipments, "UserShipId", "UserID", transaction.UserInvoiceId);
            ViewBag.UserShipmentId = new SelectList(db.UserShipments, "UserShipId", "UserID", transaction.UserShipmentId);
            return View(transaction);
        }

        // POST: AdminTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionId,UserId,UserShipmentId,UserInvoiceId,TotalCost,TransactionStatusId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TransactionStatusId = new SelectList(db.TransactionStatus, "TransactionStatusId", "Description", transaction.TransactionStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", transaction.UserId);
            ViewBag.UserInvoiceId = new SelectList(db.UserShipments, "UserShipId", "UserID", transaction.UserInvoiceId);
            ViewBag.UserShipmentId = new SelectList(db.UserShipments, "UserShipId", "UserID", transaction.UserShipmentId);
            return View(transaction);
        }

        // GET: AdminTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: AdminTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
