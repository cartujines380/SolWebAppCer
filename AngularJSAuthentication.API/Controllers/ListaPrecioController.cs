using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJSAuthentication.API.Models;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Security.Claims;
using clibProveedores.Models;
using System.Threading;
using System.IO;
using System.Web;
using clibProveedores;
namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/ListaPrecio")]
    public class ListaPrecioController : ApiController
    {
        [ActionName("consultaListaPreciosgeneral")]
        [HttpGet]
        public FormResponseNotificacion consultaListaPreciosgeneral(string tipoListag, string rucg, string regInicial, string RegFinal)
        {

            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            var wresulFactList =
                    new System.Xml.Linq.XDocument(
                            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                            new XElement("Root",
                                     new XElement("SecNotificacion",
                                          new XAttribute("Ruc", rucg != null ? rucg : ""),
                                          new XAttribute("RegInicial", regInicial != null ? regInicial : ""),
                                          new XAttribute("RegFinal", RegFinal != null ? RegFinal : ""),
                                     new XAttribute("TipoLista", tipoListag != null ? tipoListag : ""))));
            ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 412, 1);

            if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
            {
                
                FormResponse.success = true;
                List<Producto> Retorno = new List<Producto>();

                if (tipoListag == "3")
                {

                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new Producto
                               {
                                   Codigo = reg.Field<String>("Anio"),
                                   Descripcion = reg.Field<String>("Mes"),
                                   Archivo = reg.Field<String>("Archivo"),


                               }).ToList<Producto>();
                }
                else
                {

                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new Producto
                               {
                                   Codigo = reg.Field<String>("Codigo"),
                                   Descripcion = reg.Field<String>("Descripcion"),
                                   Precio = reg.Field<Decimal>("Precio"),
                                   FechaPublicacion = reg.Field<DateTime>("FechaPublicacion"),
                                   NumRegistro = reg.Field<Int64>("rownumber").ToString(),

                               }).ToList<Producto>();

                    FormResponse.msgError = ds.Tables[1].Rows[0]["Total"].ToString();
                }
                FormResponse.root.Add(Retorno);
            }
            else { 
            FormResponse.success = false;
            FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
          
           }

            
            return FormResponse;
        }

        [ActionName("registraProveedorgeneral")]
        [HttpGet]
        public FormResponseNotificacion GetregistraMensaje(string nombreg, string emailg, string telefonog, string celularg, string productosg)
        {
            FormResponseNotificacion FormResponse = new FormResponseNotificacion();
            FormResponse.success = true;
            //string secuencia = "";
            DataSet ds = new DataSet();
            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {

                var wresulFactList =
                new System.Xml.Linq.XDocument(
                        new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("Root",
                                 new XElement("SecNotificacion",
                                 new XAttribute("Usuario", nombreg != null ? nombreg : ""),
                                 new XAttribute("Correo", emailg != null ? emailg : ""),
                                 new XAttribute("Celular", celularg != null ? celularg : ""),
                                 new XAttribute("Productos", productosg != null ? productosg : ""),
                                 new XAttribute("Telefono", telefonog != null ? telefonog : "")
                                 )));

                ds = objEjecucion.EjecucionGralDs(wresulFactList.ToString(), 411, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {

                    FormResponse.success = true;

                    string enviaCorreo = AppConfig.enviaCorreoProveedor;
                    if (enviaCorreo != "")
                    { 

                    try
                            {
                            string asuntoEmail = "";
                        
                        asuntoEmail = "Portal de Proveedores - Solicitud de Proveedor";
                            PI_NombrePlantilla = "NuevoProveedor.html"; //RFD0 - 2022 - 155

                            AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new AngularJSAuthentication.API.WCFEnvioCorreo.ServEnvioClientSoapClient();
                        if (!String.IsNullOrEmpty(enviaCorreo))
                                {

                                #region RFD0-2022-155 CORREO
                                Variables = new Dictionary<string, string>();
                                Variables.Add("@@NombreUsuario", nombreg);
                                Variables.Add("@@Email", emailg);
                                Variables.Add("@@Telefono", telefonog);
                                Variables.Add("@@Celular", celularg);
                                Variables.Add("@@Productos", productosg);
                                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();

                                #endregion

                                #region RFD0-2022-155 CORREO
                                byte[] data = System.Text.Encoding.ASCII.GetBytes("TEST");

                                Thread t = new Thread(() => objEnvMail.EnviaCorreoApi(
                                    "", enviaCorreo, "", "", asuntoEmail, "", true, true, false, data, null, PI_NombrePlantilla,
                    PI_Variables));
                                t.Start();
                                #endregion


                                //Thread t = new Thread(() => objEnvMail.EnviarCorreo("", enviaCorreo, "", "", asuntoEmail, mensajeEmailparameter, true, true, false, null));
                                //    t.Start();
                                   
                                }
                            }
                            catch (Exception)
                            {
                            }
                }

                }
                else
                {
                    FormResponse.success = false;
                    FormResponse.msgError = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
                    return FormResponse;
                }
                //else
                //    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());


            }
            catch (Exception e)
            {
                FormResponse.success = false;
                FormResponse.msgError = e.Message.ToString();
                return FormResponse;
            }
            return FormResponse;
        }

    }
}
