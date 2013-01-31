using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Kkbac.Tools.Web.MVC.Exceptions
{
    using Extensions.Formats;

    public class Exceptions
    {
        public ExceptionModel Get(ExceptionContext ex)
        {
            var error = new ExceptionModel();

            var baseException = ex.Exception.GetBaseException();
            var context = ex.HttpContext;

            error.Url = context.Request.Url.ToString();
            error.TypeName = baseException.GetType().FullName;
            error.Message = baseException.Message;
            error.Source = baseException.Source;
            error.Detail = ex.Exception.ToString();

            var kv = new List<ExceptionModelKeyValue>();

            if (context.Request.QueryString.Count > 0)
            {
                foreach (var key in context.Request.QueryString.AllKeys)
                {
                    kv.Add(new ExceptionModelKeyValue()
                    {
                        k = key,
                        v = context.Request.QueryString[key]
                    });
                }
                error.QueryString = kv.Obj2Json();
            }
            else
            {
                error.QueryString = "[]";
            }

            if (context.Request.HttpMethod.ToUpper() == "POST"
                &&
                context.Request.Form.Count > 0
            )
            {
                kv = new List<ExceptionModelKeyValue>();
                foreach (var key in context.Request.Form.AllKeys)
                {
                    kv.Add(new ExceptionModelKeyValue()
                    {
                        k = key,
                        v = context.Request.Form[key]
                    });
                }
                error.Form = kv.Obj2Json();
            }
            else
            {
                error.Form = "[]";
            }

            if (context.Request.ServerVariables.Count > 0)
            {
                kv = new List<ExceptionModelKeyValue>();
                foreach (var key in context.Request.ServerVariables.AllKeys)
                {
                    kv.Add(new ExceptionModelKeyValue()
                    {
                        k = key,
                        v = context.Request.ServerVariables[key]
                    });
                }
                error.ServerVariables = kv.Obj2Json();
            }
            else
            {
                error.ServerVariables = "[]";
            }

            if (context.Request.Cookies.Count > 0)
            {
                kv = new List<ExceptionModelKeyValue>();

                for (int i = 0; i < context.Request.Cookies.Count; i++)
                {
                    var cookie = context.Request.Cookies[i];
                    kv.Add(new ExceptionModelKeyValue()
                    {
                        k = cookie.Name,
                        v = cookie.Value
                    });
                }
                error.Cookies = kv.Obj2Json();
            }
            else
            {
                error.Cookies = "[]";
            }
            error.CreateTime = DateTime.Now;

            return error;
        }

    }

}
