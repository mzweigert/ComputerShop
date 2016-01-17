using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ComputerShop.Validators
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeWithMessage : AuthorizeAttribute
    {
        public string ErrorMessage { get; set; }
        private bool _isAuthorized;

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            _isAuthorized = base.AuthorizeCore(httpContext);
            return _isAuthorized;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!_isAuthorized)
            {
                filterContext.Controller.TempData.Add("AuthorizationError", this.ErrorMessage);
            }
        }
    }
}