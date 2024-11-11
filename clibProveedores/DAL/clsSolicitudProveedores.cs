using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Xml;

namespace clibProveedores
{
    public class clsSolicitudProveedores
    {
         #region Metodos
        
        /// <summary>
        /// Graba Solictud de Proveedor
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        /// <returns></returns>
        public DataSet GrabaSolcitud(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {DataSet ds = new DataSet();
            try{
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 200;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }

            return ds;

            
            //    op.ProcedimientoAlmacenado = "[Proveedor].Pro_P_GrabaSolProveedor";
              
                                 
        }
        
        /// <summary>
        /// Consulta Solicitud de Proveedor Id
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        
        /// <returns></returns>
        public DataSet ConsultaSolProveedorId(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();

            try{
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 201;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }

            return ds;

            
            //    op.ProcedimientoAlmacenado = "[Proveedor].[Pro_P_ConsultaSolProveedorId]";
            
        }
        
        /// <summary>
        /// Consulta ultima solicitud Proveedor
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        
        public DataSet ConsultaULtSolProveedor(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();

               try{
            clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 202;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }

            return ds;


            //    op.ProcedimientoAlmacenado = "[Proveedor].[Pro_P_ConsultaULtSolProveedor]";
              
        }

        /// <summary>
        /// Consulta Datos Adjuntos por solicitud Proveedor
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        /// <returns></returns>
        public DataSet ConsultaGral(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();

            try{
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 203;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }

            return ds;


            
            //    op.ProcedimientoAlmacenado = "[Proveedor].Pro_P_ConsultaSolDocAdjuntoId";
            
        }
        
         /// <summary>
        /// Consulta Datos lINEA DE Negocio 
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        /// <returns></returns>
        public DataSet ConsultaSolLineaNegocioId(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();

                       try{
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 204;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }

            return ds;


            
            //    op.ProcedimientoAlmacenado = "[Proveedor].Pro_P_ConsultaSolLineaNegocioId";
                }
            

        /// <summary>
        /// Consulta Datos Bancos por solicitud Proveedor
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        /// <returns></returns>
        public DataSet ConsultaSolProvBancoId(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();
  
//                op.ProcedimientoAlmacenado = "[Proveedor].Pro_P_ConsultaSolProvBancoId";
  
                       try{
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 205;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }

            return ds;


        }

        
        /// <summary>
        /// Consulta Contacto por Solicitud Proveedor
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        /// <returns></returns>
        /// 
        public DataSet ConsultaSolProvContactoId(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();
            
                //op.ProcedimientoAlmacenado = "[Proveedor].Pro_P_ConsultaSolProvContactoId";
                      try{
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 206;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            
            return ds;
        }
        
        /// <summary>
        /// Consulta de historico de Observaciones de Estados por Solicitud Proveedor
        /// </summary>
         /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        /// <returns></returns>
        public DataSet ConsultaSolProvHistEstadoId(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();
            
            //    op.ProcedimientoAlmacenado = "[Proveedor].Pro_P_ConsultaSolProvHistEstadoId";
                      try{
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 207;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
             
            return ds;
        }

        /// <summary>
        /// Consulta de Zonas po Solicitud Proveedor
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        /// <returns></returns>
        public DataSet ConsultaSolZonaId(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();
            
                //op.ProcedimientoAlmacenado = "[Proveedor].Pro_P_ConsultaSolZonaId";
                      try{
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 208;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            
            return ds;
        }
        
        /// <summary>
        /// Consulta de Direcciones por Solicitud Proveedor
        /// </summary>
        /// <param name="PI_Semilla"></param>
        /// <param name="PI_Session"></param>
        /// <param name="PI_XmlDatos"></param>
        /// <returns></returns>
        public DataSet ConsultaSolProvDireccionId(string PI_Semilla, string PI_Session,string PI_XmlDatos)
        {
            DataSet ds = new DataSet();
            
            //    op.ProcedimientoAlmacenado = "[Proveedor].Pro_P_ConsultaSolProvDireccionId";
                      try{
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 209;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_XmlDatos
                };
                ds = objSeg.EjecutaTransaccionDS(PI_Session);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
             
            return ds;
        }

        
        #endregion
    }

    }

