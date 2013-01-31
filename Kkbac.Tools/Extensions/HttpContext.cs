using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kkbac.Tools.Extensions.HttpContext
{
    using Formats;
    public static class Requests
    {
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsPost(this HttpRequestBase request)
        {
            var httpmethod = request.HttpMethod.ToLower();
            var b = (httpmethod == "post");
            return b;
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsGet(this HttpRequestBase request)
        {
            var httpmethod = request.HttpMethod.ToLower();
            var b = (httpmethod == "get");
            return b;
        }

        /// <summary>
        /// 从 Cookies、Form、QueryString 或 ServerVariables 集合中获取指定的对象。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(this HttpRequestBase request,
            string key)
        {
            var s = request[key].ToStrings();
            return s;
        }

        /// <summary>
        /// 从 Cookies、Form、QueryString 或 ServerVariables 集合中获取指定的对象。
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt(
            this HttpRequestBase request,
            string key,
            int defaultValue = 0
        )
        {
            return request.GetString(key).ToInt(defaultValue);
        }

        public static DateTime GetDatetime(this HttpRequestBase request,
            string key)
        {
            return request.GetString(key).ToDateTime();
        }

        public static string GetCookie(
            this HttpRequestBase request,
            string key
        )
        {
            string value = "";
            HttpCookie httpcookie = request.Cookies[key];
            if (httpcookie != null)
            {
                value = httpcookie.Value;
            }
            return value;
        }

        /// <summary>
        /// 取得上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer(this HttpRequestBase request)
        {
            string r = "";
            var url = request.UrlReferrer;
            if (url != null)
            {
                r = url.ToString();
            }

            return r;
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP(this HttpRequestBase request)
        {
            string result = String.Empty;

            result = request.ServerVariables.Get("HTTP_X_FORWARDED_FOR");
            if (string.IsNullOrEmpty(result) == false && result.IndexOf(",") > -1)
            {
                result = result.Split(',')[0];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = request.ServerVariables.Get("REMOTE_ADDR");
            }

            if (string.IsNullOrEmpty(result))
            {
                result = request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "";
            }

            var b = System.Text.RegularExpressions.Regex.IsMatch(
                result,
                @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");


            if (!b)
            {
                result = "";
            }

            return result;

        }

        /// <summary>
        /// request.GetString("returnurl");
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetReturnUrl(this HttpRequestBase request)
        {
            return request.GetString("returnurl");
        }

        public static IEnumerable<string> GetFileLineString(
          this HttpPostedFileBase postedfile
        )
        {
            var filetext = postedfile.GetFileString(System.Text.Encoding.Default);
            var filetextlist = System.Text.RegularExpressions.Regex.Split(filetext, @"\r\n").ToList();
            return filetextlist;
        }

        public static string GetFileString(
            this HttpPostedFileBase postedfile
        )
        {
            var filetext = postedfile.GetFileString(System.Text.Encoding.Default);
            return filetext;
        }

        public static string GetFileString(
            this HttpPostedFileBase postedfile,
            System.Text.Encoding encoding
        )
        {
            var filetext = "";
            using (System.IO.StreamReader streamReader
                = new System.IO.StreamReader(postedfile.InputStream, encoding))
            {
                filetext = streamReader.ReadToEnd();
            }

            return filetext;
        }
    }

    public static class Responses
    {
        public static void OutputCaptcha(this HttpResponseBase response,
            byte[] byteArray)
        {
            using (var stream = new System.IO.MemoryStream(byteArray))
            {
                using (var bitmap = new System.Drawing.Bitmap(
                    (System.Drawing.Image)new System.Drawing.Bitmap(stream)))
                {
                    response.ContentType = "image/pjpeg";
                    bitmap.Save(response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                }
            }

        }

        /// <summary>
        /// 输出. (Clear()- Write(s) - text/plain - [utf-8] - End())
        /// </summary>
        /// <param name="s"></param>
        public static void OutputString(this HttpResponseBase response,
            string s,
            string contentType = "text/plain",
            string charset = "UTF-8")
        {
            response.Clear();
            response.Write(s);
            response.ContentType = contentType;
            response.Charset = charset;
            response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires">分钟</param>
        public static void SetCookie(this HttpResponseBase response,
            string key,
            string value,
            int expires = 365*24*60)
        {
            HttpCookie httpcookie = new HttpCookie(key);

            httpcookie.Value = value;
            if (expires > 0)
                httpcookie.Expires = DateTime.Now.AddMinutes(expires);
            response.Cookies.Add(httpcookie);
        }

        /// <summary>
        /// 设置Forms登录,
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userName"></param>
        /// <param name="userData"></param>
        /// <param name="expireMinutes">如果浏览器关闭过期,1分钟</param>
        public static void SetFormsLogin(this HttpResponseBase response,
            string userName,
            string userData = "",
            long expireMinutes = 365*24*60)
        {
            if (string.IsNullOrEmpty(userData))
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(userName, true);
                if (expireMinutes > 0)
                {
                    response.Cookies[
                        System.Web.Security.FormsAuthentication.FormsCookieName
                    ].Expires = DateTime.Now.AddMinutes(expireMinutes);
                }
            }
            else
            {
                // Create a new ticket used for authentication
                var ticket = new System.Web.Security.FormsAuthenticationTicket(
                   1, // Ticket version
                   userName, // Username associated with ticket
                   DateTime.Now, // Date/time issued
                   DateTime.Now.AddMinutes(expireMinutes), // Date/time to expire
                   true, // "true" for a persistent user cookie
                   userData, // User-data, in this case the roles
                   System.Web.Security.FormsAuthentication.FormsCookiePath);// Path cookie valid for

                // Encrypt the cookie using the machine key for secure transport
                string hash = System.Web.Security.FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                   System.Web.Security.FormsAuthentication.FormsCookieName, // Name of auth cookie
                   hash); // Hashed ticket

                // Set the cookie's expiration time to the tickets expiration time
                if (ticket.IsPersistent)
                {
                    cookie.Expires = ticket.Expiration;
                }

                // Add the cookie to the list for outgoing response
                response.Cookies.Add(cookie);
            }
        }
    }

    public static class Caches
    {
        /// <summary>
        /// 如果不包含此项: default(T);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this System.Web.Caching.Cache cache,
            string key)
        {
            T t = default(T);
            object o = cache.Get(key);
            if (o != null && o is T)
            {
                t = (T)o;
            }
            return t;
        }

        /// <summary>
        /// 如果不包含此项: default(T);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Remove<T>(this System.Web.Caching.Cache cache,
            string key)
        {
            T t = default(T);
            object o = cache.Remove(key);
            if (o != null && o is T)
            {
                t = (T)o;
            }
            return t;
        }

        public static void Set(this System.Web.Caching.Cache cache,
            string key,
            object value,
            long second = 1800)
        {
            if (value == null || string.IsNullOrEmpty(key))
            {
                return;
            }
            if (second > 0)
            {
                cache.Insert(
                    key,
                    value,
                    null,
                    DateTime.Now.AddSeconds(second),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                cache.Insert(key, value);
            }
        }
    }

    public static class Sessions
    {
        public static T Get<T>(this System.Web.HttpSessionStateBase session,
            string key)
        {
            T t = default(T);
            object o = session[key];
            if (o != null && o is T)
            {
                t = (T)o;
            }
            return t;
        }
    }

    public static class RouteDatas
    {
        /// <summary>
        /// controller名
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static string GetController(
            this System.Web.Routing.RouteData routeData,
            string controllerName = "controller")
        {
            var n = routeData.Values[controllerName].ToStrings();
            return n;
        }
        /// <summary>
        /// action名
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static string GetAction(
            this System.Web.Routing.RouteData routeData,
            string actionName = "action")
        {
            var n = routeData.Values[actionName].ToStrings();
            return n;
        }

    }
}
