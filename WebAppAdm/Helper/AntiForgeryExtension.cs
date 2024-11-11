using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace WebAppMVC.Helper
{
    public static class AntiForgeryExtension
    {
    public static string RequestVerificationToken(this HtmlHelper helper)
        {
            return String.Format("ncg-request-verification-token={0}", GetTokenHeaderValue());
        }

        private static string GetTokenHeaderValue()
        {
            string cookieToken, formToken;
            System.Web.Helpers.AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }
        public static string DatedContent(this UrlHelper urlHelper, string contentPath)
        {
            var datedPath = new StringBuilder(contentPath);
            datedPath.AppendFormat("{0}m={1}",
                                   contentPath.IndexOf('?') >= 0 ? '&' : '?',
                                   getModifiedDate(contentPath));
            return urlHelper.Content(datedPath.ToString());
        }

        private static string getModifiedDate(string contentPath)
        {
            return System.IO.File.GetLastWriteTime(HostingEnvironment.MapPath(contentPath)).ToString("yyyyMMddhhmmss");
        }
    }
 }