using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Security.Claims;

using System.Text;
using System.Xml;
using System.Data;

using clibProveedores.Models;

using System.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace clibProveedores
{
    public class ClsGeneral
    {

        public DataSet EjecucionGralDs(string PI_ParamXML, int IdTransaccion, int IdOpcion)
        {
            return EjecucionGralDs(new Object[1] { PI_ParamXML }, IdTransaccion, IdOpcion);
        }

        public DataSet EjecucionGralEtiDs(string PI_ParamXML, int IdTransaccion, int IdOpcion)
        {
            return EjecucionGralDs(new Object[1] { PI_ParamXML }, IdTransaccion, IdOpcion);
        }

        public DataSet EjecucionGralLinea(string PI_ParamXML, int IdTransaccion, int IdOpcion)
        {
            return EjecucionGralDs(new Object[1] { PI_ParamXML }, IdTransaccion, IdOpcion);
        }


        public Boolean EjecucionVerificar(string PI_ParamObjList, int IdTransaccion, int IdOpcion)
        {

            Boolean ds = false;
            var ValorTokenUser = string.Empty;
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            Logger.Logger loge = new Logger.Logger();

            var identity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;

            
                try
                {
                ValorTokenUser = (from c in identity.Claims where c.Type == "tokenuser" select c.Value).Single();
                }
                catch (InvalidOperationException ee)
                {
                loge.FilePath = p_Log;
                loge.WriteMensaje("Seg # EjecucionVerificar EX1");
                loge.Linea();
                ValorTokenUser = "Age claim wasn’t found or " +
                        "there were more than one Age claims provided";
                }
            try
            {
                loge.FilePath = p_Log;
                loge.WriteMensaje("Seg # EjecucionVerificar 1");
                loge.Linea();


                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = Proceso.IsPermisoUserTransOpcion(InitialiseService.Semilla, InitialiseService.IdOrganizacion, IdTransaccion, IdOpcion, PI_ParamObjList, ValorTokenUser);

                loge.FilePath = p_Log;
                loge.WriteMensaje("Seg # EjecucionVerificar 2");
                loge.Linea();
            }
            catch (Exception ex)
            {
                loge.WriteMensaje("Seg # EjecucionVerificar EX2");

                ds = false;
            }
            return ds;
        }


        public DataSet EjecucionGralDs(Object[] PI_ParamObjList, int IdTransaccion, int IdOpcion)
        {

            DataSet ds = new DataSet();
            var ValorTokenUser = string.Empty;

            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            Logger.Logger loge = new Logger.Logger();

            var identity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;

            //if (IdTransaccion>=1)
            //if (IdTransaccion>=1)
            if (((IdTransaccion == 306) && (IdOpcion == 1)) || ((IdTransaccion == 1) && (IdOpcion == 1)) || ((IdTransaccion == 411) && (IdOpcion == 1)) || ((IdTransaccion == 412) && (IdOpcion == 1)) || ((IdTransaccion == 210) && (IdOpcion == 1)) || ((IdTransaccion == 201) && (IdOpcion == 1)) || ((IdTransaccion == 202) && (IdOpcion == 1)) || ((IdTransaccion == 216) && (IdOpcion == 1)) || ((IdTransaccion == 101) && (IdOpcion == 1)) || ((IdTransaccion == 102) && (IdOpcion == 1)))
            {
                ValorTokenUser = InitialiseService.PI_Session;


            }
            else
            {
                try
                {
                    ValorTokenUser = (from c in identity.Claims where c.Type == "tokenuser" select c.Value).Single();
                }
                catch (InvalidOperationException ee)
                {
                    ValorTokenUser = "Age claim wasn’t found or " +
                        "there were more than one Age claims provided";

                    ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ee.Message));
                    return ds;
                }

            }

            try
            {
                //using (var client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri("http://localhost:60391/");
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //    // New code:
                //    //HttpResponseMessage response = await client.GetAsync("api/products/1");
                //    var ruta = "api/BaseInvocacion/DatosBase/?Semilla=" + InitialiseService.Semilla + "&IdOrganizacion=" + InitialiseService.IdOrganizacion + "&IdTransaccion=" + IdTransaccion + "&IdOpcion=" + IdOpcion + "&PI_ParamObjList=" + PI_ParamObjList[0].ToString() + "&ValorTokenUser=" + ValorTokenUser;
                //    HttpResponseMessage response = client.GetAsync(ruta).Result;
                //    if (response.IsSuccessStatusCode)
                //    {

                //        var users = response.Content.ReadAsAsync<IEnumerable<DataSet>>().Result;
                //        aux = "";
                //        //     response.Content.rea
                //        // Product product = await response.Content.ReadAsAsync>Product>();

                //    }
                //}

                loge.FilePath = p_Log;
                loge.WriteMensaje("EjecucionGralDs ValorTokenUser: " + ValorTokenUser);
                loge.Linea();

                ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();

                loge.FilePath = p_Log;
                loge.WriteMensaje("EjecucionGralDs Url antes: " + Proceso.Url);
                loge.Linea();

                Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();

                loge.FilePath = p_Log;
                loge.WriteMensaje("EjecucionGralDs Url despues: " + Proceso.Url);
                loge.Linea();

                ds = Proceso.DatosBase(InitialiseService.Semilla, InitialiseService.IdOrganizacion, IdTransaccion, IdOpcion, PI_ParamObjList, ValorTokenUser);
                //Por cada transaccion un metodo con su respectivo ID
                //clibSeguridadCR.clsClienteSeg objSeg = new clibSeguridadCR.clsClienteSeg();
                //objSeg.SetSemilla(InitialiseService.Semilla);
                //objSeg.IdOrganizacion = InitialiseService.IdOrganizacion;
                //objSeg.IdTransaccion = IdTransaccion;
                //objSeg.IdOpcion = IdOpcion;
                ////objSeg.ArrParams = new Object[1] {
                ////    PI_ParamXML
                ////};
                //objSeg.ArrParams = PI_ParamObjList;

                ////ds = objSeg.EjecutaTransaccionDS(InitialiseService.PI_Session);

                //ds = objSeg.EjecutaTransaccionDS(ValorTokenUser);
            }
            catch (Exception ex)
            {
                ds = new DataSet();
                ds.Tables.Add(clibSeguridadCR.clsClienteSeg.getTblEstado(-1, ex.Message));
            }
            return ds;
        }


        public List<TablaCatalogo> GetCatalogos(string NombreCatalogo)
        {

            List<TablaCatalogo> Retorno = new List<TablaCatalogo>();

            TablaCatalogo TmpItem;

            DataSet ds = new DataSet();

            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            Logger.Logger loge = new Logger.Logger();
            string p_Log = ((string)System.Configuration.ConfigurationManager.AppSettings["RutaLog"]).Trim();
            try
            {
                xmlParam.DocumentElement.SetAttribute("NombreTabla", NombreCatalogo);

                loge.FilePath = p_Log;
                loge.WriteMensaje("Ejecucion EjecucionGralDs: " + xmlParam.DocumentElement.GetAttribute("NombreTabla"));
                loge.Linea();

                ClsGeneral ObjCtg = new ClsGeneral();
                ds = ObjCtg.EjecucionGralDs(xmlParam.DocumentElement.GetAttribute("NombreTabla"), 1, 1);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        TmpItem = new TablaCatalogo();
                        TmpItem.Codigo = Convert.ToString(item["Codigo"]);
                        TmpItem.Detalle = Convert.ToString(item["Detalle"]);
                        TmpItem.DescAlterno = Convert.ToString(item["DescAlterno"]);
                        Retorno.Add(TmpItem);
                    }
                }
                else
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
            }
            catch (Exception ex)
            {                

                loge.FilePath = p_Log;
                loge.WriteMensaje("Exception NombreCatalogo: " + ex.Message);
                loge.Linea();

                throw ex;
            }


            return Retorno;
        }


    }
}