using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WCFEnvioCorreo.Model
{
    public class FormResponseModelo
    {
        public FormResponseModelo()
        {
            root = new List<Object>();
        }

        public string codError { get; set; }
        public string msgError { get; set; }

        public Boolean success { get; set; }

        public List<Object> root { get; set; }
    }
}
