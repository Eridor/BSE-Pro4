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
    public class UserTransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserTransactions
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            var transactions = db.Transactions.Include(t => t.TransactionStatus).Include(t => t.User).Include(t => t.UserInvoice).Include(t => t.UserShipment).Where(t => t.UserId == userid);
            return View(transactions.ToList());
        }

        // GET: UserTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userid = User.Identity.GetUserId();
            Transaction transaction = db.Transactions.Include(t => t.TransactionStatus).Include(t => t.User).Include(t => t.UserInvoice).Include(t => t.UserShipment).FirstOrDefault(t => t.UserId == userid && t.TransactionId == id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: UserTransactions/Create
        public ActionResult Create()
        {
            string userid = User.Identity.GetUserId();
            var carts = db.Carts.Where(t => t.UserID == userid).Include(c => c.ProductItem).Include(c => c.User).Include(c => c.ProductItem.Tax);
            
            if (!carts.Any())
                return RedirectToAction("Index");

            Transaction ts = db.Transactions.Include(t=> t.TransactionItems.Select(s=> s.Product).Select(s=> s.Tax)).SingleOrDefault(
                    t =>
                        t.UserId == userid &&
                        t.TransactionStatus == db.TransactionStatus.FirstOrDefault(k => k.Description == "Nowe"));
            if (ts == null)
            {
                ts = new Transaction();
                ts.TransactionStatus = db.TransactionStatus.FirstOrDefault(t => t.Description == "Nowe");
                ts.UserId = userid;
                db.Transactions.Add(ts);


                double sum = 0;
                foreach (var item in carts)
                {
                    db.TransactionItems.Add(new TransactionItem()
                    {
                        Cost = item.ProductItem.Cost,
                        Count = item.Quantity,
                        Discount = item.ProductItem.Discount,
                        Product = item.ProductItem,
                        Tax = item.ProductItem.Tax.Value,
                        Transaction = ts

                    });
                    sum += (item.Quantity * item.ProductItem.Cost * (1 - item.ProductItem.Discount) * (1 + item.ProductItem.Tax.Value));
                }
                ts.TotalCost = sum;
                db.SaveChanges();
            }
            
            
            ViewBag.UserInvoiceId = new SelectList(db.UserShipments, "UserShipId", "AdditionalInfo");
            ViewBag.UserShipmentId = new SelectList(db.UserShipments, "UserShipId", "AdditionalInfo");
            return View(ts);
        }

        // POST: UserTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserShipmentId,UserInvoiceId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                string userid = User.Identity.GetUserId();
                Transaction ts = db.Transactions.Include(t => t.TransactionItems.Select(s => s.Product).Select(s => s.Tax)).SingleOrDefault(
                    t =>
                        t.UserId == userid &&
                        t.TransactionStatus == db.TransactionStatus.FirstOrDefault(k => k.Description == "Nowe"));

                ts.UserInvoice = transaction.UserInvoice;
                ts.UserShipment = transaction.UserShipment;
                ts.TransactionStatus = db.TransactionStatus.FirstOrDefault(k => k.Description == "Kompletowane");
                db.Carts.RemoveRange(db.Carts.Where(t => t.UserID == userid));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TransactionStatusId = new SelectList(db.TransactionStatus, "TransactionStatusId", "Description", transaction.TransactionStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", transaction.UserId);
            ViewBag.UserInvoiceId = new SelectList(db.UserShipments, "UserShipId", "UserID", transaction.UserInvoiceId);
            ViewBag.UserShipmentId = new SelectList(db.UserShipments, "UserShipId", "UserID", transaction.UserShipmentId);
            return View(transaction);
        }

        public ActionResult Abort()
        {
            string userid = User.Identity.GetUserId();
            Transaction ts = db.Transactions.SingleOrDefault(
                    t =>
                        t.UserId == userid &&
                        t.TransactionStatus == db.TransactionStatus.FirstOrDefault(k => k.Description == "Nowe"));
            db.Transactions.Remove(ts);
            db.SaveChanges();
            return RedirectToAction("Index", "UserCarts");
            
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
