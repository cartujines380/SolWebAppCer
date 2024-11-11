using Quartz;
using Quartz.Impl;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;

namespace WinSerCreaUsuario
{
    public partial class CreaUsuario : ServiceBase
    {
        #region Variables

        private string sSource;
        private string IntevaloMinuto;
        private string _clase;
        private IScheduler scheduler;
        private ISchedulerFactory schedulerFactory;

        #endregion

        public CreaUsuario()
        {
            _clase = this.GetType().Name;
            schedulerFactory = CreateSchedulerFactory();
            scheduler = GetScheduler();

            InitializeComponent();
            sSource = "SIPE Servicio_CreaUsuario";
            IntevaloMinuto = ConfigurationManager.AppSettings["INTERVALO_MINUTO"];

#if DEBUG
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info("CreaUsuario: " + " Advertencia se esta ejecutando en modo Debug. ");
            OnStart(null);
#endif
        }

        #region Crea Configuracion Sheduler

        protected virtual IScheduler GetScheduler()
        {
            return schedulerFactory.GetScheduler();
        }
        protected virtual IScheduler Scheduler
        {
            get { return scheduler; }
        }
        protected virtual ISchedulerFactory CreateSchedulerFactory()
        {
            return new StdSchedulerFactory();
        }


        #endregion

        protected override void OnStart(string[] args)
        {
            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(metodo + " INICIA SERVICIO | Intervalo: " + IntevaloMinuto + " INI. ");
            #endregion Log

            try
            {


                #if DEBUG               
                ClsCreaUsuario test = new ClsCreaUsuario();
                test.Procesa();
                //#else
                //_intervalo = clsGlobal.intervaloServicioCorreo;
                //_timer.Elapsed += new System.Timers.ElapsedEventHandler(IntervalTimer_Elapsed);
                //_timer.Interval = _intervalo;
                //_timer.Enabled = true;
                //_timer.Start();
#endif
                Scheduler.Start();
                CreateJob();

            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Info(metodo + " ERROR.: " + ex.Message);
                EventLog.WriteEntry(sSource, ex.Message);
            }
            finally
            {
                p_Log.Graba_Log_Info(metodo + " INICIA SERVICIO. FIN. ");
            }
        }


        protected override void OnStop()
        {
            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info( _clase + " " + metodo + " INI ");
            #endregion Log
            String sMensaje = String.Format("Se Detuvo servicio Crea Usuario a las {0} ", DateTime.Now.ToShortTimeString());

            try
            {
                scheduler.Shutdown(true);
                
            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " ERROR.: " + ex.Message);
            }
            p_Log.Graba_Log_Info(_clase + " " + metodo + " " + sMensaje);
            p_Log.Graba_Log_Info(_clase + " " + metodo + " FIN ");
        }



        private void CreateJob()
        {
            #region Log
            string metodo = MethodBase.GetCurrentMethod().Name;
            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info(_clase + " " + metodo + " INI ");
            #endregion Log

            try
            {

                if (!clsGlobal.SetLoginAplicacion())
                {
                    p_Log.Graba_Log_Error(" ERROR: " + clsGlobal.Msg);
                    return;
                }

                IJobDetail job = JobBuilder.Create<EjecutarProceso>()
                    .WithIdentity("job", "group")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger", "group")
                    .StartNow()
                    .WithSchedule(SimpleScheduleBuilder.RepeatMinutelyForever(Convert.ToInt32(IntevaloMinuto)))
                    .Build();

                Scheduler.ScheduleJob(job, trigger); //DESCOMENTAR EN PRODUCCION.
            }
            catch (SchedulerException se)
            {
                p_Log.Graba_Log_Error(_clase + " " + metodo + " ERROR: " + se.Message);
            }
            finally
            {
                p_Log.Graba_Log_Info(_clase + " " + metodo + " FIN ");
            }
        }        


    }
}
