using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AngularJSAuthentication.API.Handlers
{
    public class AntiForgeryValidate : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string cookieToken = "";
            string formToken = "";

            IEnumerable<string> tokenHeaders;
            if (actionContext.Request.Headers.TryGetValues("RequestVerificationToken", out tokenHeaders))
            {
                string[] tokens = tokenHeaders.First().Split(':');
                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0].Trim();
                    formToken = tokens[1].Trim();
                }
            }

            try
            {
                System.Web.Helpers.AntiForgery.Validate(cookieToken, formToken);
            }
            catch (Exception)
            {

                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    base.OnActionExecuting(actionContext);
                    return;
                }
            }



            base.OnActionExecuting(actionContext);
        }
    }
}