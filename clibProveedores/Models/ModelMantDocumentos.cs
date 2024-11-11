using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJSAuthentication.API.Models
{
    public class Documentos
    {
        public Int64 IdDocumentos { get; set; }
        public string CodTipoPersona { get; set; }
        public string NomTipoPersona { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string EsObligatorio { get; set; }
        public string UsuarioCreacion { get; set; }
        public string FechaRegistro { get; set; }
        public string Estado { get; set; }
    }

    public class FormResponseDocumentos
    {
        public FormResponseDocumentos()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }
        public Boolean success { get; set; }
        public List<Object> root { get; set; }
    }
}
