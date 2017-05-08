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
    public class UserProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserProducts
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Author).Include(p => p.Category).Include(p => p.Tax);
            return View(products.ToList());
        }

        public ActionResult Category(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Include(t=>t.Products).SingleOrDefault(t=>t.CategoryId == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        public ActionResult Author(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Include(t => t.Products).SingleOrDefault(t => t.AuthorId == id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: UserProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userid = User.Identity.GetUserId();
           // var carts = db.Carts.Where(t => t.UserID == userid).Include(c => c.ProductItem).Include(c => c.User).Include(c => c.ProductItem.Tax);
            Product product = db.Products.Include(p => p.Author).Include(p => p.Category).Include(p => p.Tax).SingleOrDefault(t => t.ProductId == id);

            ////For combobox
            var list = new List<SelectListItem>();
            for (var i = 1; i <= product.QuantityAvailable; i++)
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            ViewBag.list = list;

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        
        /*
        // GET: UserProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            product.QuantityAvailable -= 1;
            Cart newCart = new Cart();

           /// string userid = User.Identity.GetUserId();
          //  var carts = db.Carts.Where(t => t.UserID == userid).Include(c => c.ProductItem).Include(c => c.User);

            newCart.ProductId = product.ProductId;
           // newCart.UserID = userid;

            db.SaveChanges();

            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", product.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.TaxId = new SelectList(db.Taxes, "TaxId", "Description", product.TaxId);
            return View(product);
        }

        // POST: UserProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,CategoryId,AuthorId,Name,Desc,Format,Pages,QuantityAvailable,Cost,TaxId,Discount")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", product.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.TaxId = new SelectList(db.Taxes, "TaxId", "Description", product.TaxId);
            return View(product);
        }

    */
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
