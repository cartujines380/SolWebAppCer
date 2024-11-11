using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Models
{
    public class DMEtiqueta
    {

        public EtiProveedor[] p_EtiProveedor { get; set; }
        

     

        public class EtiProveedor
        {

            public string CodProveedor { get; set; }
            public string Ruc { get; set; }
            public string NombreComercial { get; set; }
            public Boolean GeneraEtiqueta { get; set; }
            
        }


    }
   

    public class FormResponseEtiqueta
    {
        public FormResponseEtiqueta()
        {
            root = new List<Object>();

        }


        public Boolean success { get; set; }
        public String mensaje { get; set; }
        public List<Object> root { get; set; }

    }
}