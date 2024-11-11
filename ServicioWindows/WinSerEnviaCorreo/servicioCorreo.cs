using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Reflection;
using System.IO;

namespace FE.ServicioSendCorreo
{
    public partial class servicioCorreo : ServiceBase
    {
        string sSource;
        string sLog;
        string sEvent;

        public servicioCorreo()
        {
            InitializeComponent();
            _clase = this.GetType().Name;

            sSource = "SIPE Correo FE";
            sLog = "Application";
            sEvent = "Servicio Correo";

            // INI Solo Pruebas
            OnStart(null);
            // FIN Solo Pruebas

            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);

        }
        
        private bool _enEjecucion = false;
        private int _intervalo = 1000;
        private static System.Timers.Timer _timer = new System.Timers.Timer();
        private string _clase;
        private string _metodo;
        clsBussinesLogic _Bussines;

        private string _fechadeldia;
        private bool _cambiodia;


        protected override void OnStart(string[] args)
        {
            try
            {
                _fechadeldia = "";
                _metodo = MethodBase.GetCurrentMethod().Name;
                clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI | Intervalo:" + _intervalo.ToString());

                _Bussines = new clsBussinesLogic();

                // LoginAplicacion Framework Seguridad
                if (!clsGlobal.SetLoginAplicacion())
                {
                    clsEntidadCorreo.p_Log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + clsGlobal.Msg);
                    return;
                }
                // Fin LoginAplicacion

                // INI Solo Pruebas
                //TareaServicio();
                // FIN Solo Pruebas



                _intervalo = clsEntidadCorreo.intervaloServicioCorreo;
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
                _timer.Interval = _intervalo;               
                _timer.Enabled = true;
                _timer.Start();

                clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(sSource, ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                DateTime lv_FechaIni = DateTime.Now;
                _metodo = MethodBase.GetCurrentMethod().Name;
                clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI | EnEjecucion:" + _enEjecucion.ToString());
                while (_enEjecucion)
                {
                    // Hasta que termine de Ejecutarse.
                    if ((DateTime.Now - lv_FechaIni).TotalSeconds > 20) // Esperar 10 Segundos en caso que demore la salida.
                        _enEjecucion = false;
                }
                _timer.Stop();
                _timer.Dispose();
                clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN | EnEjecucion:" + _enEjecucion.ToString());
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(sSource, ex.Message);
            }
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            if (!_enEjecucion)
                TareaServicio();
        }

        private void TareaServicio()
        {
            try
            {
                clsEntidadCorreo.p_Log = new clibLogger.clsLogger();
                _metodo = MethodBase.GetCurrentMethod().Name;
                _Bussines = new clsBussinesLogic();
                _enEjecucion = true;
                DataTable lv_DtDocumentosNoEnviados = new DataTable();
                DataTable lv_Certificados = new DataTable();
                clsMultiplesProcesos objMultipleProcesos = new clsMultiplesProcesos();

                if (!_Bussines.BL_ConsultaDocumentosNoEnviadosPorCorreo(ref lv_DtDocumentosNoEnviados))
                {
                    _enEjecucion = false;
                    return;
                }
                
                if (lv_DtDocumentosNoEnviados.Rows.Count == 0)
                {

                    _enEjecucion = false;
                    return;
                }
                objMultipleProcesos.MP_ProcesarRegistros_v2(lv_DtDocumentosNoEnviados);
            }
            catch (Exception ex)
            {
                _enEjecucion = false;
                clsEntidadCorreo.p_Log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message);
            }
            finally
            {
                _enEjecucion = false;
            }
            _enEjecucion = false;
        }
    }
}
