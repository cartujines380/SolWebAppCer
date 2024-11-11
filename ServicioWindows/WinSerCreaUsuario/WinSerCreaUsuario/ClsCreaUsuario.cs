using Model;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
namespace WinSerCreaUsuario
{

    public class ClsCreaUsuario
    {

        #region ==========================> Variables

        private readonly string _clase;

        private readonly string _conexion;
        private readonly string _semilla;
        private readonly string _session;

        public static async Task EjecutarJob()
        {
            var oBusiness = new ClsCreaUsuario();
            var result = await Task.Run(() =>
            {
                Console.WriteLine("Ejecucion");
                oBusiness.Procesa();
                return true;
            });
        }

        #endregion =======================> Variables

        public ClsCreaUsuario() {
            _clase = this.GetType().Name;
            _conexion = clsGlobal.CadenaConexion;
            _semilla = clsGlobal._Semilla;
            _session = clsGlobal._PI_Session;
        }

        #region ==========================> Metodos
        public void Procesa()
        {
            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(" =====> " + _clase + " " + metodo + " INI ");
            #endregion Log

            DataSet DSResult;

            try
            {
                string UrlPortal = System.Configuration.ConfigurationManager.AppSettings["UrlPortal"];
                p_Log.Graba_Log_Info(_clase + " " + metodo + " INFO: Variables INI ==================================>");
                p_Log.Graba_Log_Info(_clase + " " + metodo + " INFO: UrlPortal: " + UrlPortal);
                var rutaArchivo = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoNewUsr"]).Trim();
                p_Log.Graba_Log_Info(_clase + " " + metodo + " INFO: rutaArchivo: " + rutaArchivo);
                p_Log.Graba_Log_Info(_clase + " " + metodo + " INFO: Variables FIN ==================================>");

                if ( !File.Exists(rutaArchivo))
                {
                    p_Log.Graba_Log_Info(_clase + " " + metodo + " ADVERTENCIA:  El archivo " + rutaArchivo + " no existe.");
                    p_Log.Graba_Log_Info(_clase + " " + metodo + " ADVERTENCIA:  No se enviará el adjunto en el Correo. Favor de cargar el archivo " + rutaArchivo);
                }


                DSResult = Obtenerdatos();
                if (DSResult.Tables.Count != 0 && DSResult.Tables[0].Rows.Count > 0)
                {
                    var tableResultado = DSResult.Tables[0];
                    p_Log.Graba_Log_Info(_clase + " " + metodo + " Cantidad: " + tableResultado.Rows.Count);

                    for (var i = 0; i < tableResultado.Rows.Count; i++)
                    {
                        var item = new SegGrabaUsrAdmin();
                        item.pRuc = tableResultado.Rows[i]["Ruc"].ToString();
                        item.pNombre = tableResultado.Rows[i]["RazonSocial"].ToString();
                        item.pTelefono = tableResultado.Rows[i]["Telefono"].ToString();
                        item.pCelular = tableResultado.Rows[i]["Celular"].ToString();
                        item.pCorreoE = tableResultado.Rows[i]["CorreoE"].ToString();
                        item.pCodSap = tableResultado.Rows[i]["CodSAP"].ToString();
                        item.pClave = tableResultado.Rows[i]["Clave"].ToString();

                        item.pEstado = tableResultado.Rows[i]["Estado"].ToString();
                        item.pIdRepresentante = tableResultado.Rows[i]["IdRepresentante"].ToString();
                        
                        GetGrabarUsrAdmin(item);
                    }
                }
                else
                {
                    p_Log.Graba_Log_Info(_clase + " " + metodo + " : *** No hay datos.");
                }

            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " ERROR.: " + ex.Message);
            }

            p_Log.Graba_Log_Info(" =====> " + _clase + " " + metodo + " FIN ");
        }

        public void GetGrabarUsrAdmin(SegGrabaUsrAdmin userModel)
        {
            Model.formResponseSeguridad FormResponse = new Model.formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            XmlDocument xmlParam = new XmlDocument();
            XmlElement xEl1;
            XmlDocument xmlResp = new XmlDocument();

            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            System.Collections.Generic.Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + metodo + " INI ");
            #endregion Log

            try
            {
                xmlParam.LoadXml("<Root />");
                // INI - armado de datos
                string opcion   = "New";

                userModel.pClave = generarPassword(8, 0 , "");

                xEl1 = xmlParam.CreateElement(opcion);
                xEl1.SetAttribute("IdEmpresa", "1");
                xEl1.SetAttribute("Ruc", userModel.pRuc);
                
                xEl1.SetAttribute("CodProveedor", userModel.pCodSap);
                xEl1.SetAttribute("CorreoE", userModel.pCorreoE);
                xEl1.SetAttribute("Telefono", userModel.pTelefono);
                xEl1.SetAttribute("Celular", userModel.pCelular);

                xEl1.SetAttribute("UsrAutorizador", "Adm_Service");

                xEl1.SetAttribute("Estado", userModel.pEstado);
                if (opcion == "New")
                {
                    xEl1.SetAttribute("Clave", userModel.pClave);
                    xEl1.SetAttribute("Nombre", userModel.pNombre);
                }
                XmlElement xRol;
                xRol = xmlParam.CreateElement("Rol");
                xRol.SetAttribute("IdRol", "3"); //rol default para usuarios que pueden hacer logon
                xEl1.AppendChild(xRol);
                xRol = xmlParam.CreateElement("Rol");
                xRol.SetAttribute("IdRol", "21"); //rol con Permisos de Usuarios Proveedores Básicos
                xEl1.AppendChild(xRol);
                xRol = xmlParam.CreateElement("Rol");
                //xRol.SetAttribute("IdRol", "23"); //rol con Permisos de Usuarios Proveedores Administradores
                xRol.SetAttribute("IdRol", "24"); //rol con Permisos de Usuarios Proveedores Administradores
                xEl1.AppendChild(xRol);
                xmlParam.DocumentElement.AppendChild(xEl1);
                // FIN - armado de datos
                xmlResp.LoadXml(GrabaUsuarioAdministrador(xmlParam));
                if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                {

                    string asuntoEmail = "";
                    if (opcion == "New")
                    {
                        userModel.pUsuario = xmlResp.DocumentElement.GetAttribute("Usuario");
                        userModel.pIdParticipante = Int32.Parse(xmlResp.DocumentElement.GetAttribute("IdParticipante"));
                        PI_NombrePlantilla = "NewUserAdmin.html";
                        asuntoEmail = "Nuevo Usuario - Portal de Proveedores";               
                    }

                    p_Log.Graba_Log_Info(_clase + " " + metodo + "========> USUARIO: " + userModel.pUsuario  + " CREADO.");


                    FormResponse.success = true;
                    FormResponse.root.Add(userModel);
                    
                    if (FormResponse.success && userModel.pClave != "")
                    {
                        clibSeguridad.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibSeguridad.WCFEnvioCorreo.ServEnvioClientSoapClient();

                        p_Log.Graba_Log_Info(_clase + " " + metodo + "========> Datos Usuario: " + userModel.pUsuario + " Ruc: " + userModel.pRuc
                            + " CodSap: " + userModel.pCodSap + " NombreRazonSocial:" + userModel.pNombre + " ==>FIN");


                        #region RFD0-2022-155 CORREO
                        Variables = new System.Collections.Generic.Dictionary<string, string>();
                        Variables.Add("@@Ruc", userModel.pRuc);
                        Variables.Add("@@CodSap", userModel.pCodSap);
                        Variables.Add("@@NombreRazonSocial", userModel.pNombre);
                        Variables.Add("@@Usuario", userModel.pUsuario);
                        Variables.Add("@@Clave", userModel.pClave);

                        //SE AGREGA URL PORTAL
                        string UrlPortal = System.Configuration.ConfigurationManager.AppSettings["UrlPortal"];
                        Variables.Add("@@UrlPortal", UrlPortal);

                        #endregion


                        if (userModel.pIdRepresentante != "")
                        {
                            Variables.Add("@@IdRepresentante", userModel.pIdRepresentante);
                        }
                        else
                        {
                            Variables.Add("@@IdRepresentante", userModel.pRuc);
                        }

                        var rutaArchivo = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaArchivoNewUsr"]).Trim();

                        string FileName = String.Empty;
                        byte[] data = null;
                        Boolean TieneAdjunto = true;
                        if (System.IO.File.Exists(rutaArchivo))
                        {
                            var nomArchivo = rutaArchivo.Split('\\');
                            FileInfo fInfo = new FileInfo(rutaArchivo);
                            long numBytes = fInfo.Length;
                            FileStream fStream = new FileStream(rutaArchivo,
                            FileMode.Open, FileAccess.Read);
                            BinaryReader br = new BinaryReader(fStream);
                            // convert the file to a byte array
                            data = br.ReadBytes((int)numBytes);

                            FileName = nomArchivo[nomArchivo.Length - 1];
                        }
                        else 
                        {
                            data = System.Text.Encoding.ASCII.GetBytes("TEST");
                            TieneAdjunto = false;
                            p_Log.Graba_Log_Info(_clase + " " + metodo + " INFO: Archivo no existe o no se tiene permiso. Adjunto no enviado.");

                        }

                        PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();
                        p_Log.Graba_Log_Info(_clase + " " + metodo + " INFO: Mail-Endpoint: " + objEnvMail.Endpoint.Address);
                        
                        string retorno = objEnvMail.EnviaCorreoDF("", userModel.pCorreoE, "", "", asuntoEmail, "", true, true, TieneAdjunto,
                            data, FileName
                            , PI_NombrePlantilla, PI_Variables);

                        if (retorno != "")
                        {
                            FormResponse.success = false;
                            FormResponse.codError = "-100";
                            FormResponse.msgError = "Los datos fueron actualizados pero se falló al enviar el correo con el ERROR: " + retorno;
                        }
                    }
                }
                else
                {
                    p_Log.Graba_Log_Info(_clase + " " + metodo + " ERROR al Grabar Usuario Administrador: " +
                        xmlResp.DocumentElement.GetAttribute("MsgError"));
                }
            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " ERROR.: " + ex.Message);
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " FIN ");

            }
        }

        private DataSet Obtenerdatos()
        {
            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + metodo + " INI ");
            #endregion Log
            DataSet dtRetorno;
            XmlDocument ListaRuc = new XmlDocument();
            try
            {
                string BanderaConsulta = System.Configuration.ConfigurationManager.AppSettings["ConsultaxRuc"];
                string V_ListaRuc = String.Empty;

                if (BanderaConsulta == "S")
                {
                    p_Log.Graba_Log_Info(_clase + " " + metodo + " INFO: -----Consulta por Ruc.-----");

                    V_ListaRuc = System.Configuration.ConfigurationManager.AppSettings["ListaRuc"];
                    p_Log.Graba_Log_Info(_clase + " " + metodo + " INFO: -----Ruc: " + V_ListaRuc);

                    if (!string.IsNullOrEmpty(V_ListaRuc))
                    {
                        string[] ArrayListaRuc = V_ListaRuc.Split(';');
                        if (ArrayListaRuc.Length > 0)
                        {
                            ListaRuc = ArmaDataRuc(ArrayListaRuc);
                        }
                    }
                    
                }
                p_Log.Graba_Log_Info(_clase + " " + metodo + " Conn: " + _conexion);

                PP.AccesoDatos.OperadorBaseDatos op = new PP.AccesoDatos.OperadorBaseDatos(_conexion);
                op.ProcedimientoAlmacenado = "[Seguridad].[Seg_P_ConsProveedoresPendiente]";
                op.AgregarParametro("@PI_ParamXML", SqlDbType.Xml, ListaRuc.OuterXml);
                op.AgregarParametro("@V_Bandera", SqlDbType.VarChar, BanderaConsulta);

                p_Log.Graba_Log_Info(_clase + " " + metodo + " ****Inicia ConsultarDataSet");
                dtRetorno = op.ConsultarDataSet();
                p_Log.Graba_Log_Info(_clase + " " + metodo + " ****Fin ConsultarDataSet");

                return dtRetorno;
            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " ERROR.: " + ex.Message);
                throw;            
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " FIN ");

            }
        }

        private XmlDocument ArmaDataRuc(string[] ListaRuc)
        {
            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + metodo + " INI ");
            #endregion Log


            XmlDocument doc = new XmlDocument();

            try
            {
                XmlElement cuerpo = doc.CreateElement(string.Empty, "Root", string.Empty);
                doc.AppendChild(cuerpo);
                foreach (var x in ListaRuc)
                {
                    XmlElement ventas = doc.CreateElement(string.Empty, "Ventas", string.Empty);
                    cuerpo.AppendChild(ventas);
                    ventas.SetAttribute("Ruc",x);
                }

                return doc;

            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " ERROR.: " + ex.Message);
                throw;
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " FIN ");

            }

        }


        private string GrabaUsuarioAdministrador(XmlDocument PI_XmlDoc)
        {
            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + metodo + " INI ");
            #endregion Log
        
            XmlDocument xmlResul = new XmlDocument();

            try
            {
                clibSeguridad.clsClienteSeg model = new clibSeguridad.clsClienteSeg();
                model.pCadenaBD = _conexion;
                model.pSemilla = _semilla;
                model.pSession = _session;
                xmlResul = model.GrabaUsuarioAdministrador(PI_XmlDoc.OuterXml);
            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " ERROR.: " + ex.Message);

            }
            p_Log.Graba_Log_Info(_clase + " " + metodo + " FIN ");

            return xmlResul.OuterXml;

        }

        #endregion ========================> Metodos


        //int largo, int numEsp, string listaCarEsp
        private string generarPassword(int largo, int numEsp, string listaCarEsp)
        {
            var Resultado = "";
            var CanNum = false;
            var CanLet = false;
            var idx = 0;
            //Cargamos la matriz con números y letras
            string[] Caracter = {
                "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "a",
                "b",
                "c",
                "d",
                "e",
                "f",
                "g",
                "h",
                "i",
                "j",
                "k",
                "l",
                "m",
                "n",
                "o",
                "p",
                "q",
                "r",
                "s",
                "t",
                "u",
                "v",
                "w",
                "x",
                "y",
                "z"
            };

            //Math.random();

            Random rnd = new Random();

            while (Resultado.Length < (largo - numEsp))
            {
                idx = rnd.Next(0, 35);
                Resultado = Resultado + Caracter[idx];
                if ((idx < 10))
                    CanNum = true;
                if ((idx > 9))
                    CanLet = true;
            }
            if (CanNum == false)
            {
                Resultado = Resultado.Substring(0, Resultado.Length - 1);
                idx = rnd.Next(0, 35);
                Resultado = Resultado + Caracter[idx];
            }
            if (CanLet == false)
            {
                Resultado = Resultado.Substring(0, Resultado.Length - 1);
                idx = rnd.Next(0, 35);
                Resultado = Resultado + Caracter[idx];
                Resultado = Resultado + Caracter[idx];
            }
            //Math.random();
            while (Resultado.Length < largo)
            {
                idx = rnd.Next(0, 35);
                Resultado = Resultado + listaCarEsp.Substring(idx, idx + 1);
            }
            if (Resultado.Length > 2)
            {
                Resultado = Resultado.Substring(1, Resultado.Length - 1) + Resultado.Substring(0, 1);
            }
            return Resultado;
        }



    }
}
