using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSProcesoBase.Model
{
    public class Tra_SolicitudCita
    {
        public string idconsolidacion { get; set; }
        public string emision { get; set; }
        public string almacensolicitante { get; set; }
        public string almacendestino { get; set; }
        public string estadoconsolidacion { get; set; }
        public string caducidadsolicitud { get; set; }
        public string vehiculo { get; set; }
        public string chofer { get; set; }
        public string asistente { get; set; }
        public string variacita { get; set; }
        public string citarapida { get; set; }

    }
    public class cls_Stock
    {
        public string BASE_UOM { get; set; }
        public string DATE { get; set; }
        public string LIFNR { get; set; }
        public string MATNR { get; set; }
        public string STOCK { get; set; }
        public string WERKS { get; set; }
        public string ZIDNLF { get; set; }
    }
    public class FormResponse
    {
        public FormResponse()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }
}