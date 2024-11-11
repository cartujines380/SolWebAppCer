using AngularJSAuthentication.API.Custom;
using AngularJSAuthentication.API.Entities;
using AngularJSAuthentication.API.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace AngularJSAuthentication.API.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = new Client();
            client.Active = true;
            client.AllowedOrigin = "*";
            client.RefreshTokenLifeTime = 1;

            try
            {
                if (client == null)
                {
                    context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                    return Task.FromResult<object>(null);
                }



                if (client.ApplicationType == Models.ApplicationTypes.NativeConfidential)
                {
                    if (string.IsNullOrWhiteSpace(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret should be sent.");
                        return Task.FromResult<object>(null);
                    }
                    else
                    {
                        if (client.Secret != Helper.GetHash(clientSecret))
                        {
                            context.SetError("invalid_clientId", "Client secret is invalid.");
                            return Task.FromResult<object>(null);
                        }
                    }
                }

                if (!client.Active)
                {
                    context.SetError("invalid_clientId", "Client is inactive.");
                    return Task.FromResult<object>(null);
                }

                context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
                context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

                context.Validated();
                return Task.FromResult<object>(null);
            }
            catch (Exception ex) {
                throw ex;
            }

        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            string rucReceive = string.Empty;

            string sourceLogin = string.Empty;
            string resultadoxml = string.Empty;

            string IdParticipante = string.Empty;
            string NombreParticipante = string.Empty;

            string IdentParticipante = string.Empty;
            string Estado = string.Empty;
            string PS_Token = string.Empty;
            string EsClaveNuevo = string.Empty;
            string EsClaveCambio = string.Empty;
            string EsClaveBloqueo = string.Empty;
            string CodSAP = string.Empty;
            string CorreoE = string.Empty;
            string Celular = string.Empty;
            string EsAdmin = string.Empty;
            string Usuario = string.Empty;
            string CargoEmpleado = string.Empty;
            string esEtiqueta = string.Empty;

            string UsrParam = string.Empty;


            string tokenUser = string.Empty;

            string roles = string.Empty;

            string Rol = string.Empty;
            string nomEmpresa = string.Empty;
            string rucEmpresa = string.Empty;
            string sociedad = string.Empty;

            string apl = string.Empty;
            tokenUser = InitialiseService.PI_Session;

            UsrParam = System.Web.HttpContext.Current.Request.Form["username"].ToString();
            rucReceive = System.Web.HttpContext.Current.Request.Form["ruc"].ToString();
            sourceLogin = System.Web.HttpContext.Current.Request.Form["sl"].ToString();
            apl = System.Web.HttpContext.Current.Request.Form["apl"]?.ToString();
            try
            {
                sociedad = System.Web.HttpContext.Current.Request.Form["sociedad"].ToString();
            }
            catch (Exception)
            {

                sociedad = "";
            }

            ApplicationUser userapp = new ApplicationUser();

            clibEncriptaQS.clsEncriptaQS ObjEncrip2 = new clibEncriptaQS.clsEncriptaQS();

            string UsrParam4 = string.Empty;

            if (sourceLogin == "4")
            {
                UsrParam = context.UserName;
                UsrParam4 = ObjEncrip2.Decryp(UsrParam);
                UsrParam = UsrParam4;
            }
            else
                UsrParam = context.UserName;

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            //SE COMENTA X F5
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string RucSaved = string.Empty;



            if (sourceLogin == "1")
            {

                //ApplicationDbContext db2 = new ApplicationDbContext();

                //RucSaved = (from s in db2.Users where s.UserName == UsrParam select s.Ruc).First();

                ApplicationUser user = new ApplicationUser();
                using (AuthRepository _repo = new AuthRepository())
                {


                    try
                    {
                        //user = await _repo.FindUser(UsrParam, context.Password);
                        clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();
                        var CripPass = objEnc.Encripta(context.Password, InitialiseService.Semilla);
                        user = _repo.consultaUsuarioP(context.UserName, CripPass);
                        RucSaved = user.Ruc;

                        if (!user.EmailConfirmed)
                        {

                            NombreParticipante = string.Empty;
                            RucSaved = string.Empty;
                            context.SetError("invalid_grant", "Usuario no ha confirmado su cuenta de correo electrónico");
                            return;
                        }
                        NombreParticipante = user.UserName;


                    }
                    catch (Exception ee)
                    {
                        NombreParticipante = string.Empty;
                        RucSaved = string.Empty;
                        context.SetError("invalid_grant", "El usuario o contraseña es incorrecta.");
                        return;
                    }


                    if (rucReceive != RucSaved)
                        user = null;

                    if (user == null)
                    {
                        context.SetError("invalid_grant", "El usuario o contraseña es incorrecta.");
                        return;
                    }
                }
            }

            XmlDocument xmlResp = new XmlDocument();
            TransaccionUser tmpTrx;

            // Recuperar la lista menu, submenus, items





            //List<TransaccionUser> ListmpTrx=new List<TransaccionUser>();

            //InitialiseService.TransaccionesUsuario.re

            //int indice = InitialiseService.TransaccionesUsuario.Count;

            //for (int ind = 0; ind < indice; ind++)
            //{
            //    if (InitialiseService.TransaccionesUsuario[ind].UserId == context.UserName)
            //        InitialiseService.TransaccionesUsuario.RemoveAt(ind);
            //}

            //.RemoveAll(x =>x.Member.Name == shortName);

            InitialiseService.TransaccionesUsuario.RemoveAll(x => x.UserId == UsrParam);



            //InitialiseService.TransaccionsUser TransaccionesUsuario = new List<TransaccionsUser

            string IdUsername = string.Empty;
            string strtransaccion = string.Empty;

            if (sourceLogin == "2" || sourceLogin == "3" || sourceLogin == "4")
            {

                //LoginFS("1001402211001", "CER00008", "1001402211CER00008", "aaaa1111X");

                using (AuthRepository _repo = new AuthRepository())
                {

                    IdUsername = string.Empty;

                    if (sourceLogin == "2")
                        IdUsername = rucReceive.Substring(0, 10) + UsrParam;

                    if (sourceLogin == "3" || sourceLogin == "4")
                    {
                        rucReceive = string.Empty;
                        IdUsername = UsrParam;
                    }

                    resultadoxml = _repo.LoginFS(sourceLogin, rucReceive, IdUsername, UsrParam, context.Password, sociedad, apl);
                    xmlResp.LoadXml(resultadoxml);


                    if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
                    {

                        tokenUser = xmlResp.DocumentElement.GetAttribute("PXML_SESSION");

                        IdParticipante = xmlResp.DocumentElement.GetAttribute("IdParticipante");
                        NombreParticipante = xmlResp.DocumentElement.GetAttribute("NombreParticipante");
                        IdentParticipante = xmlResp.DocumentElement.GetAttribute("IdentParticipante");
                        Estado = xmlResp.DocumentElement.GetAttribute("Estado");
                        PS_Token = xmlResp.DocumentElement.GetAttribute("PS_Token");
                        EsClaveNuevo = xmlResp.DocumentElement.GetAttribute("EsClaveNuevo");
                        EsClaveCambio = xmlResp.DocumentElement.GetAttribute("EsClaveCambio");
                        EsClaveBloqueo = xmlResp.DocumentElement.GetAttribute("EsClaveBloqueo");
                        CodSAP = xmlResp.DocumentElement.GetAttribute("CodSAP");
                        CorreoE = xmlResp.DocumentElement.GetAttribute("CorreoE");
                        Celular = xmlResp.DocumentElement.GetAttribute("Celular");
                        EsAdmin = xmlResp.DocumentElement.GetAttribute("EsAdmin");
                        Usuario = xmlResp.DocumentElement.GetAttribute("Usuario");
                        CargoEmpleado = xmlResp.DocumentElement.GetAttribute("CargoEmpleado");

                        Rol = xmlResp.DocumentElement.GetAttribute("Rol");
                        rucEmpresa = xmlResp.DocumentElement.GetAttribute("rucEmpresa");
                        nomEmpresa = xmlResp.DocumentElement.GetAttribute("nomEmpresa");
                        esEtiqueta = xmlResp.DocumentElement.GetAttribute("EsEtiqueta");
                        sociedad = xmlResp.DocumentElement.GetAttribute("Sociedad");

                        XmlNodeList nodeList = xmlResp.GetElementsByTagName("Row");
                        int datos=0;

                        foreach (XmlNode no in nodeList)
                        {
                            try
                            {
                                strtransaccion = no.Attributes["Transaccion"].Value;
                                if (datos == 0)
                                    roles = strtransaccion;
                                else
                                    roles = roles + "," + strtransaccion;
                                datos++;
                                tmpTrx = new TransaccionUser();
                                tmpTrx.TransaccionId = strtransaccion;
                                tmpTrx.UserId = UsrParam;
                                InitialiseService.TransaccionesUsuario.Add(tmpTrx);
                            }
                            catch { }
                            

                        }



                    }
                    else
                    {

                        context.SetError("invalid_grant", xmlResp.DocumentElement.GetAttribute("MsgError"));
                        return;
                    }


                }


            }


            ////////////

            //List<TransaccionUser> ListmpTrx = new List<TransaccionUser>();

            //ListmpTrx=InitialiseService.TransaccionesUsuario.FindAll(x => x.UserId == context.UserName);


            //ApplicationDbContext db = new ApplicationDbContext();


            //string ApplicationId = string.Empty;

            //string clientId = string.Empty;
            //string clientSecret = string.Empty;


            //clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();
            //clientId = context.ClientId;

            //var DeSecretClientId = objEnc.Desencripta(clientId, InitialiseService.Semilla);

            //Items.Where(x => x.TestList.All(s => stateList.Contains(s.State)));

            //var resp=db.Menues.Where(x => x.Submenues.All(s => ListmpTrx.Contains(s.Menu.CodTransaccion)));

            //var resp = db.Menues.Where(p => ListmpTrx.All(c => p.Submenues.Contains(c)).ToList();

            //try
            //{
            //    var resp1 = db.Menues.Where(p => ListmpTrx.All(c => p.Submenues.Any(cat => cat.CodTransaccion == c.TransaccionId))).ToList();


            //}
            //catch (Exception ee)
            //{

            //    throw ee;
            //}


            //  var resp2 = db.Submenues.Where(p => ListmpTrx.All(c => p.Items.Any(cat => cat.CodTransaccion == c.TransaccionId))).ToList();

            //  var resp3 = db.Items.Where(p => ListmpTrx.All(c => p.CodTransaccion == c.TransaccionId)).ToList();



            ////////////



            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, UsrParam));
            identity.AddClaim(new Claim(ClaimTypes.Role, "WebUsers"));
            identity.AddClaim(new Claim("userName", UsrParam));
            identity.AddClaim(new Claim("ruc", rucReceive));
            identity.AddClaim(new Claim("tokenuser", tokenUser));
            //identity.AddClaim(new Claim("sub", context.UserName));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { 
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    { 
                        "userName", UsrParam
                    },
                    { 
                        "tokenUser", tokenUser
                    } 
                    ,
                    { 
                        "IdParticipante", IdParticipante
                    } 
                    ,
                    { 
                        "NombreParticipante", NombreParticipante
                    } 
                    ,
                    { 
                        "IdentParticipante", IdentParticipante
                    } 
                    ,
                    { 
                        "Estado", Estado
                    } 
                    ,
                    { 
                        "PS_Token", PS_Token
                    }  ,
                    { 
                        "EsClaveNuevo", EsClaveNuevo
                    }  ,
                    { 
                        "EsClaveCambio", EsClaveCambio
                    }  ,
                    { 
                        "EsClaveBloqueo", EsClaveBloqueo
                    }  ,
                    { 
                        "CodSAP", CodSAP
                    }  ,
                    { 
                        "CorreoE", CorreoE
                    }  ,
                    { 
                        "Celular", Celular
                    }  ,
                    { 
                        "EsAdmin", EsAdmin
                    }  ,
                    { 
                        "Usuario", Usuario
                    } 
                     ,
                    { 
                        "ruc", RucSaved
                    }  ,
                    { 
                        "CargoEmpleado", CargoEmpleado
                    } ,
                    { 
                        "roles", roles
                    } ,
                    {
                        "rolAdm", Rol
                    },
                    {
                        "rucEmpresa", rucEmpresa
                    },
                    {
                        "nomEmpresa", nomEmpresa
                    },
                    {
                        "esEtiqueta",esEtiqueta
                    }


                });


            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}