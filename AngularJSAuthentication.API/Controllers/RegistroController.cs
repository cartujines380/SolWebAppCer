using AngularJSAuthentication.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/Registro")]
    public class RegistroController : ApiController
    {
        private readonly string _class = "IntegrationSuccesfactor";        
        private string _method;
        public RegistroController()
        {
            _class = this.GetType().Name;
        }
        //[ActionName("RegistraProveedor")]
        //[HttpGet]
        //public FormResponseModelo RegistraProveedor()
        //{

        //}
    }
}