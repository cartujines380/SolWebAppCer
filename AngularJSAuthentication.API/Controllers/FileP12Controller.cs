using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Renci.SshNet;
using AngularJSAuthentication.API.Models;
using System.Net.Http;
using System.Net;
namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/FileP12")]
    public class FileP12Controller : ApiController
    {
        [System.Web.Http.HttpPost]
        public string UploadFile(string clave,string ruc)
        {
            string retorno = "";
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + clave));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + clave));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + clave), httpPostedFile.FileName);
                httpPostedFile.SaveAs(fileSavePath);
            
            }
            return retorno;
        }
    }
}
