using System;
using System.Xml;

namespace FE.ServicioSendCorreo
{
    public static class clsGlobal
    {
        // parameterless constructor required for static class
        static clsGlobal() {  } // default value

        // public get, and private set for strict access control
        public static string CadenaConexion { get; private set; }
        public static string Msg = "";
        // GlobalInt can be changed only via this method
        public static bool  SetLoginAplicacion()
        {
            bool lv_Retorno = false;
            Msg = "";
            try
            {
                //clibClienteSegNet.clsClienteSeg objSeg = new clibClienteSegNet.clsClienteSeg();
                XmlDocument xmlResul = new XmlDocument();
                xmlResul.XmlResolver = null;
                CadenaConexion = System.Configuration.ConfigurationManager.ConnectionStrings["SqlConn"].ToString();

                clsBussinesLogic cls_bLogic = new clsBussinesLogic();
                System.Data.DataTable dtConexion = new System.Data.DataTable();
                if(cls_bLogic.BL_ConsultarConexionCorreo(ref dtConexion))
                {
                    clsEntidadCorreo.p_Log.Graba_Log_Info("SetLoginAplicacion Paso#1");

                    if (dtConexion.Rows.Count > 0)
                    {
                        clsEntidadCorreo.p_Log.Graba_Log_Info("SetLoginAplicacion Paso#2");
                        clsEntidadCorreo.intervaloServicioCorreo = int.Parse(dtConexion.Rows[0]["ServicioCorreo_intervaloCorreoEnvia"].ToString());
                        clsEntidadCorreo.maxRegistrosCorreo = Convert.ToInt32(dtConexion.Rows[0]["ServicioCorreo_maxRegistrosCorreo"]);
                        clsEntidadCorreo.maxItemsConcurrenciaCorreo = Convert.ToInt32(dtConexion.Rows[0]["ServicioCorreo_maxItemsConcurrenciaCorreo"]);
                        clsEntidadCorreo.Notificacion_ServidorSmtp = dtConexion.Rows[0]["ServidorSmtp"].ToString();
                        clsEntidadCorreo.Notificacion_PuertoSmtp = dtConexion.Rows[0]["PuertoSmtp"].ToString();
                        clsEntidadCorreo.Notificacion_UsuarioSmtp = dtConexion.Rows[0]["UsuarioSmtp"].ToString();
                        clsEntidadCorreo.Notificacion_ClaveSmtp = dtConexion.Rows[0]["ClaveSmtp"].ToString();                        
                        clsEntidadCorreo.Notificacion_SenderSmtp = dtConexion.Rows[0]["Notificacion_SenderSmtp"].ToString();
                        if (dtConexion.Rows[0]["AplicaSSL"].ToString() == "1")
                        {
                            clsEntidadCorreo.Notificacion_AplicarSSL = true;
                        }
                        else { clsEntidadCorreo.Notificacion_AplicarSSL = false; }
                    }
                }

                lv_Retorno = true;
                //xmlResul.LoadXml(objSeg.LoginAplicacion(Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IdEmpresa"]), Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IdSucursal"]), Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IdAplicacion"]), Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["IdOrganizacion"]), System.Configuration.ConfigurationManager.AppSettings["IdUsuarioApl"]));
                //if (objSeg.codError == 0)
                //{
                //    //Max Pool Size=200

                //    XmlElement XmlConexion = (XmlElement)xmlResul.SelectSingleNode("/Registro/Servidores/Servidor[@Id='" + System.Configuration.ConfigurationManager.AppSettings["IdServidorEjecutaSQL"].ToString() + "']");
                    

                //    clsEntidadCorreo.intervaloServicioCorreo = Convert.ToInt32(XmlConexion.GetAttribute("ServicioCorreo_intervaloCorreoEnvia"));
                //    clsEntidadCorreo.maxRegistrosCorreo = Convert.ToInt32(XmlConexion.GetAttribute("ServicioCorreo_maxRegistrosCorreo"));
                //    clsEntidadCorreo.maxItemsConcurrenciaCorreo = Convert.ToInt32(XmlConexion.GetAttribute("ServicioCorreo_maxItemsConcurrenciaCorreo"));
                //    clsEntidadCorreo.Notificacion_ServidorSmtp = XmlConexion.GetAttribute("ServidorSmtp");
                //    clsEntidadCorreo.Notificacion_PuertoSmtp = XmlConexion.GetAttribute("PuertoSmtp");
                //    clsEntidadCorreo.Notificacion_UsuarioSmtp = XmlConexion.GetAttribute("UsuarioSmtp");
                //    clsEntidadCorreo.Notificacion_ClaveSmtp = XmlConexion.GetAttribute("ClaveSmtp");
                //    //clsEntidadCorreo.Notificacion_EnviaSmtp = dtParametros.Select("IdParametro='CorreoDestino_PermiteEnvio'")[0]["Valor"].ToString();
                //    clsEntidadCorreo.Notificacion_SenderSmtp = XmlConexion.GetAttribute("Notificacion_SenderSmtp"); 
                //    if (XmlConexion.GetAttribute("AplicaSSL") == "1")
                //    {
                //        clsEntidadCorreo.Notificacion_AplicarSSL = true;
                //    }
                //    else { clsEntidadCorreo.Notificacion_AplicarSSL = false; }

                //    //XmlElement XmlConexion = (XmlElement)xmlResul.SelectSingleNode("/Registro/Servidores/Servidor[@Id='" + System.Configuration.ConfigurationManager.AppSettings["IdServidorEjecutaSQL"].ToString() + "']");
                //    if (XmlConexion.GetAttribute("Usuario").Trim().Equals(""))
                //    {
                //        CadenaConexion = "Data Source=" + XmlConexion.GetAttribute("Servidor") + ";Initial Catalog=" + XmlConexion.GetAttribute("BaseDatos") + ";Integrated Security=true;Max Pool Size=" + XmlConexion.GetAttribute("MaxPool");
                //    }
                //    else
                //    {
                //        CadenaConexion = "Data Source=" + XmlConexion.GetAttribute("Servidor") + ";Initial Catalog=" + XmlConexion.GetAttribute("BaseDatos") + ";User ID=" + XmlConexion.GetAttribute("Usuario") + ";Password=" + XmlConexion.GetAttribute("Clave") + ";Max Pool Size=" + XmlConexion.GetAttribute("MaxPool");
                //    }


                //    lv_Retorno = true;
                //}
                //else
                //    throw new Exception(objSeg.MsgError);
            }
            catch (Exception ex)
            {
                lv_Retorno = false;
                CadenaConexion = "";
                Msg = ex.Message;
            }

            return lv_Retorno;
        }
    }
}
