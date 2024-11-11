using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WSArchivoP12
{
    /// <summary>
    /// Descripción breve de ServArchivoP12
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ServArchivoP12 : System.Web.Services.WebService
    {

        [WebMethod]
        public string archivoP12(byte[] nombreArchivo, string ruc, string clave)
        {
            string retorno = "";
            try
            {

                clibFirmador.clsEstablecimiento objFirmador=new clibFirmador.clsEstablecimiento(ruc, new MemoryStream(nombreArchivo), clave);
             
                retorno = objFirmador.lv_FechaExpiracion_Desde.ToString("dd/MM/yyyy") + "," + objFirmador.lv_FechaExpiracion_Hasta.ToString("dd/MM/yyyy");

            }
            catch (Exception)
            {

                throw;
            }


            return retorno;
        }

          [WebMethod]
        public string archivoFimar(string PI_ArchivoOrigen,string PI_ArchivoDestino,byte[] nombreArchivo, string clave)
        {
            string retorno = "";
           
            return retorno;
              
        }
    }
}
