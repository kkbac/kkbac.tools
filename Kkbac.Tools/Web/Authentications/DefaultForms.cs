using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Kkbac.Tools.Web.Authentications
{
    public class DefaultForms
    {
        public void Signout()
        {
            FormsAuthentication.SignOut();
        }

        public void SetAuth(
            HttpContext httpContext,
            string user
        )
        {
            FormsAuthentication.SetAuthCookie(user, false);
        }

        public void SetAuth(
            HttpContext httpContext,
            string user,
            string userdata
        )
        {

            var Ticket = new FormsAuthenticationTicket(
                1
                , user
                , DateTime.Now
                , DateTime.Now.AddYears(1),
                false,
                userdata,
                FormsAuthentication.FormsCookiePath
            );
            var HashTicket = FormsAuthentication.Encrypt(Ticket);
            var cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                HashTicket
            );

            httpContext.Response.Cookies.Add(cookie);
        }

        public string GetAuthUserdata(HttpContext httpContext)
        {
            var isAuth = IsAuthenticated(httpContext);

            if (isAuth == false)
            {
                return null;
            }

            var identity = (FormsIdentity)httpContext.User.Identity;
            var userdata = identity.Ticket.UserData;

            return userdata;
        }

        public string GetAuthUser(HttpContext httpContext)
        {
            var isAuth = IsAuthenticated(httpContext);

            if (isAuth == false)
            {
                return null;
            }

            var identity = (FormsIdentity)httpContext.User.Identity;
            var user = identity.Name;

            return user;
        }

        public bool IsAuthenticated(HttpContext httpContext)
        {
            return httpContext.Request.IsAuthenticated == true;
        }
    }

}
