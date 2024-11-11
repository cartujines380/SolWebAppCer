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
    [RoutePrefix("api/FileTransporte")]
    public class FileTransporteController : ApiController
    {
        [System.Web.Http.HttpPost]
        public void UploadFile(string direccion)
        {
           
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + direccion));
                if (!folderExists)
                 Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + direccion));
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + direccion), httpPostedFile.FileName);
                httpPostedFile.SaveAs(fileSavePath);

           }
        }
        
    }

   
}