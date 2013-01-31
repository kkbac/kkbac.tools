using System.Globalization;

namespace Kkbac.Tools
{
    public class Cultures
    {
        public void SetZhNumber()
        {
            //修改CultureInfo的NumberFormatInfo
            var culture = (CultureInfo)CultureInfo.GetCultureInfo("zh-cn").Clone();
            culture.NumberFormat.NumberGroupSizes =
                culture.NumberFormat.CurrencyGroupSizes = new int[] { 4 };
            culture.NumberFormat.NumberGroupSeparator =
                culture.NumberFormat.CurrencyGroupSeparator = ",";

            //设置成当前线程的CultureInfo
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            //var formatString = "#,###0";
            //return formatString;
        }

    }

}
