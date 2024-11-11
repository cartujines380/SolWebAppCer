
using FE.Utileria;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

namespace FE.ServicioSendCorreo
{
    internal class clsMultiplesProcesos
    {
        private string _clase;
        private string _metodo;
        private int _maxHilos;
        private string _LogoEmpresa;
        private string _DestinoCorreo_DelConfigTesting;
        private string _RUC_EmisorCorreoDestino_DelConfigTesting;

        public clsMultiplesProcesos()
        {
            this._clase = this.GetType().Name;
            this._maxHilos = clsEntidadCorreo.maxItemsConcurrenciaCorreo;
        }

        public bool MP_ProcesarRegistros(DataTable PI_DtDocumentos)
        {
            bool flag = true;
            this._metodo = MethodBase.GetCurrentMethod().Name;
            clsEntidadCorreo.p_Log.Graba_Log_Info(this._clase + " " + this._metodo + " INI ");
            try
            {
                int length = PI_DtDocumentos.Rows.Count - 1;
                Action[] actionArray = new Action[length];
                for (int Documento = 0; Documento < length; ++Documento)
                    actionArray[Documento] = (Action)(() => this.MP_ProcesarDocumento((object)PI_DtDocumentos.Rows[Documento]));
                Parallel.Invoke(new ParallelOptions()
                {
                    MaxDegreeOfParallelism = this._maxHilos
                }, actionArray);
            }
            catch (Exception ex)
            {
                flag = false;
                clsEntidadCorreo.p_Log.Graba_Log_Error(this._clase + " " + this._metodo + " ERROR: " + ex.Message, "0");
            }
            clsEntidadCorreo.p_Log.Graba_Log_Info(this._clase + " " + this._metodo + " FIN ");
            return flag;
        }

        public bool MP_ProcesarRegistros_v2(DataTable PI_DtDocumentos)
        {
            bool flag = true;
            this._metodo = MethodBase.GetCurrentMethod().Name;
            clsEntidadCorreo.p_Log.Graba_Log_Info(this._clase + " " + this._metodo + " INI ");
            try
            {
                int count = PI_DtDocumentos.Rows.Count;
                Parallel.ForEach<DataRow>((IEnumerable<DataRow>)DataTableExtensions.AsEnumerable(PI_DtDocumentos), new ParallelOptions()
                {

                    MaxDegreeOfParallelism = this._maxHilos
                }, (Action<DataRow>)(drValue => this.MP_ProcesarDocumento((object)drValue)));
            }
            catch (Exception ex)
            {
                flag = false;
                string str = "";
                try
                {
                    str = ex.InnerException.Message;
                }
                catch
                {
                }
                clsEntidadCorreo.p_Log.Graba_Log_Error(this._clase + " " + this._metodo + " ERROR: " + ex.Message + " INNER_EXCEPT:" + str, "0");
            }
            clsEntidadCorreo.p_Log.Graba_Log_Info(this._clase + " " + this._metodo + " FIN ");
            return flag;
        }

        public bool MP_ProcesarRegistros_Unico(DataTable PI_DtDocumentos)
        {
            bool flag = true;
            this._metodo = MethodBase.GetCurrentMethod().Name;
            clsEntidadCorreo.p_Log.Graba_Log_Info(this._clase + " " + this._metodo + " INI ");
            try
            {
                var options = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["Concurrencia"]) // maximum number of threads
                };


                Parallel.ForEach(PI_DtDocumentos.AsEnumerable(), options,  drow =>
                {
                    this.MP_ProcesarDocumento((object)drow);
                });
                    
            }
            catch (Exception ex)
            {
                flag = false;
                clsEntidadCorreo.p_Log.Graba_Log_Error(this._clase + " " + this._metodo + " ERROR: " + ex.Message, "0");
            }
            clsEntidadCorreo.p_Log.Graba_Log_Info(this._clase + " " + this._metodo + " FIN ");
            return flag;
        }

        public bool MP_ProcesarDocumento(object drDocumento)
        {
            bool flag = true;
            this._metodo = MethodBase.GetCurrentMethod().Name;
            clsEntidadCorreo.p_Log.Graba_Log_Info(this._clase + " " + this._metodo + " INI ");
            DataRow dataRow = (DataRow)drDocumento;
            int PI_IdReg = Convert.ToInt32(dataRow["IdNotificacion"]);
            clsBussinesLogic clsBussinesLogic = new clsBussinesLogic();
            try
            {
                                               
                DataTable DtNotificacionBody = null;
                DataTable DtNotificacionAdjunto = null;
                Boolean lv_respuesta = false;

                lv_respuesta = clsBussinesLogic.BL_ConsultarDetalleNotificacion(PI_IdReg, ref DtNotificacionBody, ref DtNotificacionAdjunto);
                byte[] byteContent = (byte[])null;

                clsEmail clsEmail = new clsEmail();
                List<clsEmail.AdjuntoEnvio> adjuntos = new List<clsEmail.AdjuntoEnvio>();
                string destinatario = dataRow["Destinatario"].ToString();
                string Motivo = dataRow["Motivo"].ToString();
                string PI_ArchivoHtmlCorreo = dataRow["PlantillaHtml"].ToString();
                int ia = 0;
                while (ia < DtNotificacionAdjunto.Rows.Count)
                {
                    string AdjuntoNombre = DtNotificacionAdjunto.Rows[ia]["AdjuntoNombre"].ToString();
                    byteContent = (byte[])DtNotificacionAdjunto.Rows[ia]["AdjuntoArchivo"];
                    using (ZipFile zipFile = new ZipFile())
                    {
                        clsEmail.AdjuntoEnvio abj = new clsEmail.AdjuntoEnvio();
                        if (DtNotificacionAdjunto.Rows[ia]["compress"].ToString().Equals("True"))
                        {
                            abj.lisadjunto.Add(Decompress(byteContent));
                        }
                        else
                        {
                            abj.lisadjunto.Add(byteContent);
                        }
                        abj.NombreAdjunto = AdjuntoNombre;
                        adjuntos.Add(abj);

                    }
                    ia++;
                }
                
                string codError = "00";
                flag = clsEmail.EnviarCorreo(clsEntidadCorreo.Notificacion_ServidorSmtp, clsEntidadCorreo.Notificacion_PuertoSmtp, clsEntidadCorreo.Notificacion_SenderSmtp, clsEntidadCorreo.Notificacion_UsuarioSmtp, clsEntidadCorreo.Notificacion_ClaveSmtp, Motivo, destinatario, DtNotificacionBody, adjuntos, PI_ArchivoHtmlCorreo, clsEntidadCorreo.Notificacion_AplicarSSL);
                string PI_Estado1;
                if (flag)
                {
                    PI_Estado1 = "PR";
                    codError = "00";
                }
                else
                {
                    PI_Estado1 = "ER";
                    codError = "99";
                    clsEntidadCorreo.p_Log.Graba_Log_Error(this._clase + " " + this._metodo + " ERROR: " + (string)clsEmail.Msg, "0");
                }
                clsBussinesLogic.BL_ActualizaEstadoDocumento(PI_IdReg, PI_Estado1, codError, clsEmail.Msg);
            }

            catch (Exception ex)
            {
                flag = false;
                clsEntidadCorreo.p_Log.Graba_Log_Error(this._clase + " " + this._metodo + " ERROR: " + ex.StackTrace.ToString() + ex.Message, "0");
                try
                {
                    clsBussinesLogic.BL_ActualizaEstadoDocumento(PI_IdReg, "ER", "99", ex.Message );}
                catch { }

            }
            clsEntidadCorreo.p_Log.Graba_Log_Info(this._clase + " " + this._metodo + " FIN ");
            return flag;
        }
        private static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }



    }


}
