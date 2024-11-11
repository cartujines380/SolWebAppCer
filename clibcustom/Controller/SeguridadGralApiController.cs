using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AngularJSAuthentication.API.Models;

using System.Data;
using System.Xml;
using System.Security.Claims;
using clibCustom.Models;
using clibCustom;
using AngularJSAuthentication.API.Custom;
//using clibProveedores.Models;
//using clibProveedores;

 
namespace AngularJSAuthentication.API.Controllers
{

   
    [RoutePrefix("api/Seguridad")]
    public class SeguridadController : ApiController
    {


        [ActionName("Menus")]
        [HttpGet]
        public formResponseSeguridad GetMenus(string clientId, string userName)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            var identity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;
            //string clientId=string.Empty;
            //string userName = string.Empty;

            List<TransaccionUser> ListmpTrx = new List<TransaccionUser>();
            List<string> ListmpTrxstr = new List<string>();
            clibEncriptaQS.clsEncriptaQS ObjEncrip2 = new clibEncriptaQS.clsEncriptaQS();
            if(userName == null)
            {
                userName="";
            }
            if(userName.Length==64)
            {
                userName=ObjEncrip2.Decryp(userName);
            }

            ListmpTrxstr = InitialiseService.TransaccionesUsuario.FindAll(x => x.UserId == userName).Select(t => t.TransaccionId).ToList();


            ApplicationDbContext db = new ApplicationDbContext();


            string ApplicationId = string.Empty;

            //string clientId = string.Empty;
            string clientSecret = string.Empty;


            clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();
            //clientId = context.ClientId;
            var DeSecretClientId = objEnc.Desencripta(clientId, InitialiseService.Semilla);

            List<Menu> Retorno = new List<Menu>();
            Menu TmpMenu;
            Submenu TmpSubmenu;
            Item TmpItem;

            //List<Menu> menuesfiltered = new List<Menu>();
            //List<Submenu> submenuesfiltered = new List<Submenu>();
            //List<Item> itemsfiltered = new List<Item>();

            try
            {
                FormResponse.success = true;
                FormResponse.root.Add(Retorno);
            }
            catch (Exception ex)
            {
                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = ex.Message;
            }
            //FormResponse.root.Add(menuesfiltered);
            //FormResponse.root.Add(submenuesfiltered);
            //FormResponse.root.Add(itemsfiltered);
            return FormResponse;
        }



        [ActionName("CatalogosFS")]
        [HttpGet]
        public List<segTblCatalogo> GetCatalogosFS(string NombreTablaFS)
        {
            clsSeguridad ObjEjecucion = new clsSeguridad();
            List<segTblCatalogo> result = new List<segTblCatalogo>();
            switch (NombreTablaFS)
            {
                case "ConsImgSeguras":
                    result = ObjEjecucion.GetCatalogosFS(InitialiseService.PI_Session, 50);
                    break;
                case "ConsPregSeguras":
                    result = ObjEjecucion.GetCatalogosFS(InitialiseService.PI_Session, 51);
                    break;
            }
            return result;
        }

    }
}
