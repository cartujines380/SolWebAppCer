using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API
{
	public static class Utility {
		public static string AbsoluteUrl(string relativeUrl) {
			if (string.IsNullOrEmpty(relativeUrl))
				return relativeUrl;

			var httpContext = HttpContext.Current;
			if (httpContext == null)
				return relativeUrl;

			if (relativeUrl.StartsWith("/"))
				relativeUrl = relativeUrl.Insert(0, "~");
			if (!relativeUrl.StartsWith("~/"))
				relativeUrl = relativeUrl.Insert(0, "~/");

			var url = httpContext.Request.Url;
			var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

			return String.Format("{0}://{1}{2}{3}",
				url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
		}
        public static string AbsoluteUrlRef(string relativeUrl)
        {
            

            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            var httpContext = HttpContext.Current;
            if (httpContext == null)
                return relativeUrl;

            //if (relativeUrl.StartsWith("/"))
            //    relativeUrl = relativeUrl.Insert(0, "~");
            //if (!relativeUrl.StartsWith("~/"))
            //    relativeUrl = relativeUrl.Insert(0, "~/");

            var url = httpContext.Request.UrlReferrer;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            var virtpath = relativeUrl;// VirtualPathUtility.ToAbsolute(relativeUrl);
            var result = String.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, virtpath);

            return result;
        }
	}
}