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
    public class UserCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserCarts
        public ActionResult Index()
        {
            string userid = User.Identity.GetUserId();
            var carts = db.Carts.Where(t => t.UserID == userid).Include(c => c.ProductItem).Include(c => c.User);
            return View(carts.ToList());
        }

        public ActionResult Sub(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            cart.Quantity--;
            if (cart.Quantity == 0)
                db.Carts.Remove(cart);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Add(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            cart.Quantity++;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: UserCarts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: UserCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
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
