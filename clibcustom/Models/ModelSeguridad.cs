using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace clibCustom.Models
{


    static public class seg_Complexity
    {
        static public bool CheckPasswordComplexity(string Password, ref string Error)
        {
            Error = "";
            //Mínimo 8 caracteres
            if (Password.Length < 8)
            {
                Error = "Longitud es menor a 8 caracteres.";
                return false;
            }
            //Empezar con caracter alfabético
            if (!Char.IsLetter(Password, 0))
            {
                Error = "Primer caracter no es alfabético.";
                return false;
            }
            int totDigitC = 0;
            int totUpperC = 0;
            int totLowerC = 0;
            foreach (char part in Password.ToCharArray())
            {
                if (char.IsDigit(part))
                    totDigitC++;
                else if (char.IsUpper(part))
                    totUpperC++;
                else if (char.IsLower(part))
                    totLowerC++;
            }
            //Al menos un caracter en mayúscula
            if (totUpperC < 1)
            {
                Error = "No hay un caracter en mayúscula.";
                return false;
            }
            //Al menos un caracter en minúscula
            if (totLowerC < 1)
            {
                Error = "No hay un caracter en minúscula.";
                return false;
            }
            //Al menos un caracter numérico
            if (totDigitC < 1)
            {
                Error = "No hay un caracter numérico.";
                return false;
            }
            return true;
        }
    
        static public bool CheckUserComplexity(string User, ref string Error)
        {
            Error = "";
            //Mínimo 5 caracteres
            if (User.Length < 5)
            {
                Error = "Longitud es menor a 5 caracteres.";
                return false;
            }
            //Empezar con caracter alfabético
            if (!Char.IsLetter(User, 0))
            {
                Error = "Primer caracter no es alfabético.";
                return false;
            }
            int totDigitC = 0;
            int totUpperC = 0;
            int totLowerC = 0;
            foreach (char part in User.ToCharArray())
            {
                if (char.IsDigit(part))
                    totDigitC++;
                else if (char.IsUpper(part))
                    totUpperC++;
                else if (char.IsLower(part))
                    totLowerC++;
            }
            //No debe existir ni espacios ni caracteres especiales
            if (totDigitC + totUpperC + totLowerC != User.Length)
            {
                Error = "Solo se aceptan caracteres alfabéticos y numéricos.";
                return false;
            }
            return true;
        }
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



    public class segTblCatalogo
    {
        public int Tabla { get; set; }
        public string Codigo { get; set; }
        public string Detalle { get; set; }
        public string Estado { get; set; }
        public string DescAlterno { get; set; }
    }



    public class seg_ConsBandejaUsrAdmin
    {
        public string ruc { get; set; }
        public string codSAP { get; set; }
        public string razonSocial { get; set; }
        public string correoE { get; set; }
        public string telefono { get; set; }
        public string celular { get; set; }
        public string usuario { get; set; }
        public string estado { get; set; }
        public int idParticipante { get; set; }
        public string IdRepresentante { get; set; }
    }

    public class seg_GrabaUsrAdmin
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

    //Agregado el 16-01-2016 por J. Navarrete
    //Para la consulta y modificación de usuarios asociados - codigos legacys
    public class seg_ConsDatosLegAsociados
    {
        public string pRuc { get; set; }
        public string pUsuario { get; set; }
        public string pCedula { get; set; }
        public string pNombres { get; set; }
        public string pApellidos { get; set; }
        public string pCodLegacy { get; set; }
        public string pUserLegacy { get; set; }
        public string pCorreo { get; set; }
        public string pCelular { get; set; }
        public Boolean prolAdmin { get; set; }
        public Boolean prolContable { get; set; }
        public Boolean prolLogistico { get; set; }
        public Boolean prolComercial { get; set; }
    }

    public class seg_ConsUsrFirstLogon
    {
        public string ruc { get; set; }
        public string usuario { get; set; }
        public int idParticipante { get; set; }
        public string codSAP { get; set; }
        public string identReprLegal { get; set; }
        public string correoE { get; set; }
        public string celular { get; set; }
        public string telefono { get; set; }
        public string pNomComercial { get; set; }
    }

    public class seg_GrabaUsrFirstLogon
    {
        public seg_GrabaUsrFirstLogonItem pDatosUsr { get; set; }
        public seg_RespuestasSeguridad[] pRespSeg { get; set; }

        public class seg_GrabaUsrFirstLogonItem
        {
            public string pRuc { get; set; }
            public string pUsuario { get; set; }
            public int pIdParticipante { get; set; }
            public string pCorreoE { get; set; }
            public string pCelular { get; set; }
            public string pTelefono { get; set; }
            public string pCodImgSegura { get; set; }
            public string pClaveNew { get; set; }
            public string pNombre { get; set; }
            public string pNomComercial { get; set; }
        }

        public class seg_RespuestasSeguridad
        {
            public string pCodigo { get; set; }
            public string pPregunta { get; set; }
            public string pRespuesta { get; set; }
        }

    }



    public class seg_ConsBandejaUsrAdic
    {
        public string idRol { get; set; }
        public string rol { get; set; }
        public string identificacion { get; set; }
        public string usuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correoE { get; set; }
        public string celular { get; set; }
        public string usrAutorizador { get; set; }
        public string estado { get; set; }
        public string clave { get; set; }
        public Boolean recActas { get; set; }
        public Boolean rolAdmin { get; set; }
        public Boolean rolComercial { get; set; }
       

    }

   

    public class seg_GrabaUsrAdic
    {
        public string pRuc { get; set; }
        public string pUsuario { get; set; }
        public string pCorreoE { get; set; }
        public string pTelefono { get; set; }
        public string pCelular { get; set; }
        public string pCodSap { get; set; }
        public string pClave { get; set; }
        public string pEstado { get; set; }
        public int pIdParticipante { get; set; }
        public string pTipoIdent { get; set; }
        public string pIdentificacion { get; set; }
        public string pApellido1 { get; set; }
        public string pApellido2 { get; set; }
        public string pNombre1 { get; set; }
        public string pNombre2 { get; set; }
        public string pEstadoCivil { get; set; }
        public string pGenero { get; set; }
        public string pPais { get; set; }
        public string pProvincia { get; set; }
        public string pCiudad { get; set; }
        public string pDireccion { get; set; }
        public string pDepartamento { get; set; }
        public string pFuncion { get; set; }
        public Boolean pRecibeActa { get; set; }
        public string pRazonSocial { get; set; }
        public string pApoderado { get; set; }

        public seg_UsrAdicZona[] pListZona { get; set; }
        public seg_UsrAdicRol[] pListRol { get; set; }
        public seg_UsrAdicAlmacen[] pListAlm { get; set; }

        public class seg_UsrAdicZona
        {
            public string pCodZona { get; set; }
            public string pDescripcion { get; set; }
        }

        public class seg_UsrAdicAlmacen
        {
            public string pCodAlmacen { get; set; }
            public string pNomAlmacen { get; set; }
            public string pCodCiudad { get; set; }
            
        }

        public class seg_UsrAdicRol
        {
            public string pIdRol { get; set; }
            public string pDescripcion { get; set; }
            public string pDescripcionRol { get; set; }
        }
    }



}


