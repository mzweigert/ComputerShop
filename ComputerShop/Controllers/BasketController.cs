using System;
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

namespace ComputerShop.Controllers
{
    [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can be here")]
    public class BasketController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Basket
        public async Task<ActionResult> Index()
        {
            var basket = db.Basket.Include(b => b.Product).Include(b => b.Purchase);
            return View(await basket.ToListAsync());
        }

        // GET: Basket/Details/5
        public async Task<ActionResult> Details(long? PurchaseId, long? ProductId)
        {
            if (PurchaseId == null || ProductId == null)
            {
                return RedirectToAction("Index");
            }
            Basket basket = await db.Basket.FindAsync(PurchaseId, ProductId);
            if (basket == null)
            {
                TempData["ErrorReason"] = "Could not find basket with PurchaseId = " + PurchaseId + " and ProductId = " + ProductId;
                return View("Error");
            }
            return View(basket);
        }


        // GET: Basket/Edit/5
        public async Task<ActionResult> Edit(long? PurchaseId, long? ProductId)
        {
            if (PurchaseId == null || ProductId == null)
            {
                return RedirectToAction("Index");
            }
            Basket basket = await db.Basket.FindAsync(PurchaseId, ProductId);
            if (basket == null)
            {
                TempData["ErrorReason"] = "Could not find basket with PurchaseId = " + PurchaseId+ " and ProductId = "+ProductId;
                return View("Error");
            }
            return View(basket);
        }

        // POST: Basket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PurchaseId,ProductId,Quantity")] Basket basket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(basket);
        }

        // GET: Basket/Delete/5
        public async Task<ActionResult> Delete(long? PurchaseId, long? ProductId)
        {
            if (PurchaseId == null || ProductId == null)
            {
                return RedirectToAction("Index");
            }
            Basket basket = await db.Basket.FindAsync(PurchaseId, ProductId);
            if (basket == null)
            {
                TempData["ErrorReason"] = "Could not find basket with PurchaseId = " + PurchaseId + " and ProductId = " + ProductId;
                return View("Error");
            }
            return View(basket);
        }

        // POST: Basket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long? PurchaseId, long? ProductId)
        {
            Basket basket = await db.Basket.FindAsync(PurchaseId, ProductId);
            db.Basket.Remove(basket);
            await db.SaveChangesAsync();
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
