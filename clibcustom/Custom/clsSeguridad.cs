using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using clibCustom;
using clibSeguridadCR;

namespace clibCustom
{
    public class clsSeguridad
    {

        public List<Models.segTblCatalogo> GetCatalogosFS(string PI_Session, int IdTblCatalogo)
        {
            List<Models.segTblCatalogo> Retorno = new List<Models.segTblCatalogo>();
            Models.segTblCatalogo TmpItem;
            DataSet ds = new DataSet();
            try
            {
                //clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                //ds = objSeg.ConsCatalogoFS(PI_Session, IdTblCatalogo.ToString());
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.GetCatalogosFS(PI_Session, IdTblCatalogo);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TmpItem = new Models.segTblCatalogo();
                        TmpItem.Tabla = IdTblCatalogo;
                        TmpItem.Codigo = Convert.ToString(item["Codigo"]);
                        TmpItem.Detalle = Convert.ToString(item["Descripcion"]);
                        TmpItem.Estado = Convert.ToString(item["Estado"]);
                        TmpItem.DescAlterno = Convert.ToString(item["DescAlterno"]);
                        Retorno.Add(TmpItem);
                    }
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return Retorno;
        }

        public DataSet ConsRolesPorOrgFS(string Semilla,string ValorTokenUser,int IdOrganizacion)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsRolesPorOrgFS(Semilla,ValorTokenUser, IdOrganizacion);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }

        public DataSet GetConsTodasZonas(string Semilla, int IdOrganizacion, string ValorTokenUser)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.GetConsTodasZonas(Semilla, IdOrganizacion, ValorTokenUser);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }
        public DataSet GetConsValUsrFirstLogon(string Semilla, int IdOrganizacion, string ValorTokenUser, XmlDocument xmlParam)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.GetConsValUsrFirstLogon(Semilla, IdOrganizacion, ValorTokenUser, xmlParam);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }
        public string GrabaUsuarioAdministrador(string Semilla, string ValorTokenUser, XmlDocument xmlParam)
        {
            string ds = "";
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.GrabaUsuarioAdministrador(Semilla, ValorTokenUser, xmlParam);
            }
            catch (Exception ex)
            {
                ds = ex.Message;
            }
            return ds;
        }
        public string GrabaActivacionNuevoUsuario(string Semilla, string ValorTokenUser, XmlDocument xmlParam)
        {
            string ds = "";
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.GrabaActivacionNuevoUsuario(Semilla, ValorTokenUser, xmlParam);
            }
            catch (Exception ex)
            {
                ds = ex.Message;
            }
            return ds;
        }
        public string CambiarClaveRecupera(string Semilla,string ValorTokenUser, string Usuario, string Clave, string Ruc)
        {
            string ds = "";
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.CambiarClaveRecupera(Semilla, ValorTokenUser, Usuario, Clave, Ruc);
            }
            catch (Exception ex)
            {
                ds = ex.Message;
            }
            return ds;

        }
        public string DesbloquearClave(string Semilla,string ValorTokenUser,string Usuario,Boolean bandera,string Clave,string Ruc)
        {
            string ds = "";
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.DesbloquearClave(Semilla,ValorTokenUser, Usuario, bandera, Clave, Ruc);
            }
            catch (Exception ex)
            {
                ds = ex.Message;
            }
            return ds;
        }
        public DataSet ConsInformacionSeguraUsuarioFS(string Semilla, string Ruc, string PI_Session, string pUsuario)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsInformacionSeguraUsuarioFS(Semilla, Ruc, PI_Session, pUsuario);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }
        public DataSet GetRecuperaClaveValidar(string Semilla, int IdOrganizacion, string PI_Session, XmlDocument xmlParam)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.GetRecuperaClaveValidar(Semilla, IdOrganizacion, PI_Session, xmlParam);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }
        public string CambiarClave(string Semilla, string ValorTokenUser, string Usuario, string ClaveAct, string ClaveNew, string Ruc)
        {
            string ds = "";
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.CambiarClave(Semilla,ValorTokenUser, Usuario, ClaveAct, ClaveNew, Ruc);
            }
            catch (Exception ex)
            {
                ds = ex.Message;
            }
            return ds;
        }
        public string GrabaUsuarioAdicional(string Semilla, string ValorTokenUser,string xmlParam)
        {
            string ds="";
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.GrabaUsuarioAdicional(Semilla, ValorTokenUser, xmlParam);
            }
            catch (Exception ex)
            {
                ds = ex.Message;
            }
            return ds;

        }
        public DataSet ConsRolesPorUsuarioFS(string Semilla, string ValorTokenUser, string Usuario, string Ruc)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsRolesPorUsuarioFS(Semilla, ValorTokenUser, Usuario, Ruc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }

        public DataSet ConsRolesPorListaUsuariosFS(string ValorTokenUser, string[] usuarios, string Ruc)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsRolesPorListaUsuariosFS(ValorTokenUser, usuarios, Ruc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }
        public DataSet ConsultaBandejaUsuariosAdministradores(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds = new DataSet();
            try
            {
                //clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                //objSeg.SetSemilla(PI_Semilla);
                //objSeg.IdOrganizacion = 39;
                //objSeg.IdTransaccion = 2;
                //objSeg.IdOpcion = 1;
                //objSeg.ArrParams = new Object[1] {
                //    PI_XmlDoc.OuterXml
                //};
                //ds = objSeg.EjecutaTransaccionDS(PI_Session);
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsultaBandejaUsuariosAdministradores(PI_Semilla, PI_Session, PI_XmlDoc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }

        public DataSet ConsultaBandejaUsuariosAdicionales(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsultaBandejaUsuariosAdicionales(PI_Semilla, PI_Session, PI_XmlDoc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }

        public DataSet ConsultaDatosUsuarioAdministrador(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsultaDatosUsuarioAdministrador(PI_Semilla, PI_Session, PI_XmlDoc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }

        public DataSet ConsultaDatosUsuarioAdicional(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsultaDatosUsuarioAdicional(PI_Semilla, PI_Session, PI_XmlDoc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }
        public DataSet ConsultaActivarUsuario(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsultaActivarUsuario(PI_Semilla, PI_Session, PI_XmlDoc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }
        //Agregado el 16-01-2016 por J. Navarrete
        public DataSet ConsultaDatosLegacyAsociados(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ConsultaDatosLegacyAsociados(PI_Semilla, PI_Session, PI_XmlDoc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }

        //Agregado el 16-01-2016 por J. Navarrete
        public DataSet ActualizaDatosLegacyAsociados(string PI_Semilla, string PI_Session, XmlDocument PI_XmlDoc)
        {
            DataSet ds = new DataSet();
            try
            {
                clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.ActualizaDatosLegacyAsociados(PI_Semilla, PI_Session, PI_XmlDoc);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }
        
    }
}
