using System;
using System.IO;
using System.Net;
using System.Text;

namespace Kkbac.Tools
{
    public class GetHtml
    {
        public bool IsSuccess { get; set; }

        public string Html { get; set; }

        public GetHtml()
        {
            IsSuccess = false;
            Html = string.Empty;
        }

        public bool Get(string url)
        {
            var encoding = Encoding.Default;
            IsSuccess = Get(url, encoding);
            return IsSuccess;
        }

        public bool Get(string url, Encoding encoding)
        {
            IsSuccess = false;
            Html = string.Empty;
            try
            {
                WebRequest request = WebRequest.Create(url);
                using (var response = request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        Html = reader.ReadToEnd();
                    } 
                } 

                IsSuccess = true;
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Html = ex.Message;
            }
            return IsSuccess;
        }

    }

}
