using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Threading;


namespace clibCustom.Custom
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
