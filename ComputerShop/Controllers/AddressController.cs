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
    public class AddressController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Address
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can see all addresses")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Address.ToListAsync());
        }

        // GET: Address/Details/5
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can see it")]
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
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can create address")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can create address")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Street,City,Number,ZipCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Address.Add(address);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(address);
        }

        // GET: Address/Edit/5
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can edit address")]
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
            return View(address);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can edit address")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Street,City,Number,ZipCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(address);
        }

        // GET: Address/Delete/5
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can delete address")]
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
        [AuthorizeWithMessage(Roles = "Admin", ErrorMessage = "Only admin can delete address")]
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
