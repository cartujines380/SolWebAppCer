using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;

namespace clibProveedores
{
    public class clsArticulo
    {

        //Articulo.Sol_P_Consulta	101
        public DataSet ConsSolicitudArticulo(string PI_Semilla, string PI_Session, string PI_ParamXML)
        {
            DataSet ds = new DataSet();
            try
            {
                //Por cada transaccion un metodo con su respectivo ID
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 101;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_ParamXML
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

        //Articulo.Sol_P_Grabar	102	
        public DataSet SolicitudArticulo(string PI_Semilla, string PI_Session, string PI_ParamXML)
        {
            DataSet ds = new DataSet();
            try
            {
                //Por cada transaccion un metodo con su respectivo ID
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 102;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_ParamXML
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

        //Articulo.Art_P_Consulta	100	
        public DataSet ConsArticulo(string PI_Semilla, string PI_Session, string PI_ParamXML)
        {
            DataSet ds = new DataSet();
            try
            {
                //Por cada transaccion un metodo con su respectivo ID
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 100;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_ParamXML
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

        //Articulo.Art_P_Grabar	103	
        public DataSet Articulo(string PI_Semilla, string PI_Session, string PI_ParamXML)
        {
            DataSet ds = new DataSet();
            try
            {
                //Por cada transaccion un metodo con su respectivo ID
                clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                objSeg.SetSemilla(PI_Semilla);
                objSeg.IdOrganizacion = 39;
                objSeg.IdTransaccion = 103;
                objSeg.IdOpcion = 1;
                objSeg.ArrParams = new Object[1] {
                    PI_ParamXML
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

    }
}
