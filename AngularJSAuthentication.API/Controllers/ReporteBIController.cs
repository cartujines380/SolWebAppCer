using AngularJSAuthentication.API.Controllers;
using AngularJSAuthentication.API.Handlers;
using AngularJSAuthentication.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PowerBIEmbedded_AppOwnsData.Controllers
{
    [RoutePrefix("api/ReporteBI")]
    public class ReporteBIController : ApiController
    {



        public ReporteBIController()
        {

        }

        [AntiForgeryValidate]
        [ActionName("consToken")]
        [HttpPost]
        public FormResponseModelo consToken(string report)
        {
            FormResponseModelo FormResponse = new FormResponseModelo();
            try
            {
                AngularJSAuthentication.API.ProcesoWs.ServBaseProceso datos = new AngularJSAuthentication.API.ProcesoWs.ServBaseProceso();
                var doc = InitialiseService.xmlPerfilSitio;
                var Username = doc.SelectNodes("//Registro/@UsrReportBI")[0].InnerText;
                var Password = doc.SelectNodes("//Registro/@PassReportBI")[0].InnerText;
                //var Username = "jtorres@sipecom.com";
                //var Password = "Andresep2023**";
                //var Username = "cmarquez@CorporacionRosado.onmicrosoft.com";
                //var Password = "Mabb760211";

                var CatalogoBI = "BI_PARAMETROS";
                var ApplicationId = GetCatalogo(CatalogoBI, "ID-APLIC");
                var WorkspaceId = GetCatalogo(CatalogoBI, "ID-WORKS");
                //var ReportId = GetCatalogo(CatalogoBI, "ID-REPORT");
                var ReportId = GetCatalogo(CatalogoBI, report);
                //var ApplicationId = "1be71c23-f58f-4552-a730-94139e776322";
                //var WorkspaceId = "dda67c6d-6ee4-4459-8cda-706a65ea6c01";
                //var ReportId = "3a5c9e87-6c37-4093-9e99-285309fb9b4c";
                //var ApplicationId = "193757c8-f3c1-4b57-a3b6-a481f9125591";
                //var WorkspaceId = "0246052e-23bd-4c5a-9a92-3b08ce0f30a4";
                //var ReportId = "44f24728-19d4-4dd5-bde8-32d663ef2930";
                var embedResult = datos.consultaTokenBI(Username, Password, ApplicationId, WorkspaceId, ReportId);
                FormResponse.root.Add(embedResult.EmbedConfig);
                FormResponse.lSuccess = embedResult.Resultado;
                FormResponse.cMsgError = embedResult.EmbedConfig.ErrorMessage == null ? "" : GetCatalogo("BI_MENSAJES", embedResult.EmbedConfig.ErrorMessage);
            }
            catch (Exception ex)
            {
                FormResponse.lSuccess = false;
                FormResponse.cMsgError = ex.Message.ToString();
            }
            return FormResponse;
        }

        private string GetCatalogo(string nombreCatalogo, string parametro)
        {
            string respuesta = "";
            try
            {
                List<repCatalogo> lst_catalogo;
                ToleranciaController catalago = new ToleranciaController();
                var res = catalago.catalogoTipoPedido("1", nombreCatalogo);
                if (res.lSuccess)
                {
                    if (res.root.Count() > 0)
                    {
                        var lista = res.root[0];
                        lst_catalogo = (List<repCatalogo>)lista;
                        respuesta = lst_catalogo.Where(x => x.codigo == parametro).FirstOrDefault().descripcion;                    }
                }
            }
            catch (Exception)
            {
                respuesta = "";
            }
            return respuesta;
        }
    }
}