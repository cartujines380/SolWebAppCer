using System;
using System.Configuration;
using System.Data;
using clibLogger;
using System.Reflection;
using Renci.SshNet;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace wsProcesaPagosBG
{
    internal class clsMultiplesProcesos
    {
        private string _clase;
        private string _metodo;
        private string _host;
        private string _username;
        private string _password;
        clsLogger log = new clsLogger();
        public string nArchivo { get; set; }

        public clsMultiplesProcesos()
        {
            _clase = this.GetType().Name;
            _host = clsGlobal.ServidorSFTP.Trim();
            _username = clsGlobal.UsuarioSFTP.Trim();
            _password = clsGlobal.ClaveSFTP.Trim();
        }

        public bool MP_ExtraeArchivoSFTP()
        {
            DataSet dsBitacora = new DataSet();
            DataTable dtBitacora = new DataTable();
            bool lv_Retorno = true;
            string detallesBit="";
            clsBussinesLogic bl = new clsBussinesLogic();

            dsBitacora.DataSetName = "Root";
            dsBitacora.Tables.Add(dtBitacora);
            dtBitacora.TableName = "Pagos";
            dtBitacora.Columns.Add("Tipo", typeof(string));
            dtBitacora.Columns.Add("Proceso", typeof(string));
            dtBitacora.Columns.Add("Servicio", typeof(string));
            dtBitacora.Columns.Add("Detalle", typeof(string));
            dtBitacora.Columns.Add("Accion", typeof(string));

            this._metodo = MethodBase.GetCurrentMethod().Name;
            string nomArchivo = NombreArchivo();
            this.nArchivo = nomArchivo;
            string inPath = clsGlobal.RutaSFTP.Trim();
            string outPath = ((string)ConfigurationManager.AppSettings["stRutaLocalSFTP"]).Trim();

            try
            {
                log.Graba_Log_Info(" * * * DESCARGA SFTP * * *");
                log.Graba_Log_Info("==============================");
                log.Graba_Log_Info("Clase: " + _clase + "     Metodo: " + _metodo);
                log.Graba_Log_Info("==============================");
                log.Graba_Log_Info("SFTP: " + _host);

                using (SftpClient client = new SftpClient(_host, _username, _password))
                {
                    log.Graba_Log_Info("RSERVER: " + inPath + nomArchivo);
                    log.Graba_Log_Info("RLOCAL: " + outPath + nomArchivo);

                    client.KeepAliveInterval = TimeSpan.FromSeconds(60);
                    client.ConnectionInfo.Timeout = TimeSpan.FromMinutes(180);
                    client.OperationTimeout = TimeSpan.FromMinutes(180);
                    client.Connect();
                    bool connected = client.IsConnected;

                    var file = File.OpenWrite(outPath + nomArchivo + "_.txt");

                    log.Graba_Log_Info("Descargando archivo..."); 

                    if (!(client.Exists(inPath + nomArchivo + ".txt")))
                    {
                        detallesBit = "Server: " + inPath + nomArchivo + ".txt | Error: Archivo no encontrado en el SFTP";
                        dtBitacora.Rows.Add("B", "Descarga Archivo", _metodo, detallesBit, "C");
                        bl.RegistraBitacora(dsBitacora.GetXml());

                        log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: Archivo no encontrado en el SFTP");

                        lv_Retorno = false;
                    }
                    else
                    {
                        client.DownloadFile(inPath + nomArchivo + ".txt", file);

                        detallesBit = "Server: " + inPath + nomArchivo + " | File: " + client.GetAttributes(inPath + nomArchivo + ".txt") + " | Con: " + client.ConnectionInfo;
                        dtBitacora.Rows.Add("B", "Descarga Archivo", _metodo, detallesBit, "C");
                        bl.RegistraBitacora(dsBitacora.GetXml());

                    }
                    file.Close();
                    client.Disconnect();
                }

            }

            catch (Exception ex)
            {
                //Registrar en Tabla bitacora
                detallesBit = "Server: " + inPath + nomArchivo + " | Error: " + ex.Message;
                dtBitacora.Rows.Add("B","Descarga Archivo", _metodo, detallesBit, "C");
                bl.RegistraBitacora(dsBitacora.GetXml());

                lv_Retorno = false;

                log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message + " " + ex.StackTrace);
            }

            return lv_Retorno;
        }

        public DataSet MP_LeeRegistros()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataSet dsBitacora = new DataSet();
            DataTable dtBitacora = new DataTable();
            clsBussinesLogic bl = new clsBussinesLogic();
            string detallesBit = "";

            this._metodo = MethodBase.GetCurrentMethod().Name;

            int counter = 0;
            string nomArchivo = NombreArchivo() + "_.txt";
            string outPath = ((string)ConfigurationManager.AppSettings["stRutaLocalSFTP"]).Trim();

            log.Graba_Log_Info("Lectura: " + outPath + nomArchivo);
            ds.DataSetName = "Root";
            ds.Tables.Add(dt);
            dt.TableName = "Pagos";
            dt.Columns.Add("CodProv", typeof(string));
            dt.Columns.Add("FormaPago", typeof(string));
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Valor", typeof(string));
            dt.Columns.Add("TipoIdentificacion", typeof(string));
            dt.Columns.Add("Identificacion", typeof(string));
            dt.Columns.Add("Factura", typeof(string));
            dt.Columns.Add("Tipo", typeof(string));
            dt.Columns.Add("FechaDesde", typeof(string));
            dt.Columns.Add("FechaHasta", typeof(string));

            dsBitacora.DataSetName = "Root";
            dsBitacora.Tables.Add(dtBitacora);
            dtBitacora.TableName = "Pagos";
            dtBitacora.Columns.Add("Tipo", typeof(string));
            dtBitacora.Columns.Add("Proceso", typeof(string));
            dtBitacora.Columns.Add("Servicio", typeof(string));
            dtBitacora.Columns.Add("Detalle", typeof(string));
            dtBitacora.Columns.Add("Accion", typeof(string));

            try
            {
                foreach (string line in File.ReadLines(outPath + nomArchivo))
                {
                    string[] datosLinea = line.Split(new char[] { '\t'});

                    dt.Rows.Add(
                        datosLinea[0].Trim(),
                        datosLinea[1].Trim(),
                        datosLinea[2].Trim(),
                        datosLinea[3].Trim(),
                        datosLinea[4].Trim(),
                        datosLinea[5].Trim(),
                        datosLinea[6].Trim().Length > 1 ? datosLinea[6].Trim().Substring(0,datosLinea[6].Trim().Length-1) : datosLinea[6].Trim(),
                        "I","",""
                        );

                    counter++;
                }

                detallesBit = "Lineas Leidas: " + counter + " | File: " + outPath + nomArchivo ;
                dtBitacora.Rows.Add("B","Registros", _metodo, detallesBit, "C");
                bl.RegistraBitacora(dsBitacora.GetXml());

                log.Graba_Log_Info("Lineas Leidas: " + counter);
            }
            catch (Exception ex)
            {
                //Registrar en Tabla bitacora
                detallesBit = "Error: "+ex.Message;
                dtBitacora.Rows.Add("B", "Registros", _metodo, detallesBit, "C");
                bl.RegistraBitacora(dsBitacora.GetXml());

                log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message + " " + ex.StackTrace);
                return null;
            }

            return ds;
        }

        public void MP_EliminaArchivoLocal()
        {
            string nomArchivo = NombreArchivo() + "_.txt";
            string outPath = ((string)ConfigurationManager.AppSettings["stRutaLocalSFTP"]).Trim();

            string ruta = outPath + nomArchivo;
            File.Delete(ruta);
        }

        public void MP_EnviaCorreo(DataSet registros,string destinarios)
        {
            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            System.Collections.Generic.Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            string detalle = "";
            PI_NombrePlantilla = "NotificacionPagos.html"; //RFD0 - 2022 - 155

            this._metodo = MethodBase.GetCurrentMethod().Name;

            log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            if (registros.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in registros.Tables[0].Rows)
                {
                    detalle = detalle + "<tr><td style='border: 1px solid #d9d9d9'>" + dr["N"].ToString() + "</td>";
                    detalle = detalle + "<td style = 'border: 1px solid #d9d9d9'>" + dr["Identificacion"].ToString() + "</td>";
                    detalle = detalle + "<td style = 'border: 1px solid #d9d9d9'>" + dr["Proveedor"].ToString() + "</td>";
                    detalle = detalle + "<td style = 'border: 1px solid #d9d9d9'>" + dr["Factura"].ToString() + "</td>";
                    detalle = detalle + "<td style = 'border: 1px solid #d9d9d9'>" + dr["FormaPago"].ToString() + "</td>";
                    detalle = detalle + "<td style = 'border: 1px solid #d9d9d9'>" + dr["FechaPago"].ToString() + "</td>";
                    detalle = detalle + "<td style = 'border: 1px solid #d9d9d9'>" + dr["Valor"].ToString() + "</td>";
                    detalle = detalle + "<td style = 'border: 1px solid #d9d9d9'>" + dr["Detalle"].ToString() + "</td></tr>";
                }

                //mensajeEmail = mensajeEmail.Replace("_@Detalle", detalle);

                #region RFD0-2022-155 CORREO
                Variables = new System.Collections.Generic.Dictionary<string, string>();
                Variables.Add("@@Detalle", detalle);
                

                #endregion


                Thread t = new Thread(() => EnviarEmail(destinarios, "Detalle de Pagos", "", null, PI_NombrePlantilla, Variables));
                t.Start();
            }
            else {
                log.Graba_Log_Error("No existen registros por notificar");
            }

            log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

        }

        private string EnviarEmail(string pCorreoE, string asuntoEmail, string mensajeEmail, byte[] archivo, string PI_NombrePlantilla, Dictionary<string, string> Variables)
        {
            string retornon = "";
            String PI_Variables = string.Empty;

            string Pl_Usr_Envia = ((string)ConfigurationManager.AppSettings["SRVCorreoDirSender"]).Trim(); 
            try
            {
                log.Graba_Log_Info("Inicio Envia Email");
                log.Graba_Log_Info("Correos: " + pCorreoE); 

                clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();
                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();

                archivo = System.Text.Encoding.ASCII.GetBytes("TEST");

                retornon = objEnvMail.EnviaCorreoDF(Pl_Usr_Envia, pCorreoE, "", "", asuntoEmail, "", false, true, false,
                            archivo, "",
                            PI_NombrePlantilla, PI_Variables);

                log.Graba_Log_Info("Envio de correo: " + retornon + " " + pCorreoE);
            }
            catch (Exception ex)
            {
                log.Graba_Log_Error("Error Correo: " + ex.Message);

            }
            return retornon;
        }

        private string NombreArchivo()
        {
            DateTime fecha = DateTime.Now;
            string prefijoArchivo = ((string)ConfigurationManager.AppSettings["prefijoArchivo"]).Trim();

            return prefijoArchivo + fecha.Year.ToString("D4") + fecha.Month.ToString("D2") + fecha.Day.ToString("D2");
        }

    }
}
