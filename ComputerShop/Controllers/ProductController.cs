﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComputerShop.Models;
using ComputerShop.Validators;
using ComputerShop.Managers;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
namespace ComputerShop.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Product
        public async Task<ActionResult> Index()
        {
            return View(await db.Product.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorReason"] = "Could not find product with id = " + id;
                return View("Error");
            }
            return View(product);
        }

        // GET: Product/Create
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can create product")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can create product")]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductName,ProductType,Price,ProductionDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Product/Edit/5
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can edit product")]
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorReason"] = "Could not find product with id = " + id;
                return View("Error");
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can edit product")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductName,ProductType,Price,ProductionDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Delete/5
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can delete product")]
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorReason"] = "Could not find product with id = " + id;
                return View("Error");
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can delete product")]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Product product = await db.Product.FindAsync(id);
            db.Product.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Product/Buy/5
        [AuthorizeWithMessage(Roles = "Admin, User", ErrorMessage = "If u want buy this product you must be logged in")]
        public async Task<ActionResult> Buy(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorReason"] = "Could not find product";
                return View("Error");
            }
            return View(product);
        }

        // POST: Product/Buy/5
        [HttpPost, ActionName("Buy")]
        [ValidateAntiForgeryToken]
        [AuthorizeWithMessage(Roles = "Admin, User", ErrorMessage = "If u want buy this product you must be logged in")]
        public async Task<ActionResult> Buy(long ProductId, int? quantity)
        {
            if (db.Product.Find(ProductId) == null)
            {
                return View("Error");
            }
            if (quantity == null)
            {
                return View("Error");
            }
            if(quantity < 0 || quantity > 100)
            {
                return View("Error");
            }

            long UserId = User.Identity.GetUserId<long>();

            Purchase purchase = (from p in db.Purchase where
                                 p.UserId == UserId &&
                                 p.IsConfirmed == false
                                 select p).FirstOrDefault();

            if (purchase == null)
            {
                purchase = new Purchase() { UserId = UserId, IsConfirmed = false, PurchaseDate = DateTime.Now };
                db.Purchase.Add(purchase);
                await db.SaveChangesAsync();
                db.Entry(purchase).GetDatabaseValues();
                if (purchase.Id > 0)
                {
                    Basket b = new Basket() { ProductId = ProductId, PurchaseId = purchase.Id, Quantity = quantity.Value };
                    db.Basket.Add(b);
                }
            }
            else
            {
                Basket basketExist = await db.Basket.FindAsync(purchase.Id, ProductId);

                if (basketExist != null)
                {
                    return RedirectToAction("Edit", "MyOrders", routeValues: new { PurchaseId = purchase.Id, ProductId = ProductId });
                }
                basketExist = new Basket() { ProductId = ProductId, PurchaseId = purchase.Id, Quantity = quantity.Value };
                db.Basket.Add(basketExist);
            }
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "MyOrders");
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
