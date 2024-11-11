using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace AngularJSAuthentication.API.Custom
{
    public class MembershipHttpAuthorizeAttribute : BasicHttpAuthorizeAttribute
    {

        public MembershipHttpAuthorizeAttribute(string roleSelector)
        {

            Roles = GetRoles(roleSelector);
        }


        private string GetRoles(string roleSelector)
        {
            // Do something to get the dynamic list of roles instead of returning a hardcoded string

            string retorno = string.Empty;

            switch (roleSelector)
            {

                case "Rol000100":

                    retorno = "Admin";

                    break;


                default:
                    break;
            }


            return retorno;
        }

        /// <;summary>;
        /// Implement to include authentication logic and create IPrincipal
        /// <;/summary>;
        protected override bool TryCreatePrincipal(string user, string password,
            out IPrincipal principal)
        {
            principal = null;
            if (!Membership.Provider.ValidateUser(user, password))
                return false;
            string[] roles = System.Web.Security.Roles.Provider.GetRolesForUser(user);
            principal = new GenericPrincipal(new GenericIdentity(user), roles);
            return true;
        }
    }
}