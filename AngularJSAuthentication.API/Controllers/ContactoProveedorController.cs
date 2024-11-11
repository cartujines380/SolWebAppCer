using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AngularJSAuthentication.API.Models;
using System.Data;

using System.Xml;
using System.Xml.Linq;
using clibProveedores;
using System.Web;
using Renci.SshNet;
using System.IO;
using SAP.Middleware.Connector;
using System.Threading;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AngularJSAuthentication.API.Controllers
{


    //[Authorize]

    [RoutePrefix("api/ContactoProveedor")]

    public class ContactoProveedorController : ApiController
    {
      
        
    
        [ActionName("ContactoGraba")]
        [HttpPost]
        public FormResponseContacto GetContactoGraba(DMContactos SolContacto)
        {
            //Consultar los contactos grabados en SAP
            DMContactos solContactoNew = new DMContactos();
            FormResponseContacto resConsulta = new FormResponseContacto();
            List<DMSolcitudProveedor.SolProvContacto> RetornoCons = new List<DMSolcitudProveedor.SolProvContacto>();
            List<DMSolcitudProveedor.SolProvContactoAlm> RetornoConsAlm = new List<DMSolcitudProveedor.SolProvContactoAlm>();
            //resConsulta = getProveedorContactoList(SolContacto.p_CodigoSAP);
            ////Quitar la informacion del usuario que se modifica
            //var listaContactos = resConsulta.root[0];
            //RetornoCons = (List<DMSolcitudProveedor.SolProvContacto>)listaContactos;
            //RetornoCons.RemoveAll(x => x.Identificacion == SolContacto.p_Identificacion);
            ////RetornoCons.RemoveAll(x => x.Identificacion != "");
            //var listaAlmacenes = resConsulta.root[1];
            //RetornoConsAlm = (List<DMSolcitudProveedor.SolProvContactoAlm>)listaAlmacenes;           
            //RetornoConsAlm.RemoveAll(x => x.pCodUsuario == SolContacto.p_Identificacion);


            try
            {

                auditoriaTransaccion(Serialize(SolContacto), "5001");
                //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(datos.GetType());
                //x.Serialize(Console.Out, datos);
                //Console.WriteLine();
                //Console.ReadLine();
            }
            catch (Exception ex)
            {

            }

            
            //RetornoConsAlm.RemoveAll(x => x.pCodUsuario != "");
            //Agregar contacto modificado/agregado a los demas consultado para grabar en SAP
            foreach(var i in SolContacto.p_SolProvContacto)
            {
                DMSolcitudProveedor.SolProvContacto addContacto = new DMSolcitudProveedor.SolProvContacto();
                

                 addContacto.Id  = i.Id;
                 addContacto.IdSolContacto  = i.IdSolContacto;
                 addContacto.IdSolicitud  = i.IdSolicitud;
                 addContacto.TipoIdentificacion = i.TipoIdentificacion;
                 addContacto.DescTipoIdentificacion  = i.DescTipoIdentificacion;
                 addContacto.Identificacion = i.Identificacion;
                 addContacto.Nombre2 = i.Nombre2;
                 addContacto.Nombre1  = i.Nombre1;
                 addContacto.Apellido2 = i.Apellido2;
                 addContacto.Apellido1 = i.Apellido1;
                 addContacto.CodSapContacto = i.CodSapContacto;
                 addContacto.PreFijo = i.PreFijo;
                 addContacto.DepCliente = i.DepCliente;  
                 addContacto.Departamento = i.Departamento;
                 addContacto.Funcion = i.Funcion;
                 addContacto.RepLegal = i.RepLegal;
                 addContacto.Estado = i.Estado;
                 addContacto.TelfFijo = i.TelfFijo;
                 addContacto.TelfFijoEXT  = i.TelfFijoEXT;
                 addContacto.TelfMovil = i.TelfMovil;
                 addContacto.EMAIL = i.EMAIL;
                 addContacto.DescFuncion = i.DescFuncion;
                 addContacto.DescDepartamento = i.DescDepartamento;
                 addContacto.NotElectronica = i.NotElectronica;
                 addContacto.NotTransBancaria = i.NotTransBancaria;
                 addContacto.pRecActas = i.pRecActas;
                 RetornoCons.Add(addContacto);
               
            }

            //Agregar almacenes a los demas consultado para grabar en SAP
            foreach (var i in SolContacto.p_ListaAlm)
            {
                DMSolcitudProveedor.SolProvContactoAlm addAlmacen = new DMSolcitudProveedor.SolProvContactoAlm();


                addAlmacen.pCodAlmacen = i.pCodAlmacen;
                addAlmacen.pCodCiudad = i.pCodCiudad;
                addAlmacen.pCodUsuario = i.pCodUsuario;
                addAlmacen.pNomAlmacen = i.pNomAlmacen;                
                RetornoConsAlm.Add(addAlmacen);
             

            }

            FormResponseContacto resultado = new FormResponseContacto();
            //DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
            
                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");

                foreach (DMSolcitudProveedor.SolProvContacto erow in RetornoCons)
                {
               
                    XmlElement elem = xmlParam.CreateElement("Contacto");
                    elem.SetAttribute("Id", erow.Id != null ? erow.Id.ToString() : "");
                    elem.SetAttribute("IdSolContacto", erow.IdSolContacto != null ? erow.IdSolContacto.ToString(): "");
                    elem.SetAttribute("IdSolicitud", erow.IdSolicitud != null ? erow.IdSolicitud.ToString() : "");
                    elem.SetAttribute("CodProveedor", SolContacto.p_CodigoSAP.ToString());
                    elem.SetAttribute("TipoIdentificacion", erow.TipoIdentificacion.ToString());
                    elem.SetAttribute("DescTipoIdentificacion", erow.DescTipoIdentificacion != null ? erow.DescTipoIdentificacion.ToString() : ""  );
                    elem.SetAttribute("Identificacion", erow.Identificacion.ToString());
                    elem.SetAttribute("Nombre2", erow.Nombre2.ToString());
                    elem.SetAttribute("Nombre1", erow.Nombre1.ToString());
                    elem.SetAttribute("Apellido2", erow.Apellido2.ToString());
                    elem.SetAttribute("Apellido1", erow.Apellido1.ToString());
                    elem.SetAttribute("CodSapContacto", erow.CodSapContacto.ToString());
                    elem.SetAttribute("PreFijo", erow.PreFijo.ToString());
                    elem.SetAttribute("DepCliente", erow.DepCliente.ToString());
                    elem.SetAttribute("Departamento", erow.Departamento.ToString());
                    elem.SetAttribute("Funcion", erow.Funcion != null ? erow.Funcion.ToString() : "");
                    elem.SetAttribute("RepLegal", erow.RepLegal == true ? "1" : "0");
                    elem.SetAttribute("Estado", erow.Estado == true ? "1":"0");
                    elem.SetAttribute("TelfFijo", erow.TelfFijo != null ? erow.TelfFijo.ToString() : "");
                    elem.SetAttribute("TelfFijoEXT", erow.TelfFijoEXT != null ? erow.TelfFijoEXT.ToString() : "");
                    elem.SetAttribute("TelfMovil", erow.TelfMovil != null ? erow.TelfMovil.ToString() : "");
                    elem.SetAttribute("EMAIL", erow.EMAIL != null ? erow.EMAIL.ToString() : "");
                    elem.SetAttribute("DescFuncion", erow.DescFuncion != null ? erow.DescFuncion.ToString() : "");
                    elem.SetAttribute("DescDepartamento", erow.DescDepartamento != null ? erow.DescDepartamento.ToString(): "");
                    elem.SetAttribute("NotElectronica", erow.NotElectronica == true ? "1":"0");
                    elem.SetAttribute("NotTransBancaria", erow.NotTransBancaria == true ? "1" : "0");
                    elem.SetAttribute("RecActas", erow.pRecActas == true ? "1" : "0");
                
                    xmlParam.DocumentElement.AppendChild(elem);
                }
                //XmlElement elemIdentif = xmlParam.CreateElement("Identificacion");
                //elemIdentif.SetAttribute("Identificacion", SolContacto.p_Identificacion.ToString());
                //xmlParam.DocumentElement.AppendChild(elemIdentif);
                if (SolContacto.p_SolZonas != null)
                {
                    //foreach (DMContactos.SolProvZonas erow in SolContacto.p_SolZonas)
                    //{
                    //    XmlElement elem = xmlParam.CreateElement("Zona");
                    //    elem.SetAttribute("Codigo", erow.Id != null ? erow.Id.Substring(erow.Id.Length - 2) : "");

                    //    elem.SetAttribute("CodProveedor", SolContacto.p_CodigoSAP.ToString());

                    //    xmlParam.DocumentElement.AppendChild(elem);
                    //}
                }


                //Listado de usuarios-almacenes
                CatalogosController catalag = new CatalogosController();
                var catCiudad = catalag.GetCatalogos("tbl_Ciudad");
                XmlDocument xmlParam2 = new XmlDocument();
                xmlParam2.LoadXml("<Root />");

                foreach (DMSolcitudProveedor.SolProvContactoAlm erow in RetornoConsAlm)
                {

                    XmlElement elem2 = xmlParam2.CreateElement("UsrAlmacen");
                    elem2.SetAttribute("Identificacion", erow.pCodUsuario.ToString());
                    elem2.SetAttribute("CodAlmacen", erow.pCodAlmacen.ToString());
                    var catCiu = catCiudad.Where(x => x.Codigo == erow.pCodCiudad).FirstOrDefault();
                    var regPais = catCiu == null ? "" :catCiu.DescAlterno;
                    elem2.SetAttribute("CodPais", regPais == "" ? "" : regPais.Split('-')[0]);
                    elem2.SetAttribute("CodRegion", regPais == "" ? "" : regPais.Split('-')[1]);
                    elem2.SetAttribute("CodCiudad", erow.pCodCiudad.ToString());

                    xmlParam2.DocumentElement.AppendChild(elem2);
                }


                var workBapi = AppConfig.WorkBAPI;
                //var workBapi = "N";
                if (workBapi == "S" && SolContacto.p_Accion == "S")
                {
                    string res = "",cod ="";
                    var ValorTokenUser = string.Empty;
                    ValorTokenUser = InitialiseService.GetTokenUser();
                    ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                    Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                    
                        var resp= Proceso.getActualizaContactoListBD(InitialiseService.Semilla,
                        ValorTokenUser, InitialiseService.IdOrganizacion,
                        xmlParam, xmlParam2, 1, SolContacto.p_CodigoSAP.ToString());

                    XmlDocument respuesta = new XmlDocument();
                respuesta.LoadXml(resp.OuterXml);

                    XmlNodeList xRegistro = respuesta.GetElementsByTagName("Registro");

                    int CodError = Convert.ToInt32(respuesta.DocumentElement.GetAttribute("CodError"));
                    string MsgError = respuesta.DocumentElement.GetAttribute("MsgError");

                    if (CodError < 0 || MsgError.Length > 0)
                    {
                        resultado.success = false;
                        resultado.mensaje = MsgError;
                        return resultado;
                    }
                    else
                    {
                        XmlNodeList xMensajeProc = ((XmlElement)xRegistro[0]).GetElementsByTagName("Row");
                        foreach (XmlElement nodo in xMensajeProc)
                        {
                            cod = nodo.GetAttribute("cod_error");
                            res = nodo.GetAttribute("msg_proc");
                        }
                         

                        if (cod.Equals("1000"))
                        {
                            resultado.success = true;
                            resultado.mensaje = res;
                            return resultado;
                        }
                        else
                        {
                            resultado.success = false;
                            resultado.mensaje = res;
                            return resultado;
                        }
                    }
                }

            }catch(Exception e){
                resultado.success = false;
                resultado.mensaje = e.Message.ToString();
            }
            return resultado;
        }


        private Boolean auditoriaTransaccion(string datos,string transaccion)
        {
            Boolean ds = false;
            ClsGeneral objEjecucion = new ClsGeneral();
            try
            {
                string cabe="<?xml version=\"1.0\" encoding=\"utf-16\"?>";
                    // encoding="utf-16"?>';
                datos=datos.Replace(cabe,"");
                ds = objEjecucion.EjecucionVerificar(datos, Convert.ToInt32(transaccion), 1);
            }
            catch (Exception ex)
            {
                ds = false;
            }
            return ds;

        }
        public static string Serialize(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (StringWriter stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }

        [ActionName("ProveedorContactoList")]
        [HttpGet]
        public IEnumerable<DMSolcitudProveedor.SolProvContacto> getProveedorContactoList(String idproveedorconta)
        {

            IEnumerable<DMSolcitudProveedor.SolProvContacto> Retorno = new List<DMSolcitudProveedor.SolProvContacto>();
            DataSet ds = new DataSet();
            ClsGeneral objEjecucion = new ClsGeneral();
            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            XmlElement elem = xmlParam.CreateElement("Contacto");


            clibLogger.clsLogger p_Log = new clibLogger.clsLogger();
            p_Log.Graba_Log_Info("ContactoProveedorController" + " -- getProveedorContactoList " + " INI ");

            var ValorTokenUser = string.Empty;
            ValorTokenUser = InitialiseService.GetTokenUser();

            //List<DMSolcitudProveedor.SolProvContacto> Retorno = new List<DMSolcitudProveedor.SolProvContacto>();
            //List<DMSolcitudProveedor.SolProvContacto> Retorno2 = new List<DMSolcitudProveedor.SolProvContacto>();
            //List<DMSolcitudProveedor.SolProvContactoAlm> RetornoAlm = new List<DMSolcitudProveedor.SolProvContactoAlm>();
            try
            {              
                //Consulta a BD local
                elem.SetAttribute("CodProveedor", idproveedorconta.ToString());
                xmlParam.DocumentElement.AppendChild(elem);
                ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 206, 1);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    string jsonString = JsonConvert.SerializeObject(ds.Tables[0].AsEnumerable());
                    p_Log.Graba_Log_Info("getProveedorContactoList:" + " -- DATA : " + jsonString);

                    Retorno = (from reg in ds.Tables[0].AsEnumerable()
                               select new DMSolcitudProveedor.SolProvContacto
                               {
                                   Apellido1 = reg.Field<String>("Apellido1"),
                                   Apellido2 = reg.Field<String>("Apellido2"),
                                   CodSapContacto = reg.Field<String>("CodSapContacto"),
                                   Departamento = reg.Field<String>("Departamento"),
                                   DepCliente = reg.Field<String>("DepCliente"),
                                   DescTipoIdentificacion = reg.Field<String>("DescTipoIdentificacion"),
                                   EMAIL = reg.Field<String>("EMAIL"),
                                   Estado = reg.Field<Boolean>("Estado"),
                                   Funcion = reg.Field<String>("Funcion"),
                                   Identificacion = reg.Field<String>("Identificacion"),
                                   IdSolContacto = reg.Field<Int64>("Id") != null ? reg.Field<Int64>("Id").ToString() : "",
                                   IdSolicitud = "0",//reg.Field<Int64>("IdSolicitud") != null ? reg.Field<Int64>("IdSolicitud").ToString() : "",
                                   NotElectronica = (reg.Field<Boolean?>("NotElectronica") != null) ? reg.Field<Boolean>("NotElectronica"): false,
                                   NotTransBancaria = (reg.Field<Boolean?>("NotTransBancaria") != null) ? reg.Field<Boolean>("NotTransBancaria") : false,
                                   Nombre1 = reg.Field<String>("Nombre1"),
                                   Nombre2 = reg.Field<String>("Nombre2"),
                                   PreFijo = reg.Field<String>("PreFijo"),
                                   RepLegal = reg.Field<Boolean>("RepLegal"),
                                   TelfFijo = reg.Field<String>("TelfFijo"),
                                   TelfFijoEXT = reg.Field<String>("TelfFijoEXT"),
                                   TelfMovil = reg.Field<String>("TelfMovil"),
                                   TipoIdentificacion = reg.Field<String>("TipoIdentificacion"),
                                   DescDepartamento = reg.Field<String>("DescDepartamento"),
                                   DescFuncion = reg.Field<String>("DescDepartamento"),
                                   EstadoCivil = reg.Field<String>("EstadoCivil"),
                                   RegimenMatrimonial = reg.Field<String>("RegimenMatrimonial"),
                                   ConyugeTipoIdentificacion = reg.Field<String>("ConyugeTipoIdentificacion"),
                                   ConyugeIdentificacion = reg.Field<String>("ConyugeIdentificacion"),
                                   ConyugeNombres = reg.Field<String>("ConyugeNombres"),
                                   ConyugeApellidos = reg.Field<String>("ConyugeApellidos"),
                                   ConyugeFechaNac = (reg.Field<DateTime?>("ConyugeFechaNac") != null) ? reg.Field<DateTime>("ConyugeFechaNac") : DateTime.MinValue,
                                   ConyugeNacionalidad = reg.Field<String>("ConyugeNacionalidad"),
                                   FechaNacimiento = (reg.Field<DateTime?>("FechaNacimiento") != null) ? reg.Field<DateTime>("FechaNacimiento") : DateTime.MinValue,
                                   Nacionalidad = reg.Field<String>("Nacionalidad"),
                                   Residencia = reg.Field<String>("PaisResidencia"),
                                   RelacionDependencia = reg.Field<String>("RelacionDependencia"),
                                   AntiguedadLaboral = reg.Field<String>("AntiguedadLaboral"),
                                   TipoIngreso = reg.Field<String>("TipoIngreso"),
                                   IngresoMensual = reg.Field<String>("IngresoMensual"),
                                   TipoParticipante = reg.Field<String>("TipoParticipante"),
                               }).ToList<DMSolcitudProveedor.SolProvContacto>();

                }
                else
                {
                    p_Log.Graba_Log_Error("getProveedorContactoList:" + " -- Error_ESTADO : " + ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                    throw new Exception(ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString());
                }


                //var workBapi = AppConfig.WorkBAPI;
                ////var workBapi = "N";
                //if (workBapi == "S")
                //{  //Invocar Web services para consultar contactos en SAP    
                //    //Catalogos
                //    CatalogosController catalag = new CatalogosController();
                //    var catDepartamentos = catalag.GetCatalogos("tbl_DepartaContacto");
                //    var catFunciones = catalag.GetCatalogos("tbl_FuncionContacto");
                //    ProcesoWs.ServBaseProceso Proceso = new ProcesoWs.ServBaseProceso();
                //    Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                //    var datos = Proceso.getProveedorContactoList(InitialiseService.Semilla,
                //        ValorTokenUser,
                //        InitialiseService.IdOrganizacion,
                //        idproveedorconta);

                //    try
                //    {

                //        auditoriaTransaccion(Serialize(datos), "5000");
                //        //System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(datos.GetType());
                //        //x.Serialize(Console.Out, datos);
                //        //Console.WriteLine();
                //        //Console.ReadLine();
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //    var idUnico = 0;
                //    foreach (var a in datos)
                //    {
                //        DMSolcitudProveedor.SolProvContacto aux = new DMSolcitudProveedor.SolProvContacto();
                //        idUnico++;
                //        aux.Identificacion = a.Identificacion != null ? a.Identificacion : "";

                //        aux.Id = idUnico.ToString();
                //        aux.Apellido1 = a.Apellido1 != null?  a.Apellido1 : "";
                //        aux.Apellido2 = a.Apellido2 != null ? a.Apellido2 : "";
                //        aux.CodSapContacto = a.CodSapContacto != null ? a.CodSapContacto : "";
                //        aux.Departamento = a.Departamento != null ? a.Departamento : "";
                //        var catDep = catDepartamentos.Where(x => x.Codigo == aux.Departamento).FirstOrDefault();
                //        aux.DescDepartamento = catDep != null ? catDep.Detalle : "";
                //        aux.DepCliente = a.DepCliente != null ? a.DepCliente : "";
                //        aux.DescTipoIdentificacion = a.DescTipoIdentificacion != null ? a.DescTipoIdentificacion : "";
                //        aux.EMAIL = a.EMAIL != null ? a.EMAIL : "";

                //        aux.Funcion = a.Funcion != null ? a.Funcion : "";
                //        var catFun = catFunciones.Where(x => x.Codigo == aux.Funcion).FirstOrDefault();
                //        aux.DescFuncion = catFun != null ? catFun.Detalle : "";
                //        aux.IdSolContacto = a.IdSolContacto != null ? a.IdSolContacto : "";
                //        aux.IdSolicitud = a.IdSolicitud != null ? a.IdSolicitud : "";
                //        aux.Nombre1 = a.Nombre1 != null ? a.Nombre1 : "";
                //        aux.Nombre2 = a.Nombre2 != null ? a.Nombre2 : "";
                //        aux.RepLegal = a.RepLegal;
                //        aux.NotElectronica = a.NotElectronica;
                //        aux.NotTransBancaria = a.NotTransBancaria;
                //        //var f = Retorno2.Where(x => x.Identificacion == aux.Identificacion && x.Departamento == aux.Departamento && x.Funcion == aux.Funcion).FirstOrDefault();
                //        //if (f != null)
                //        //{
                //            //aux.NotElectronica = f.NotElectronica != null ? f.NotElectronica : false;
                //            //aux.NotTransBancaria = f.NotTransBancaria != null ? f.NotTransBancaria : false;
                //            //aux.Estado = f.Estado != null ? f.Estado : false;
                //            //aux.RepLegal = f.RepLegal != null ? f.RepLegal : false;
                //        //}
                //        //else
                //        //{
                //            //aux.NotElectronica = false;
                //            //aux.NotTransBancaria = false;
                //            //aux.Estado = false;
                //            //aux.RepLegal = false;
                //        //}

                //        aux.PreFijo = a.PreFijo != null ? a.PreFijo : "";                        
                //        aux.TelfFijo = a.TelfFijo != null ? a.TelfFijo : "";
                //        aux.TelfFijoEXT = a.TelfFijoEXT != null ? a.TelfFijoEXT : "";
                //        aux.TelfMovil = a.TelfMovil != null ? a.TelfMovil : "";
                //        aux.TipoIdentificacion = a.TipoIdentificacion != null ? a.TipoIdentificacion : "";
                //        aux.pRecActas = a.actas == "X" ? true : false;
                //        aux.Estado = a.Estado;
                //      // if(aux.Identificacion != "")
                //        Retorno.Add(aux);
                //    }
                //    //Estructura Usr-Ciudad-Almacen
                //    var datosCiudades = Proceso.getProveedorContactoListCiudad(InitialiseService.Semilla,
                //        ValorTokenUser,
                //        InitialiseService.IdOrganizacion, 
                //        idproveedorconta);


                //    foreach(var e in datosCiudades)
                //    {
                //       DMSolcitudProveedor.SolProvContactoAlm aux2 = new DMSolcitudProveedor.SolProvContactoAlm();

                //        aux2.pCodUsuario  = e.Identificacion != null ?  e.Identificacion : "";
                //        aux2.pCodAlmacen  = e.codigoAlmacen != null ? e.codigoAlmacen : "";                       
                //        aux2.pCodCiudad  = e.codigoCiudad != null ?  e.codigoCiudad : "";
                //        RetornoAlm.Add(aux2);
                //    }

                //    //var RetornoOrd = Retorno.OrderBy(x => x.Nombre1);
                //    resultado.success = true;
                //    resultado.root.Add(Retorno);
                //    resultado.root.Add(RetornoAlm);
                //}
                //else
                //{
                //    resultado.success = true;
                //    if (Retorno2.Count > 0)
                //        resultado.root.Add(Retorno2);

                //}


            }
            catch (Exception ex)
            {
                p_Log.Graba_Log_Error("getProveedorContactoList:" + " -- Error : " + ex.Message.ToString());
                throw ex;
            }

            return Retorno;

        }

        [ActionName("UsuarioZonaList")]
        [HttpGet]
        public FormResponseContacto getUsuarioZonaList(String idproveedorconta, String Usuario)
        {

            FormResponseContacto resultado = new FormResponseContacto();
            //DataSet ds = new DataSet();
            //ClsGeneral objEjecucion = new ClsGeneral();
            //XmlDocument xmlParam = new XmlDocument();
            //xmlParam.LoadXml("<Root />");
            //XmlElement elem = xmlParam.CreateElement("Contacto");


            //List<DMContactos.SolProvZonas> Retorno = new List<DMContactos.SolProvZonas>();
            //try
            //{
                
                
               
            //       elem.SetAttribute("CodProveedor", idproveedorconta.ToString());
            //       xmlParam.DocumentElement.AppendChild(elem);
            //       XmlElement elemIdentif = xmlParam.CreateElement("Identificacion");
            //       elemIdentif.SetAttribute("Identificacion", Usuario.ToString());
            //       xmlParam.DocumentElement.AppendChild(elemIdentif);
            //       ds = objEjecucion.EjecucionGralDs(xmlParam.OuterXml, 206, 1);
            //       if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
            //       {
            //           Retorno = (from reg in ds.Tables[0].AsEnumerable()
            //                      select new DMContactos.SolProvZonas
            //                      {
            //                          Id = reg.Field<String>("Id"),


            //                      }).ToList<DMContactos.SolProvZonas>();
            //           resultado.success = true;
            //           if (Retorno.Count > 0)
            //           resultado.root.Add(Retorno);
            //       }
            //       else
            //       { 
            //          resultado.success = false;
            //          resultado.mensaje = ds.Tables["TblEstado"].Rows[0]["MsgError"].ToString();
            //       }
                 
                
                

            //}
            //catch (Exception ex)
            //{
            //    resultado.success = false;
            //    resultado.mensaje = ex.Message.ToString();
            //    //throw new DataException(ex.Message);
            //}
            return resultado;

        }


        
    }


}

