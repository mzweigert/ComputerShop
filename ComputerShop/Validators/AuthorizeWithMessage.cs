using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComputerShop.Validators
{
    public class AuthorizeWithMessage : AuthorizeAttribute
    {
        public string Message { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var result = new ViewResult();
            result.ViewName = "~/Views/Account/Login.cshtml";
            result.MasterName = "~/Views/Shared/_Layout.cshtml";
            result.ViewBag.Message = this.Message;

            filterContext.Result = result;

        }
    }
}