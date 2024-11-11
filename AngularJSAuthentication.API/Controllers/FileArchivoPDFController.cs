using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Renci.SshNet;
using AngularJSAuthentication.API.Models;
using System.Net.Http;
using System.Net;
using System.Data;
using clibProveedores;
using System.Xml;
namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/FileArchivoPDF")]
    public class FileArchivoPDFController : ApiController
    {
        [System.Web.Http.HttpPost]
        public void UploadFilePDF(string archivo)
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                if (httpPostedFile.FileName == "PREVISUALIZACION.PDF")
                {
                    archivo = "PREVISUALIZACION.PDF";
                    var fileSavePath2 = Path.Combine(AppConfig.archivoPDFPreliminar, archivo);
                    httpPostedFile.SaveAs(fileSavePath2);
                }

                bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath("~/UploadedDocuments/"));
                if (!folderExists)
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/UploadedDocuments/"));
                var fileSavePath = Path.Combine(AppConfig.archivoPDF, archivo);
                httpPostedFile.SaveAs(fileSavePath);


            }
        }

        [ActionName("cargaArchivoPDF")]
        [HttpGet]
        public int cargaArchivoPDF(string archivoFinal)
        {
            var fileSavePath = Path.Combine(AppConfig.archivoPDF, archivoFinal);
            try
            {
                File.Delete(fileSavePath);
            }
            catch (Exception ex)
            { }
            var archivo = "PREVISUALIZACION.PDF";
            var fileSavePath2 = Path.Combine(AppConfig.archivoPDF, archivo);
            try
            {
                File.Move(@fileSavePath2, @fileSavePath);
            }
            catch (Exception ex)
            { }
            return 1;
        }
    }
}