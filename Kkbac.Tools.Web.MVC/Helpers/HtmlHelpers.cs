using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kkbac.Tools.Web.MVC.Helpers.HtmlHelpers
{
    using Extensions.Formats;

    public static class HtmlHelpers
    {
        /// <summary>
        /// Gets the Gravatar image URL.
        /// </summary>
        /// <param name="emailId">The email id.</param>
        /// <param name="imgSize">Size of the img.</param>
        /// <seealso cref="https://en.gravatar.com/"/>
        /// <returns>Image MvcHtmlString</returns>
        public static MvcHtmlString RenderGravatarImage(
            this HtmlHelper helper,
            string emailId,
            int imgSize
        )
        {
            if (string.IsNullOrEmpty(emailId))
                throw new ArgumentNullException("Email Id should not be null!");

            // Convert emailID to lower-case
            emailId = emailId.ToLower();

            string hash = emailId.ToMd5();

            // build Gravatar Image URL
            string imageUrl = string.Format(@"<img src=""http://www.gravatar.com/avatar/{0}?s={1}&d=mm&r=g"" />",
                hash,
                imgSize
            );

            return new MvcHtmlString(imageUrl);
        }
    }
}
