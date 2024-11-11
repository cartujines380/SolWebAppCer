using PP.AccesoDatos;
using System;
using System.Data;
using System.Reflection;

namespace FE.ServicioSendCorreo
{
    class clsBussinesLogic
    {
        private string _clase;
        private string _metodo;
        public static DataTable dtEstablecimientos;

        public clsBussinesLogic()
        {
            _clase = this.GetType().Name;
        }

        public bool BL_ConsultaDocumentosNoEnviadosPorCorreo(ref DataTable PI_DtDocumentosNoEnviados)
        {
            bool lv_Retorno = true;
            _metodo = MethodBase.GetCurrentMethod().Name;
            clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            try
            {
                OperadorBaseDatos op = new OperadorBaseDatos(clsGlobal.CadenaConexion);
                op.ProcedimientoAlmacenado = "[Notificacion].[Not_CONS_SERVICIO_CORREO]";
                op.AgregarParametro("@PI_ESTADO", SqlDbType.VarChar, "NP", 2);
                op.AgregarParametro("@PI_Cant", SqlDbType.Int, clsEntidadCorreo.maxRegistrosCorreo);
                PI_DtDocumentosNoEnviados = op.ConsultarDataTable();

            }
            catch (Exception ex)
            {
                lv_Retorno = false;
                clsEntidadCorreo.p_Log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message);
            }
            clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            return lv_Retorno;
        }


        public bool BL_ActualizaEstadoDocumento(Int32 PI_IdReg, string PI_Estado, string CodError, string Msgerror)
        {
            bool lv_Retorno = true;
            _metodo = MethodBase.GetCurrentMethod().Name;
            clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");

            try
            {
                OperadorBaseDatos op = new OperadorBaseDatos(clsGlobal.CadenaConexion);
                op.ProcedimientoAlmacenado = "[Notificacion].[Not_ACTUALIZA_ESTADO_CORREO]";
                op.AgregarParametro("@PI_IDReg", SqlDbType.Int, PI_IdReg);
                op.AgregarParametro("@PI_ESTADO", SqlDbType.VarChar, PI_Estado, 2);
                op.AgregarParametro("@PI_CodError", SqlDbType.VarChar, CodError, 2);
                op.AgregarParametro("@PI_MsgError", SqlDbType.VarChar, Msgerror, 100);
                op.Ejecutar();

            }
            catch (Exception ex)
            {
                lv_Retorno = false;
                clsEntidadCorreo.p_Log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message);
            }
            clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            return lv_Retorno;
        }
        public bool BL_ConsultarDetalleNotificacion(int PI_IdReg, ref DataTable DtNotificacionBody, ref DataTable DtNotificacionAdjunto)
        {
            bool lv_Retorno = true;
            _metodo = MethodBase.GetCurrentMethod().Name;
            clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            try
            {
                OperadorBaseDatos op = new OperadorBaseDatos(clsGlobal.CadenaConexion);
                op.ProcedimientoAlmacenado = "[Notificacion].[Not_CONS_SERVICIO_CORREO_DET]";
                op.AgregarParametro("@PI_IdNotificacion", SqlDbType.Int, PI_IdReg);
                DtNotificacionBody = op.ConsultarDataSet().Tables[0];
                DtNotificacionAdjunto = op.ConsultarDataSet().Tables[1];

            }
            catch (Exception ex)
            {
                lv_Retorno = false;
                clsEntidadCorreo.p_Log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message);
            }
            clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            return lv_Retorno;
        }
        public bool BL_ConsultarConexionCorreo(ref DataTable DtResult)
        {
            bool lv_Retorno = true;
            _metodo = MethodBase.GetCurrentMethod().Name;
            clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " INI ");
            try
            {
                OperadorBaseDatos op = new OperadorBaseDatos(clsGlobal.CadenaConexion);
                op.ProcedimientoAlmacenado = "[Notificacion].[Not_CONS_CONFIGURACION_CORREO]";
                DtResult = op.ConsultarDataSet().Tables[0];
            }
            catch (Exception ex)
            {
                lv_Retorno = false;
                clsEntidadCorreo.p_Log.Graba_Log_Error(_clase + " " + _metodo + " ERROR: " + ex.Message);
            }
            clsEntidadCorreo.p_Log.Graba_Log_Info(_clase + " " + _metodo + " FIN ");
            return lv_Retorno;
        }
    }
}
