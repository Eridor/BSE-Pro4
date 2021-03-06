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
    public class AdminProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminProducts
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Author).Include(p => p.Category).Include(p => p.Tax);
            return View(products.ToList());
        }

        // GET: AdminProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include(p => p.Author).Include(p => p.Category).Include(p => p.Tax).SingleOrDefault(t=> t.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: AdminProducts/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.TaxId = new SelectList(db.Taxes, "TaxId", "Value");
            return View();
        }

        // POST: AdminProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,CategoryId,AuthorId,Name,Desc,Format,Pages,QuantityAvailable,Cost,TaxId,Discount")] Product product)
        {

            if(product.Format.Length > 100)
            {
                ViewBag.Error += "Za długi format książki   ";
            }
            double k;
            if (double.TryParse(product.Pages,out k))
            {
                ViewBag.Error += "Nieprawidłowa ilość stron   ";
            }
            if (product.Cost < 0)
            {
                ViewBag.Error += "Cena nie może być ujemna   ";
            }
            if (product.Discount < 0 || product.Discount > 1)
            {
                ViewBag.Error += "Rabat musi być z przedziału 0% - 100%";
            }

            if (ModelState.IsValid && ViewBag.Error == null)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", product.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.TaxId = new SelectList(db.Taxes, "TaxId", "Value", product.TaxId);
            return View(product);
        }

        // GET: AdminProducts/Edit/5
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
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", product.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.TaxId = new SelectList(db.Taxes, "TaxId", "Value", product.TaxId);
            return View(product);
        }

        // POST: AdminProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,CategoryId,AuthorId,Name,Desc,Format,Pages,QuantityAvailable,Cost,TaxId,Discount")] Product product)
        {
            if (product.Format.Length > 100)
            {
                ViewBag.Error += "Za długi format książki   ";
            }
            double k;
            if (double.TryParse(product.Pages, out k))
            {
                ViewBag.Error += "Nieprawidłowa ilość stron   ";
            }
            if (product.Cost < 0)
            {
                ViewBag.Error += "Cena nie może być ujemna   ";
            }
            if (product.Discount < 0 || product.Discount > 1)
            {
                ViewBag.Error += "Rabat musi być z przedziału 0% - 100%";
            }

            if (ModelState.IsValid && ViewBag.Error == null)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "AuthorId", "Name", product.AuthorId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.TaxId = new SelectList(db.Taxes, "TaxId", "Value", product.TaxId);
            return View(product);
        }

        // GET: AdminProducts/Delete/5
        public ActionResult Delete(int? id)
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
            return View(product);
        }

        // POST: AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
