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
    public class AdminTransactionItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminTransactionItems
        public ActionResult Index()
        {
            var transactionItems = db.TransactionItems.Include(t => t.Product).Include(t => t.Transaction);
            return View(transactionItems.ToList());
        }

        // GET: AdminTransactionItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionItem transactionItem = db.TransactionItems.Find(id);
            if (transactionItem == null)
            {
                return HttpNotFound();
            }
            return View(transactionItem);
        }

        // GET: AdminTransactionItems/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name");
            ViewBag.TransactionId = new SelectList(db.Transactions, "TransactionId", "UserId");
            return View();
        }

        // POST: AdminTransactionItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionItemId,TransactionId,ProductId,Count,Cost,Tax,Discount")] TransactionItem transactionItem)
        {
            if (ModelState.IsValid)
            {
                db.TransactionItems.Add(transactionItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", transactionItem.ProductId);
            ViewBag.TransactionId = new SelectList(db.Transactions, "TransactionId", "UserId", transactionItem.TransactionId);
            return View(transactionItem);
        }

        // GET: AdminTransactionItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionItem transactionItem = db.TransactionItems.Find(id);
            if (transactionItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", transactionItem.ProductId);
            ViewBag.TransactionId = new SelectList(db.Transactions, "TransactionId", "UserId", transactionItem.TransactionId);
            return View(transactionItem);
        }

        // POST: AdminTransactionItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionItemId,TransactionId,ProductId,Count,Cost,Tax,Discount")] TransactionItem transactionItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "Name", transactionItem.ProductId);
            ViewBag.TransactionId = new SelectList(db.Transactions, "TransactionId", "UserId", transactionItem.TransactionId);
            return View(transactionItem);
        }

        // GET: AdminTransactionItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionItem transactionItem = db.TransactionItems.Find(id);
            if (transactionItem == null)
            {
                return HttpNotFound();
            }
            return View(transactionItem);
        }

        // POST: AdminTransactionItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransactionItem transactionItem = db.TransactionItems.Find(id);
            db.TransactionItems.Remove(transactionItem);
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
