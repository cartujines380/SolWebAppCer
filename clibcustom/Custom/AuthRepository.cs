using AngularJSAuthentication.API.Entities;
using AngularJSAuthentication.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Xml;
using System.Text.RegularExpressions;
using clibCustom.ProcesoWs;
using System.Data;

namespace AngularJSAuthentication.API
{

    public class AuthRepository : IDisposable
    {
        private ApplicationDbContext _ctx;

         private UserManager<ApplicationUser> _userManager;

        public AuthRepository()
        {
            //_ctx = new ApplicationDbContext();
            //_userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(ApplicationUser userModel)
        {
    
            var user = new ApplicationUser() { UserName = userModel.UserName  };
            var result = await _userManager.CreateAsync(userModel, userModel.Password);
            

            if (result.Succeeded)
            {
                //var currentUser = _userManager.FindByName(user.UserName);

                //var roleresult = _userManager.AddToRole(currentUser.Id, "WebUsers");
 
            }
            else
            {
                //AddErrors(result);
            }

 
            return result;
        }
         

       

       public async Task<ApplicationUser> FindUser(  string userName, string password)
        {
            IdentityDbContext db = new IdentityDbContext();

            //var Opciones = _ctx.Opciones;

            //List<int> transacciones = new List<int>();
            //transacciones.Add(1000);
            //transacciones.Add(2000);
            //transacciones.Add(3000);
            //transacciones.Add(4000);
            
            //var opsInIdList = from item in Opciones
            //                     join Id in transacciones
            //                     on item.ItemId equals Id
            //                     select item.ItemId;

            //var myList = _ctx.Opciones.Where(item => transacciones.Contains(item.ItemId))
            //          .Select(a => a.ItemPadreId).ToList();

            
            ApplicationUser user;
            try
            {
                user = await _userManager.FindAsync(userName, password);
                

    
            }
            catch (Exception ee)
            {
                
                throw ee;
            }
            

            return user;
        }


        public ApplicationUser consultaUsuarioP(string email, string password)
        {
            ApplicationUser FormResponse = null;
            try
            {

                System.Data.DataSet ds = new System.Data.DataSet();

                XmlDocument xmlParam = new XmlDocument();
                xmlParam.LoadXml("<Root />");
                xmlParam.DocumentElement.SetAttribute("tipo", "CP");
                xmlParam.DocumentElement.SetAttribute("Email", email);
                xmlParam.DocumentElement.SetAttribute("PasswordHash", password);

                clibCustom.ProcesoWs.ServBaseProceso PBase = new clibCustom.ProcesoWs.ServBaseProceso();
                PBase.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
                ds = PBase.MantenimientoUser(InitialiseService.Semilla, InitialiseService.PI_Session, xmlParam.OuterXml);

                if (ds.Tables["TblEstado"].Rows[0]["CodError"].ToString().Equals("0"))
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        FormResponse = new ApplicationUser();
                        FormResponse.Email = item["Email"].ToString();
                        FormResponse.UserName = item["UserName"].ToString();
                        FormResponse.EmailConfirmed = item["EmailConfirmed"].ToString() == "True" ? true : false;
                        FormResponse.Password = item["PasswordHash"].ToString();
                        FormResponse.Id = item["Id"].ToString();
                        FormResponse.Ruc = item["Ruc"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {


            }

            return FormResponse;
        }


        private string LoginSesionUsuario(string sourceLogin, string PI_ParamXML)
        {
            string gl_SemillaEncryp = string.Empty;
       
            string xmlResp;

            clibCustom.ProcesoWs.ServBaseProceso Proceso = new clibCustom.ProcesoWs.ServBaseProceso();
            Proceso.Url = System.Configuration.ConfigurationManager.AppSettings["UrlBase"].ToString();
            xmlResp = (string)Proceso.LoginSesionUsuario(InitialiseService.Semilla, InitialiseService.PI_Session, sourceLogin, PI_ParamXML);
            return  xmlResp ;
        }

      
        public string LoginFS(string sourceLogin,string ruc,string IdUsername, string Username, string Password, string sociedad, string apl)
        {
            string resultado = string.Empty;

            XmlDocument xmlParam = new XmlDocument();
            xmlParam.LoadXml("<Root />");
            xmlParam.DocumentElement.SetAttribute("PS_IdEmpresa", "1");
            xmlParam.DocumentElement.SetAttribute("PS_IdUsuario", IdUsername);
            
            xmlParam.DocumentElement.SetAttribute("PS_Usuario", Username);
            xmlParam.DocumentElement.SetAttribute("PS_Ruc", ruc);
            xmlParam.DocumentElement.SetAttribute("PS_sociedad", sociedad);
            //xmlParam.DocumentElement.SetAttribute("MAC", "N");
            
            xmlParam.DocumentElement.SetAttribute("PS_BandAplicativo", apl);
            xmlParam.DocumentElement.SetAttribute("PS_Clave", Password);
            //para el caso que pase por proxi
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                xmlParam.DocumentElement.SetAttribute("PS_Maquina", System.Web.HttpContext.Current.Request.UserHostAddress);
            }
            else
            {
                xmlParam.DocumentElement.SetAttribute("PS_Maquina", System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString());
            }

            xmlParam.DocumentElement.SetAttribute("PS_Login", "N");
            xmlParam.DocumentElement.SetAttribute("Perfil", "M");


            clibEncripta.clsEncripta objEnc = new clibEncripta.clsEncripta();


            string Result = LoginSesionUsuario( sourceLogin, xmlParam.OuterXml  );

            XmlDocument xmlResp = new XmlDocument();
            xmlResp.LoadXml(  Result );


            string mensaje = string.Empty;

            if (xmlResp.DocumentElement.GetAttribute("CodError") == "0")
            {
                  resultado = Result; 
            }
            else
            {
                resultado = Result;
                mensaje = "Ha ocurrido un error en la autenticación: " + xmlResp.DocumentElement.GetAttribute("MsgError");
               }

            return resultado;

        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            try
            {
                var existingToken1 = _ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).ToList();
                foreach (var existingToken in existingToken1)
                {
                    if (existingToken != null)
                    {
                        var result = await RemoveRefreshToken(existingToken);
                    }
                 }
                  _ctx.RefreshTokens.Add(token);
            }
            catch (Exception ex)
            {
                
                throw;
            }
           
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
           var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

           if (refreshToken != null) {
               _ctx.RefreshTokens.Remove(refreshToken);
               return await _ctx.SaveChangesAsync() > 0;
           }

           return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
             return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
             return  _ctx.RefreshTokens.ToList();
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }

        public void Dispose()
        {
            if (_ctx != null)
            {
                _ctx.Dispose();
            }

            if (_userManager != null)
            {
                _userManager.Dispose();
            }
            

        }
    }
}