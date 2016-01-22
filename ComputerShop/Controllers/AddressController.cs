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
using Microsoft.AspNet.Identity;
using ComputerShop.Validators;

namespace ComputerShop.Controllers
{
    [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can be here")]
    public class AddressController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Address
        public async Task<ActionResult> Index()
        {
            var address = db.Address.Include(a => a.User);
            return View(await address.ToListAsync());
        }

        // GET: Address/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Address address = await db.Address.FindAsync(id);
            if (address == null)
            {
                TempData["ErrorReason"] = "Could not find address with id = " + id;
                return View("Error");
            }
            return View(address);
        }

        // GET: Address/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName");
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,Street,City,Number,ZipCode")] Address address)
        {
            if (ModelState.IsValid)
            {

                if (db.Address.Find(address.UserId) == null)
                {
                    db.Address.Add(address);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");

                }
                else
                {
                    ModelState.AddModelError("", "Address with id = " + address.UserId + " already exist in database");
                }
                
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", address.UserId);
            return View(address);
        }

        // GET: Address/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Address address = await db.Address.FindAsync(id);
            if (address == null)
            {
                TempData["ErrorReason"] = "Could not find address with id = " + id;
                return View("Error");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", address.UserId);
            return View(address);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,Street,City,Number,ZipCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "UserName", address.UserId);
            return View(address);
        }

        // GET: Address/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Address address = await db.Address.FindAsync(id);
            if (address == null)
            {
                TempData["ErrorReason"] = "Could not find address with id = " + id;
                return View("Error");
            }
            return View(address);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Address address = await db.Address.FindAsync(id);
            db.Address.Remove(address);
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
