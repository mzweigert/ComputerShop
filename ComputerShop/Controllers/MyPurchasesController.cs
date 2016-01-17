using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComputerShop.Controllers
{
    public class MyPurchasesController : Controller
    {
        // GET: MyPurchases
        public ActionResult Index()
        {
            return View();
        }
    }
}