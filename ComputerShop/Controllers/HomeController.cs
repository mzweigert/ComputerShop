using ComputerShop.Models;
using ComputerShop.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComputerShop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Author - Mateusz Zweigert®";

            return View();
        }

        
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact me.";

            return View();
        }

        //
        // GET: /Home/Search?
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Search(string phrase)
        {
            ViewBag.Phrase = phrase;

           if (phrase != null)
                using (var context = new ApplicationDbContext())
                {
                    var products = from m in context.Product
                                   where m.ProductType.Contains(phrase) || 
                                   m.ProductName.Contains(phrase) select m;
                    
                    return View(products.ToList());
                }
            else
                return RedirectToAction("Index");
           

        }

    }
}