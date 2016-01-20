using ComputerShop.Managers;
using ComputerShop.Models;
using ComputerShop.Validators;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ComputerShop.Controllers
{
    [AuthorizeWithMessage(ErrorMessage = "You must be logged in")]
    public class MyOrdersController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public MyOrdersController() { }
        public MyOrdersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

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

        public ActionResult Index()
        {
            long UserId = User.Identity.GetUserId<long>();
            Purchase userPurchase = (from p in db.Purchase
                                    where p.UserId == UserId &&
                                    p.IsConfirmed == false
                                    select p).FirstOrDefault();

            return View(userPurchase);

        }
        public async Task<ActionResult> Edit(long? PurchaseId, long? ProductId)
        {
            if (PurchaseId == null || ProductId == null)
            {
                return View("Error");
            }
            long UserId = User.Identity.GetUserId<long>();
            Basket basket = await db.Basket.FindAsync(PurchaseId, ProductId);

            if(basket != null)
            {
                if(basket.Purchase.UserId == UserId)
                {
                    return View(basket);
                }
            }

            return View("Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PurchaseId,ProductId,Quantity")]Basket basket)
        {
            long UserId = User.Identity.GetUserId<long>();
            Basket basketExist = await db.Basket.FindAsync(basket.PurchaseId, basket.ProductId);
            if(basketExist != null)
            {
                if (basketExist.Purchase.UserId == UserId)
                {
                    if (ModelState.IsValid)
                    {
                        basketExist.Quantity = basket.Quantity;
                        db.Entry(basketExist).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    return View(basketExist);
                }
            }
            
            ModelState.AddModelError(string.Empty, "Illegal Operation.");
            return View(basketExist);

        }

        public async Task<ActionResult> Delete(long? PurchaseId, long? ProductId)
        {
            if (PurchaseId == null || ProductId == null)
            {
                return View("Error");
            }
            long UserId = User.Identity.GetUserId<long>();
            Basket basket = await db.Basket.FindAsync(PurchaseId, ProductId);
            if (basket != null)
            {
                if (basket.Purchase.UserId == UserId)
                {
                    return View(basket);
                }
                
            }
            return View("Error");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "PurchaseId,ProductId")]Basket basket)
        {
            long UserId = User.Identity.GetUserId<long>();
            Basket basketUser = await db.Basket.FindAsync(basket.PurchaseId, basket.ProductId);
            if(basketUser != null)
            {
                if (basketUser.Purchase.UserId == UserId)
                {
                    db.Basket.Remove(basketUser);

                    Purchase purchase = await db.Purchase.FindAsync(basket.PurchaseId);
                    if (purchase.Baskets.Count == 0)
                    {
                        db.Purchase.Remove(purchase);
                    }
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
           
            }

            ModelState.AddModelError(string.Empty, "Illegal Operation.");
            return View(basket);
        }
        
    }
}