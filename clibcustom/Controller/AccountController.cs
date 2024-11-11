using AngularJSAuthentication.API.Models;
using AngularJSAuthentication.API.Results;
using clibCustom.Models;
using clibLogger;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace AngularJSAuthentication.API.Controllers
{

    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {

        //private ApplicationDbContext _ctx;

        //private UserManager<ApplicationUser> _userManager;

        public AccountController()
        {
            //_repo = new AuthRepository();
            //_ctx = new ApplicationDbContext();

            //_userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }


        //private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public static string Base64ForUrlEncode(string str)
        {
            var encbuff = Encoding.UTF8.GetBytes(str);
            return HttpServerUtility.UrlTokenEncode(encbuff);
        }

        public static string Base64ForUrlDecode(string str)
        {
            var decbuff = HttpServerUtility.UrlTokenDecode(str);
            return decbuff != null ? Encoding.UTF8.GetString(decbuff) : null;
        }


        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(ApplicationUser userModel)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";

            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            System.Collections.Generic.Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            Logger.Logger log1 = new Logger.Logger();
            log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
            string mensajeerror = "prueba1";
            log1.WriteMensaje(mensajeerror);

            FormResponse.success = false;

            if (userModel.Ruc == null)
            {
                ModelState.AddModelError("", "RUC es un dato requerido");

                mensajeerror = "RUC es un dato requerido";
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                return BadRequest(ModelState);
            }
            if (userModel.Email == null)
            {
                ModelState.AddModelError("", "Correo electrónico es un dato requerido");

                mensajeerror = "Correo electrónico es un dato requerido";
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                return BadRequest(ModelState);

            }

            if (!ModelState.IsValid)
            {
                mensajeerror = "antes del valid";
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                return BadRequest(ModelState);
            }

            mensajeerror = "antes de inicializar encripta";
            log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
            log1.WriteMensaje(mensajeerror);

            clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();
            formResponseSeguridad res = new formResponseSeguridad();

            string code = string.Empty;
            string callbackUrl = string.Empty;
            string rutaEmail = string.Empty;
            string pUrlConfirmacion = string.Empty;

            userModel.Email = userModel.UserName;
            userModel.Ruc = userModel.Ruc;

            var provider = new DpapiDataProtectionProvider("ngAuthApp3");
            //_userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, string>(provider.Create("Confirmation"))
            //    as IUserTokenProvider<ApplicationUser, string>;

            var CripPass = objEnc.Encripta(userModel.Password, InitialiseService.Semilla);
            var CripCod = objEnc.Encripta(userModel.UserName.ToLower() + "2015sipecomce", InitialiseService.Semilla);

            userModel.Password = CripPass;

            mensajeerror = "encripta";
            log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
            log1.WriteMensaje(mensajeerror);

            IdentityResult result;
            IHttpActionResult errorResult;
            try
            {
                mensajeerror = "antes de registrar";
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                //result = await _repo.RegisterUser(userModel);

                res = RegistraUser(userModel);

                mensajeerror = "luego de registrar";
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                errorResult = GetErrorResult(res);

                mensajeerror = "antes del errorresult";
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                if (errorResult != null)
                {
                    return errorResult;
                }
                else
                {
                    mensajeerror = "antes de cargar user";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);

                    ApplicationUser user;

                    try
                    {
                        user = consultaUsuario(userModel.Email);
                    }
                    catch (Exception exx)
                    {
                        mensajeerror = exx.Message;
                        log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                        log1.WriteMensaje(mensajeerror);

                        ModelState.AddModelError("", mensajeerror);

                        return BadRequest(ModelState);
                        throw;
                    }

                    mensajeerror = "luego de cargar user";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);

                    mensajeerror = "antes de generar code";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);
                    try
                    {
                        code = CripCod;
                    }
                    catch (Exception exx)
                    {
                        mensajeerror = exx.Message;
                        log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                        log1.WriteMensaje(mensajeerror);

                        ModelState.AddModelError("", mensajeerror);

                        return BadRequest(ModelState);

                    }
                    mensajeerror = "luego de generar code";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);

                    mensajeerror = "luego de generar html";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);

                    callbackUrl = Utility.AbsoluteUrlRef(string.Format("/portalproveedores/proveedor/Account/FrmConfirmEmail?code={0}&userId={1}&ruc={2}&email={3}&userPass={4}", code, user.Id, userModel.Ruc, userModel.Email, CripPass));

                    mensajeerror = "luego de callback";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);

                    PI_NombrePlantilla = "NewUserWeb.html"; //RFD0 - 2022 - 155

                    mensajeerror = "luego de hostenviroment";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);


                    //pUrlConfirmacion = "<a href=\"" + callbackUrl + "\">link</a>";

                    pUrlConfirmacion = callbackUrl;

                    //mensajeEmail = mensajeEmail.Replace("@@Ruc", userModel.Ruc);
                    //mensajeEmail = mensajeEmail.Replace("@@Usuario", userModel.Email);
                    //mensajeEmail = mensajeEmail.Replace("@@UrlConfirmacion", pUrlConfirmacion);


                    #region RFD0-2022-155 CORREO
                    Variables = new System.Collections.Generic.Dictionary<string, string>();
                    Variables.Add("@@Ruc", userModel.Ruc);
                    Variables.Add("@@Usuario", userModel.Email);
                    Variables.Add("@@UrlConfirmacion", pUrlConfirmacion);
                    #endregion

                    mensajeerror = "luego de generar correo";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);


                    mensajeerror = "antes de enviar correo";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);
    
                    
                    Thread t = new Thread(() => EnviarEmail(userModel.Email, "Confirmación de Correo Electrónico", "", PI_NombrePlantilla, Variables));
                    t.Start();

                    FormResponse.success = true;

                    mensajeerror = "luego de enviar correo";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);
                }

            }
            catch (Exception ee)
            {
                mensajeerror = "Error:" + ee.Message;
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                return BadRequest("Error:" + ee.Message);
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("ConfirmEmail")]
        [HttpPost]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirEmailViewModel model)
        {

            string userId = string.Empty;
            string code = string.Empty;
            string ruc = string.Empty;
            string userPass = string.Empty;

            formResponseSeguridad res = new formResponseSeguridad();

            userId = model.UserId;
            userPass = model.Password;
            code = model.Code;
            ruc = model.Ruc;

            Logger.Logger log1 = new Logger.Logger();
            log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorConfirmCe";

            var provider = new DpapiDataProtectionProvider("ngAuthApp3");

            //_userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, string>(provider.Create("Confirmation"))
            //    as IUserTokenProvider<ApplicationUser, string>;

            IdentityResult result = new IdentityResult();

            clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();

            ApplicationUser user = consultaUsuario(model.Email);

            if (user == null || userId == null || code == null)
            {
                ModelState.AddModelError("", "Error al confirmar Email.......");
                return BadRequest("No se encontró el token de confirmación de Correo Electrónico");
            }

            var DeCripPass = objEnc.Desencripta(userPass, InitialiseService.Semilla);
            user.Password = DeCripPass;

            var codcomp = objEnc.Encripta(user.UserName.ToLower() + "2015sipecomce", InitialiseService.Semilla);

            user.EmailConfirmed = true;

            try
            {
                if (codcomp != code)
                {
                    ModelState.AddModelError("", "Error al confirmar Email.......");
                    return BadRequest(ModelState);
                }
                else
                {
                    res = confirmedMail(model.Email);
                    if (res.success)
                        return Ok(new { message = "Su correo eléctronico ha sido confirmado" });
                    else
                    {
                        ModelState.AddModelError("", "Error al confirmar Email.......");
                        return BadRequest(ModelState);
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al confirmar Email.......");
                return BadRequest(ModelState);
            }
        }

        #region Reset Password
        // POST api/Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            #region RFD0-2022-155 Variables CORREO
            String PI_NombrePlantilla = string.Empty;
            String PI_Variables = string.Empty;
            System.Collections.Generic.Dictionary<string, string> Variables;
            #endregion RFD0-2022-155 Variables CORREO

            string urlorigen = string.Empty;

            urlorigen = System.Web.HttpContext.Current.Request.UrlReferrer.Scheme + "://" + System.Web.HttpContext.Current.Request.UrlReferrer.Authority.ToString();

            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";

            FormResponse.success = false;

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var provider = new DpapiDataProtectionProvider("ngAuthApp3");
            //_userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, string>(provider.Create("ResetPassword"))
            //    as IUserTokenProvider<ApplicationUser, string>;

            ApplicationUser user = consultaUsuario(model.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                ModelState.AddModelError("", "El usuario no existe, por favor verificar.");

                FormResponse.success = false;
                FormResponse.codError = "-1";
                FormResponse.msgError = "El usuario no existe, por favor verificar.";

                return BadRequest(ModelState);
            }

            string code = string.Empty;

            clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();

            var CripCod = objEnc.Encripta(model.Email.ToLower() + "|2015sipecomfp", InitialiseService.Semilla);

            try
            {
                code = CripCod;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            //var callbackUrl = Utility.AbsoluteUrlRef("/Account/FrmResetSol?code=" + code)
            var callbackUrl = Utility.AbsoluteUrlRef("/portalproveedores/proveedor/Account/FrmResetSol?code=" + code);

            PI_NombrePlantilla = "RstUserWeb.html"; //RFD0 - 2022 - 155

            string pUrlConfirmacion;
            pUrlConfirmacion = callbackUrl;

            #region RFD0-2022-155 CORREO
            Variables = new System.Collections.Generic.Dictionary<string, string>();
            Variables.Add("@@Ruc", model.Ruc);
            Variables.Add("@@Usuario", model.Email);
            Variables.Add("@@UrlRecuperacion", pUrlConfirmacion);
            #endregion

            Thread t = new Thread(() => EnviarEmail(model.Email, "Recuperación de Contraseña", "", PI_NombrePlantilla, Variables));
            t.Start();

            FormResponse.success = true;

            return Ok();
        }

        private formResponseSeguridad EnviarEmail(string pCorreoE, string asuntoEmail, string mensajeEmail, 
            String PI_NombrePlantilla, System.Collections.Generic.Dictionary<string, string> Variables)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";

            #region RFD0-2022-155 Variables CORREO
            String PI_Variables = string.Empty;
            #endregion RFD0-2022-155 Variables CORREO

            clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient objEnvMail = new clibCustom.WCFEnvioCorreo.ServEnvioClientSoapClient();
            clsLogger _objLogServicio = new clsLogger();

            try
            {
                #region RFD0-2022-155 CORREO
                PI_Variables = Newtonsoft.Json.JsonConvert.SerializeObject(Variables).ToString();

                _objLogServicio.Graba_Log_Info("INI >> Envio Mail.");
                _objLogServicio.Graba_Log_Info("Envia Mail -> NombrePlantilla: " + PI_NombrePlantilla + " Variables: " + PI_Variables);
                byte[] data = Encoding.ASCII.GetBytes("TEST");
                string retornon = objEnvMail.EnviaCorreoDF("", pCorreoE, "", "", asuntoEmail, mensajeEmail, true, true, false, data, 
                    null, PI_NombrePlantilla, PI_Variables);

                _objLogServicio.Graba_Log_Info("Envia Mail -> Respuesta: " + retornon);
                #endregion

                //string retornon = objEnvMail.EnviarCorreo("", pCorreoE, "", "", asuntoEmail, mensajeEmail, true, true, false, null);

                if (retornon != "0")
                {
                    FormResponse.success = false;
                    FormResponse.codError = "-100";
                    FormResponse.msgError = "Los datos fueron actualizados pero se falló al enviar el correo con el ERROR: " + retornon;
                }
            }
            catch (Exception ex)
            {
                _objLogServicio.Graba_Log_Info("Envia Mail -> error: " + ex.Message);

                FormResponse.success = false;
                FormResponse.codError = "-100";
                FormResponse.msgError = ex.Message;
            }
            
           
            return FormResponse;
        }

        private formResponseSeguridad RegistraUser(ApplicationUser user)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            try
            {                 

                System.Data.DataSet ds = new System.Data.DataSet();
                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("tipo", "RE");
                xmlParam.DocumentElement.SetAttribute("Email", user.Email);
                xmlParam.DocumentElement.SetAttribute("Id", user.Id);
                xmlParam.DocumentElement.SetAttribute("Ruc", user.Ruc);
                xmlParam.DocumentElement.SetAttribute("PasswordHash", user.Password);
                xmlParam.DocumentElement.SetAttribute("SecurityStamp", user.SecurityStamp);
                xmlParam.DocumentElement.SetAttribute("PhoneNumber", user.PhoneNumber);
                xmlParam.DocumentElement.SetAttribute("LockoutEnabled", user.LockoutEnabled == true ? "1" : "0");
                xmlParam.DocumentElement.SetAttribute("AccessFailedCount", user.AccessFailedCount.ToString());
                xmlParam.DocumentElement.SetAttribute("UserName", user.UserName);
                clibCustom.ProcesoWs.ServBaseProceso PBase = new clibCustom.ProcesoWs.ServBaseProceso();
                PBase.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = PBase.MantenimientoUser(InitialiseService.Semilla, InitialiseService.PI_Session, xmlParam.OuterXml);
                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse = new formResponseSeguridad();
                    FormResponse.codError = "0";
                    FormResponse.msgError = "";
                    FormResponse.success = true;
                }
            }
            catch(Exception ex) {
                FormResponse = new formResponseSeguridad();
                FormResponse.success = false;
                FormResponse.codError = "1";
                FormResponse.msgError = ex.Message;
            }

            return FormResponse;
        }

        private formResponseSeguridad confirmedMail(string email)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            try
            {

                System.Data.DataSet ds = new System.Data.DataSet();

                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("tipo", "AC");
                xmlParam.DocumentElement.SetAttribute("Email", email);

                clibCustom.ProcesoWs.ServBaseProceso PBase = new clibCustom.ProcesoWs.ServBaseProceso();
                PBase.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = PBase.MantenimientoUser(InitialiseService.Semilla, InitialiseService.PI_Session, xmlParam.OuterXml);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse = new formResponseSeguridad();
                    FormResponse.codError = "0";
                    FormResponse.msgError = "";
                    FormResponse.success = true;
                }
            }
            catch (Exception ex)
            {
                FormResponse = new formResponseSeguridad();
                FormResponse.success = false;
                FormResponse.codError = "1";
                FormResponse.msgError = ex.Message;
            }

            return FormResponse;
        }

        private formResponseSeguridad ChangePass(string Id, string pass)
        {
            formResponseSeguridad FormResponse = new formResponseSeguridad();
            FormResponse.codError = "0";
            FormResponse.msgError = "";
            FormResponse.success = true;
            try
            {

                System.Data.DataSet ds = new System.Data.DataSet();

                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("tipo", "PA");
                xmlParam.DocumentElement.SetAttribute("Id", Id);
                xmlParam.DocumentElement.SetAttribute("PasswordHash", pass);

                clibCustom.ProcesoWs.ServBaseProceso PBase = new clibCustom.ProcesoWs.ServBaseProceso();
                PBase.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = PBase.MantenimientoUser(InitialiseService.Semilla, InitialiseService.PI_Session, xmlParam.OuterXml);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    FormResponse = new formResponseSeguridad();
                    FormResponse.codError = "0";
                    FormResponse.msgError = "";
                    FormResponse.success = true;
                }
            }
            catch (Exception ex)
            {
                FormResponse = new formResponseSeguridad();
                FormResponse.success = false;
                FormResponse.codError = "1";
                FormResponse.msgError = ex.Message;
            }

            return FormResponse;
        }

        private ApplicationUser consultaUsuario(string email)
        {
            ApplicationUser FormResponse = null;            
            try
            {

                System.Data.DataSet ds = new System.Data.DataSet();

                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("tipo", "CO");
                xmlParam.DocumentElement.SetAttribute("Email", email);

                clibCustom.ProcesoWs.ServBaseProceso PBase = new clibCustom.ProcesoWs.ServBaseProceso();
                PBase.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = PBase.MantenimientoUser(InitialiseService.Semilla, InitialiseService.PI_Session, xmlParam.OuterXml);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        FormResponse = new ApplicationUser();
                        FormResponse.Email = item["Email"].ToString();
                        FormResponse.UserName= item["UserName"].ToString();
                        FormResponse.EmailConfirmed = item["EmailConfirmed"].ToString() == "1" ? true : false;
                        FormResponse.Password = item["PasswordHash"].ToString();
                        FormResponse.Id = item["Id"].ToString();
                        FormResponse.Ruc= item["Ruc"].ToString();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                

            }

            return FormResponse;
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool resultado = false;
            var provider = new DpapiDataProtectionProvider("ngAuthApp3");
            //_userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, string>(provider.Create("ResetPassword"))
            //    as IUserTokenProvider<ApplicationUser, string>;

            clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();
            var DeCripCod = objEnc.Desencripta(model.Code, InitialiseService.Semilla);
            var CripCod = objEnc.Encripta(model.Email.ToLower() + "|2015sipecomfp", InitialiseService.Semilla);

            if (CripCod == model.Code)
                if (!String.IsNullOrEmpty(DeCripCod))
                {
                    var cct = DeCripCod.Split('|').Length;
                    var recemail = DeCripCod.Split('|')[0];

                    if (recemail != model.Email)
                    {
                        return BadRequest("No se ha logrado establecer su nueva contraseña.");
                    }
                }
                else
                    return BadRequest("No se ha logrado establecer su nueva contraseña.");

            ApplicationUser user = consultaUsuario(model.Email);

            if (user != null)
            {
                try
                {
                    user.Password = model.Password;
                   
                    if (CripCod == model.Code)
                    {
                        //_userManager.RemovePassword(user.Id);
                        //_userManager.AddPassword(user.Id, user.Password);
                        user.Password = objEnc.Encripta(user.Password, InitialiseService.Semilla);
                        ChangePass(user.Id, user.Password);
                        resultado = true;
                    }
                }
                catch (Exception)
                {
                    return BadRequest("No se ha logrado establecer su nueva contraseña.");
                }

                if (!resultado) return BadRequest("No se ha logrado establecer su nueva contraseña.");
            }
            else
                return BadRequest("No se ha logrado establecer su nueva contraseña.");

            return Ok(new { message = "Su nueva contraseña se ha establecido correctamente." });
        }
        #endregion

        ////////// GET api/Account/ExternalLogin
        ////////[OverrideAuthentication]
        ////////[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        ////////[AllowAnonymous]
        ////////[Route("ExternalLogin", Name = "ExternalLogin")]
        ////////public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        ////////{
        ////////    string redirectUri = string.Empty;

        ////////    if (error != null)
        ////////    {
        ////////        return BadRequest(Uri.EscapeDataString(error));
        ////////    }

        ////////    if (!User.Identity.IsAuthenticated)
        ////////    {
        ////////        return new ChallengeResult(provider, this);
        ////////    }

        ////////    var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

        ////////    if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
        ////////    {
        ////////        return BadRequest(redirectUriValidationResult);
        ////////    }

        ////////    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        ////////    if (externalLogin == null)
        ////////    {
        ////////        return InternalServerError();
        ////////    }

        ////////    if (externalLogin.LoginProvider != provider)
        ////////    {
        ////////        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        ////////        return new ChallengeResult(provider, this);
        ////////    }

        ////////    IdentityUser user = await _repo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

        ////////    bool hasRegistered = user != null;

        ////////    redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
        ////////                                    redirectUri,
        ////////                                    externalLogin.ExternalAccessToken,
        ////////                                    externalLogin.LoginProvider,
        ////////                                    hasRegistered.ToString(),
        ////////                                    externalLogin.UserName);

        ////////    return Redirect(redirectUri);

        ////////}

        ////////// POST api/Account/RegisterExternal
        ////////[AllowAnonymous]
        ////////[Route("RegisterExternal")]
        ////////public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        ////////{
        ////////    if (!ModelState.IsValid)
        ////////    {
        ////////        return BadRequest(ModelState);
        ////////    }

        ////////    var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
        ////////    if (verifiedAccessToken == null)
        ////////    {
        ////////        return BadRequest("Invalid Provider or External Access Token");
        ////////    }

        ////////    IdentityUser user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

        ////////    bool hasRegistered = user != null;

        ////////    if (hasRegistered)
        ////////    {
        ////////        return BadRequest("External user is already registered");
        ////////    }

        ////////    user = new IdentityUser() { UserName = model.UserName };

        ////////    ApplicationUser user1 = new ApplicationUser() { UserName = user.UserName };

        ////////    IdentityResult result = await _repo.CreateAsync(user1);
        ////////    if (!result.Succeeded)
        ////////    {
        ////////        return GetErrorResult(result);
        ////////    }

        ////////    var info = new ExternalLoginInfo()
        ////////    {
        ////////        DefaultUserName = model.UserName,
        ////////        Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
        ////////    };

        ////////    result = await _repo.AddLoginAsync(user.Id, info.Login);
        ////////    if (!result.Succeeded)
        ////////    {
        ////////        return GetErrorResult(result);
        ////////    }

        ////////    //generate access token response
        ////////    var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

        ////////    return Ok(accessTokenResponse);
        ////////}

        //////[AllowAnonymous]
        //////[HttpGet]
        //////[Route("ObtainLocalAccessToken")]
        //////public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        //////{
        //////    if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
        //////    {
        //////        return BadRequest("Provider or external access token is not sent");
        //////    }

        //////    var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
        //////    if (verifiedAccessToken == null)
        //////    {
        //////        return BadRequest("Invalid Provider or External Access Token");
        //////    }

        //////    IdentityUser user = await _repo.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

        //////    bool hasRegistered = user != null;

        //////    if (!hasRegistered)
        //////    {
        //////        return BadRequest("External user is not registered");
        //////    }

        //////    //generate access token response
        //////    var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

        //////    return Ok(accessTokenResponse);
        //////}

        ////protected override void Dispose(bool disposing)
        ////{
        ////    if (disposing)
        ////    {
        ////        _repo.Dispose();
        ////    }

        ////    base.Dispose(disposing);
        ////}

        #region Helpers

        private IHttpActionResult GetErrorResult(formResponseSeguridad result)
        {
            Logger.Logger log1 = new Logger.Logger();
            log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
            string mensajeerror = "en get error";

            log1.WriteMensaje(mensajeerror);

            if (result == null)
            {
                mensajeerror = "antes de return internal server error";
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                return InternalServerError();
            }

            if (!result.success)
            {
                if (result.codError != "0")
                {
                        mensajeerror = result.msgError;
                        log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                        log1.WriteMensaje(mensajeerror);

                        ModelState.AddModelError("", result.msgError);
                }

                if (ModelState.IsValid)
                {
                    mensajeerror = "is valid";
                    log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                    log1.WriteMensaje(mensajeerror);

                    return BadRequest();
                }

                mensajeerror = "return bad request";
                log1.FilePath = "c:\\AplicacionCER\\Log\\ErrorRegistro";
                log1.WriteMensaje(mensajeerror);

                return BadRequest(ModelState);
            }

            return null;
        }

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }

            //var client = _repo.FindClient(clientId);

            //if (client == null)
            //{
            //    return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            //}

            //if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            //{
            //    return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
            //}

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "xxxxxx";
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
            }

            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {
            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
        );

            return tokenResponse;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }

        #endregion
    }
}
