using clibLogger;
using System;
using System.Configuration;
using System.Data;
using System.Reflection;


namespace wsProcesaPagosBG
{
    class clsBussinesLogic
    {
        private string _clase;
        private string _metodo;
        private string _conexion;
        clsLogger log = new clsLogger();

        public clsBussinesLogic()
        {
            _clase = this.GetType().Name;
            _conexion = clsGlobal.CadenaConexion;
        }

        public DataSet BL_RegistraPagos(DataSet ds)
        {
            string p_Log = ((string)ConfigurationManager.AppSettings["ArchivoLog"]).Trim();
            DataTable dtRes = new DataTable();
            DataSet dsRetorno = new DataSet();

            _metodo = MethodBase.GetCurrentMethod().Name;
            log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            try
            {
                FE.AccesoDatos.OperadorBaseDatos op = new FE.AccesoDatos.OperadorBaseDatos(_conexion);
                op.ProcedimientoAlmacenado = "[Proveedor].[Pro_P_ConsultaPagos]";
                op.AgregarParametro("@PI_ParamXML", SqlDbType.Xml, ds.GetXml());
                dsRetorno = op.ConsultarDataSet();
                dsRetorno.DataSetName = "RespuestaSPPagos";
                dsRetorno.Tables[0].TableName = "Registros";
                log.Graba_Log_Info(dsRetorno.GetXml());
                
            }
            catch (Exception ex)
            {
                dsRetorno = null;
                log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message + " " + ex.StackTrace);
            }
            
            log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            return dsRetorno;

        }

        public DataSet BL_ConsultaDatosProv(string xml)
        {
            string p_Log = ((string)ConfigurationManager.AppSettings["ArchivoLog"]).Trim();
            DataTable dtRes = new DataTable();
            DataSet dsRetorno = new DataSet();

            _metodo = MethodBase.GetCurrentMethod().Name;
            log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            try
            {
                FE.AccesoDatos.OperadorBaseDatos op = new FE.AccesoDatos.OperadorBaseDatos(_conexion);
                op.ProcedimientoAlmacenado = "[Proveedor].[Pro_P_ConsultaPagos]";
                op.AgregarParametro("@PI_ParamXML", SqlDbType.Xml, xml);
                dsRetorno = op.ConsultarDataSet();
                dsRetorno.DataSetName = "Root";
                dsRetorno.Tables[0].TableName = "ConsultaProv";
                log.Graba_Log_Info(dsRetorno.GetXml());

            }
            catch (Exception ex)
            {
                dsRetorno = null;
                log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message + " " + ex.StackTrace);
            }

            log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            return dsRetorno;

        }

        public void BL_RegistraMensajeFlash(String registro)
        {
            string p_Log = ((string)ConfigurationManager.AppSettings["ArchivoLog"]).Trim();
            DataTable dtRes = new DataTable();

            _metodo = MethodBase.GetCurrentMethod().Name;

            try
            {
                FE.AccesoDatos.OperadorBaseDatos op = new FE.AccesoDatos.OperadorBaseDatos(_conexion);
                op.ProcedimientoAlmacenado = "[Proveedor].[Pro_P_ConsultaPagos]";
                op.AgregarParametro("@PI_ParamXML", SqlDbType.Xml, registro);
                op.Ejecutar();

                log.Graba_Log_Info(registro);

            }
            catch (Exception ex)
            {
                log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message);
            }

        }

        public void RegistraBitacora(String registro) {
            string p_Log = ((string)ConfigurationManager.AppSettings["ArchivoLog"]).Trim();
            DataTable dtRes = new DataTable();

            _metodo = MethodBase.GetCurrentMethod().Name;
            log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            try
            {
                FE.AccesoDatos.OperadorBaseDatos op = new FE.AccesoDatos.OperadorBaseDatos(_conexion);
                op.ProcedimientoAlmacenado = "[Proveedor].[Pro_P_ConsultaPagos]";
                op.AgregarParametro("@PI_ParamXML", SqlDbType.Xml, registro);
                op.Ejecutar();
                
                log.Graba_Log_Info(registro);

            }
            catch (Exception ex)
            {
                log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message + " " + ex.StackTrace);
            }

            log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

        }

        public Boolean BL_IsPagoProcesadoHoy()
        {
            bool retorno = true;
            string p_Log = ((string)ConfigurationManager.AppSettings["ArchivoLog"]).Trim();
            string registro = "";
            DataSet dsRetorno = new DataSet();
            int verifica = 0;

            registro = "<Root><Pagos><Tipo>P</Tipo><NomArchivo>" + NombreArchivo() + "</NomArchivo></Pagos></Root>";

            _metodo = MethodBase.GetCurrentMethod().Name;
            log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            try
            {
                FE.AccesoDatos.OperadorBaseDatos op = new FE.AccesoDatos.OperadorBaseDatos(_conexion);
                op.ProcedimientoAlmacenado = "[Proveedor].[Pro_P_ConsultaPagos]";
                op.AgregarParametro("@PI_ParamXML", SqlDbType.Xml, registro);
                dsRetorno = op.ConsultarDataSet();
                dsRetorno.DataSetName = "Root";
                dsRetorno.Tables[0].TableName = "ConsultaPagoHoy";

                if (dsRetorno != null)
                {
                    if (dsRetorno.Tables[0].Rows.Count > 0)
                    {
                        verifica = dsRetorno.Tables[0].Rows[0].Field<int>("retorno");
                    }
                }

            }
            catch (Exception ex)
            {
                log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message + " " + ex.StackTrace);
            }

            log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");

            retorno = verifica == 1 ? true: false;

            return retorno;
        }

        private string NombreArchivo()
        {
            DateTime fecha = DateTime.Now;
            string prefijoArchivo = ((string)ConfigurationManager.AppSettings["prefijoArchivo"]).Trim();

            return prefijoArchivo + fecha.Year.ToString("D4") + fecha.Month.ToString("D2") + fecha.Day.ToString("D2");
        }

    }
}
