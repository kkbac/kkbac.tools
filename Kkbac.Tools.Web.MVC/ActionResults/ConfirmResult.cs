using System.Web.Mvc;

namespace Kkbac.Tools.Web.MVC.ActionResults
{
    public class ConfirmResult : ActionResult
    {
        public string Message { get; set; }
        public string ReturnUrl { get; set; }
        public bool HistoryBack { get; set; }

        public ConfirmResult(
            string message,
            string returnUrl = "",
            bool historyBack = false
        )
        {
            Message = message;
            ReturnUrl = returnUrl;
            HistoryBack = historyBack;

        }

        public override void ExecuteResult(ControllerContext context)
        {
            //throw new NotImplementedException();

            Message = Message.Replace("\"", "'");
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                ReturnUrl = context.HttpContext.Request.UrlReferrer.PathAndQuery;
            }
            var script = string.Format("<script>alert(\"{0}\");window.location.href = \"{1}\";</script>",
                    Message, ReturnUrl);
            if (HistoryBack == true)
            {
                script = string.Format("<script>alert(\"{0}\");window.history.go(-1);</script>",
                      Message);
            }
            context.HttpContext.Response.Write(script);
        }

    }

}
