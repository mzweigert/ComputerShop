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
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public async Task<ActionResult> Index()
        {
            var users = db.Users.Include(u => u.Address);
            return View(await users.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                TempData["ErrorReason"] = "Could not find user with id = " + id;
                return View("Error");
            }
            return View(user);
        }


        // GET: User/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                TempData["ErrorReason"] = "Could not find user with id = " + id;
                return View("Error");
            }
            
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AddressId,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                TempData["ErrorReason"] = "Could not find user with id = " + id;
                return View("Error");
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            User user = await db.Users.FirstOrDefaultAsync(x => x.Id == id);
            db.Users.Remove(user);
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
