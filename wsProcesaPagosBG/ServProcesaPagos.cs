using clibLogger;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace wsProcesaPagosBG
{
    public partial class ServProcesaPagos : ServiceBase
    {
        bool bandera = false;
        readonly clsLogger log = new clsLogger();
        
        public ServProcesaPagos()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            stLapso.Start();
        }

        protected override void OnStop()
        {
            stLapso.Stop();
        }

        private void stLapso_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            DateTime fecha = DateTime.Now;
            string Hora = ""; 
            string Minutos = "";

            log.Graba_Log_Info(fecha.TimeOfDay.ToString());
            log.Graba_Log_Info("Hora: " + fecha.Hour.ToString() + " - Min: " + fecha.Minute.ToString());

            Hora = ((string)ConfigurationManager.AppSettings["Hora"]).Trim();
            Minutos = ((string)ConfigurationManager.AppSettings["Minutos"]).Trim();

            if (fecha.Hour.ToString() == Hora)
            {
                if(fecha.Minute.ToString() == Minutos)
                {
                    if (!clsGlobal.SetLoginAplicacion())
                    {
                        log.Graba_Log_Error(" ERROR: " + clsGlobal.Msg);
                        return;
                    }
                    clsBussinesLogic bl = new clsBussinesLogic();
                    clsMultiplesProcesos mp = new clsMultiplesProcesos();
                    DataSet dsBitacora = new DataSet();
                    DataSet dsRespuesta = new DataSet();
                    DataSet dsInfoNotificacion = new DataSet();
                    DataSet dsRespConProv = new DataSet();
                    DataTable dtBitacora = new DataTable();
                    DataTable dtInfoNotificacion = new DataTable();
                    string detallesBit = "", mensaje ="", titulo = "";
                    int contadorInfo = 0;

                    //Bitacora
                    dsBitacora.DataSetName = "Root";
                    dsBitacora.Tables.Add(dtBitacora);
                    dtBitacora.TableName = "Pagos";
                    dtBitacora.Columns.Add("Tipo", typeof(string));
                    dtBitacora.Columns.Add("Proceso", typeof(string));
                    dtBitacora.Columns.Add("Servicio", typeof(string));
                    dtBitacora.Columns.Add("Detalle", typeof(string));
                    dtBitacora.Columns.Add("Accion", typeof(string));

                    //Info a notificar
                    dsInfoNotificacion.DataSetName = "Root";
                    dsInfoNotificacion.Tables.Add(dtInfoNotificacion);
                    dtInfoNotificacion.TableName = "Notificacion";
                    dtInfoNotificacion.Columns.Add("N", typeof(string));
                    dtInfoNotificacion.Columns.Add("Identificacion", typeof(string));
                    dtInfoNotificacion.Columns.Add("Proveedor", typeof(string));
                    dtInfoNotificacion.Columns.Add("Factura", typeof(string));
                    dtInfoNotificacion.Columns.Add("FormaPago", typeof(string));
                    dtInfoNotificacion.Columns.Add("FechaPago", typeof(string));
                    dtInfoNotificacion.Columns.Add("Valor", typeof(string));
                    dtInfoNotificacion.Columns.Add("Detalle", typeof(string));


                    if (bandera) return;

                    log.Graba_Log_Info(" * * * Inicia el proceso * * *");
                    log.Graba_Log_Info("==============================");
                    try
                    {
                        bandera = true;
                        /*Verifica si el archivo ya se proceso*/
                        if (bl.BL_IsPagoProcesadoHoy())
                        {
                            bandera = false;
                            log.Graba_Log_Info("--> Archivo del día de hoy ya fue procesado");
                            return;
                        }
                        /**/

                        if (mp.MP_ExtraeArchivoSFTP())
                        {
                            dsRespuesta = bl.BL_RegistraPagos(mp.MP_LeeRegistros());
                            //Lectura del archivo hacia DS
                            if (dsRespuesta != null)
                            {
                                mp.MP_EliminaArchivoLocal();

                                detallesBit = "Ejecucion: OK";
                                dtBitacora.Rows.Add("B", "General", "stLapso_Elapsed", detallesBit, "G");
                                bl.RegistraBitacora(dsBitacora.GetXml());
                                dsBitacora.Clear();

                                detallesBit = mp.nArchivo;
                                dtBitacora.Rows.Add("B", "General", "Procesado", detallesBit, "P");
                                bl.RegistraBitacora(dsBitacora.GetXml());

                                /*Registros a notificar*/
                                var IdentificacionValues = dsRespuesta.Tables[0].AsEnumerable()
                                                .Select(row => new {
                                                    Identificacion_name = row.Field<string>("Identificacion")
                                                })
                                                .Distinct();

                                log.Graba_Log_Info("***************************");
                                foreach (var name in IdentificacionValues)
                                {
                                    var query = from o in dsRespuesta.Tables[0].AsEnumerable()
                                                where o.Field<string>("Identificacion") == name.Identificacion_name
                                                select o;

                                    dsRespConProv = bl.BL_ConsultaDatosProv("<Root><Pagos><Tipo>D</Tipo><Identificacion>" + name.Identificacion_name + "</Identificacion></Pagos></Root>");

                                    if (dsRespConProv != null)
                                    {
                                        if (dsRespConProv.Tables[0].Rows.Count > 0)
                                        {
                                            log.Graba_Log_Info("Identificacion: " + name.Identificacion_name);

                                            contadorInfo = 0;

                                            foreach (var reg in query)
                                            {
                                                contadorInfo++;

                                                dtInfoNotificacion.Rows.Add(
                                                    contadorInfo,
                                                    reg.Field<string>("Identificacion"),
                                                    dsRespConProv.Tables[0].Rows[0].Field<string>("NomComercial").ToString(),
                                                    reg.Field<string>("Factura"),
                                                    reg.Field<string>("FormaPago"),
                                                    reg.Field<DateTime>("Fecha").ToString("yyyy-MM-dd"),
                                                    String.Format("{0:C}", reg.Field<Decimal>("Valor")),
                                                    reg.Field<string>("Detalle")
                                                    );

                                            }
                                            titulo = "Nuevo pago registrado";
                                            mensaje = "Se ha realizado un nuevo Pago. Verifique en la opción Pagos | Consulta de Pagos";

                                            if (dsRespConProv.Tables[0].Rows[0].Field<string>("CorreoE").ToString().Trim() != "" ||
                                                dsRespConProv.Tables[0].Rows[0].Field<string>("CorreoE").ToString() != null)
                                            {
                                                mp.MP_EnviaCorreo(dsInfoNotificacion, dsRespConProv.Tables[0].Rows[0].Field<string>("CorreoE").ToString());
                                            }
                                            else
                                            {
                                                log.Graba_Log_Warn("Correo " + name.Identificacion_name + " NULO o VACIO");
                                            }

                                            bl.BL_RegistraMensajeFlash("<Root><Pagos><Tipo>N</Tipo><Identificacion>" + name.Identificacion_name + "</Identificacion><Titulo>" + titulo + "</Titulo><Mensaje>" + mensaje + "</Mensaje></Pagos></Root>");
                                            dtInfoNotificacion.Rows.Clear();
                                        }
                                        else
                                        {
                                            dtBitacora.Rows.Add("B", "General", "ConsultaProveedor", "No existe nombre o correo de " + name.Identificacion_name, "C");
                                            log.Graba_Log_Error("No existe registro del proveedor " + name.Identificacion_name);
                                        }
                                    }
                                    else
                                    {
                                        dtBitacora.Rows.Add("B", "General", "ConsultaProveedor", "No existen datos para la consulta: " + name.Identificacion_name, "C");
                                        log.Graba_Log_Error("No existen datos para la consulta: " + name.Identificacion_name);
                                    }

                                }
                                log.Graba_Log_Info("***************************");
                                /*Fin de registros a notificar*/

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        detallesBit = "Error: " + ex.Message;
                        dtBitacora.Rows.Add("B", "General", "stLapso_Elapsed", detallesBit, "G");
                        bl.RegistraBitacora(dsBitacora.GetXml());

                        mp.MP_EliminaArchivoLocal();

                        log.Graba_Log_Error("--> Error: " + ex.Message);
                    }

                    log.Graba_Log_Info(" * * *   Fin el proceso  * * *");
                    log.Graba_Log_Info("==============================");
                    bandera = false;

                    var delay = Task.Delay(TimeSpan.FromSeconds(63));
                    var seconds = 0;
                    while (!delay.IsCompleted)
                    {
                        seconds++;
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        //log.Graba_Log_Info($"Waiting... {seconds}");
                    }
                    stLapso.Stop();
                    stLapso.Start();
                }
                else
                {
                    log.Graba_Log_Info("Aún faltan Minutos [" + Minutos + "][" + fecha.Minute.ToString() + "]");
                }
            }
            else
            {
                log.Graba_Log_Info("Aún faltan Horas [" + Hora + "][" + fecha.Hour.ToString() + "]");
            }
        }
    }
}
