using ComputerShop.Models;
using ComputerShop.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComputerShop.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext context;

        public ProductController()
        {
            context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            ViewBag.AllProducts = context.Product.ToList();
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errors.Add(error.ErrorMessage);
           
                    }
                }
                TempData["Errors"] = errors;
                return RedirectToAction("Index");
            }
            
            context.Product.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}