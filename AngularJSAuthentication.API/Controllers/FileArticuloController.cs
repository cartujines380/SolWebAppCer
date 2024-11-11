using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Renci.SshNet;
using AngularJSAuthentication.API.Models;


namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/FileArticulo")]
    public class FileArticuloController : ApiController
    {
        [System.Web.Http.HttpPost]
        public void UploadFile(string path)
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + path), httpPostedFile.FileName);
                httpPostedFile.SaveAs(fileSavePath);


            }
        }
    }
}