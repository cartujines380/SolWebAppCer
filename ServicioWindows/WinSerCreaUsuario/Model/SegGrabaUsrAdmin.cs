using System;
using System.Collections.Generic;

namespace Model
{
    public class SegGrabaUsrAdmin
    {
        public string pRuc { get; set; }
        public string pNombre { get; set; }
        public string pUsuario { get; set; }
        public string pCorreoE { get; set; }
        public string pTelefono { get; set; }
        public string pCelular { get; set; }
        public string pCodSap { get; set; }
        public string pClave { get; set; }
        public string pEstado { get; set; }
        public int pIdParticipante { get; set; }
        public string pIdRepresentante { get; set; }
    }

    public class formResponseSeguridad
    {
        public formResponseSeguridad()
        {
            root = new List<Object>();
        }

        public Boolean success { get; set; }

        public string codError { get; set; }
        public string msgError { get; set; }

        public List<Object> root { get; set; }
    }

}
