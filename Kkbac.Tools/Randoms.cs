using System;

namespace Kkbac.Tools
{
    public class Randoms
    {
        /// <summary>
        /// 生成随机大写字母
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string GetChar(int count)
        {
            if (count < 1)
            {
                return null;
            }
            string sc = @"3456789abcdefghgkmnpqrstuvwxyABCDEFGHGKMNPQRSTUVWXY";
            var s = "";
            Random random;
            var step = DateTime.Now.Millisecond % 10;
            for (int i = 0; i < count; i++)
            {
                random = new Random(Guid.NewGuid().GetHashCode() + step);
                // s += GetChar(random);
                s += GetDiy(random, sc);
            }
            return s;
        }

        /// <summary>
        /// 返回大小写字母和数字混合
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public string GetDiy(Random rnd, string s)
        {
            // 返回大小写字母和数字混合
            return s.Substring(0 + rnd.Next(s.Length - 1), 1);
        }

        /// <summary>
        /// 随机大写字符
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string GetUpperCases(int count)
        {
            if (count < 1)
            {
                return null;
            }
            var s = "";
            Random random;
            var t = DateTime.Now.Year
                + DateTime.Now.Month
                + DateTime.Now.Day
                + DateTime.Now.Hour
                + DateTime.Now.Minute
                + DateTime.Now.Second
                + DateTime.Now.Millisecond;
            for (int i = 0; i < count; i++)
            {
                random = new Random(t + i);
                s += GetUpperCase(random);
            }
            return s;
        }

        /// <summary>
        /// 生成随机大写字母
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public string GetUpperCase(Random rnd)
        {
            // A-Z  ASCII值  65-90 
            int i = rnd.Next(65, 91);
            char c = (char)i;
            return c.ToString();
        }

        /// <summary>
        /// 生成随机小写字母
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public string GetLowerCase(Random rnd)
        {
            // a-z  ASCII值  97-122 
            int i = rnd.Next(97, 123);
            char c = (char)i;
            return c.ToString();
        }

        /// <summary>
        /// 生成随机大小写字母混合
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public string GetLetter(Random rnd)
        {
            // A-Z  ASCII值  65-90
            // a-z  ASCII值  97-122
            int i = rnd.Next(65, 123);
            char c = (char)i;
            if (char.IsLetter(c))
            {
                return c.ToString();
            }
            else
            {
                // 递归调用，直到随机到字母
                return GetLetter(rnd);
            }
        }

        /// <summary>
        /// 生成随机大小写字母和数字混合
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public string GetChar(Random rnd)
        {
            // 0-9
            // A-Z  ASCII值  65-90
            // a-z  ASCII值  97-122
            int i = rnd.Next(0, 123);
            if (i < 10)
            {
                // 返回数字
                return i.ToString();
            }

            char c = (char)i;

            // 返回小写字母加数字
            // return char.IsLower(c) ? c.ToString() : GetChar(rnd);

            // 返回大写字母加数字
            // return char.IsUpper(c) ? c.ToString() : GetChar(rnd);

            // 返回大小写字母加数字
            return char.IsLetter(c) ? c.ToString() : GetChar(rnd);
        }

        /// <summary>
        /// 返回大小写字母和数字混合
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public string GetMix(Random rnd)
        {
            string str = @"0123456789abcdefghigklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ";
            // 返回数字
            // return rnd.Next(10).ToString();

            // 返回小写字母
            // return str.Substring(10+rnd.Next(26),1);

            // 返回大写字母
            // return str.Substring(36+rnd.Next(26),1);

            // 返回大小写字母混合
            // return str.Substring(10+rnd.Next(52),1);

            // 返回小写字母和数字混合
            // return str.Substring(0 + rnd.Next(36), 1);

            // 返回大写字母和数字混合
            // return str.Substring(0 + rnd.Next(36), 1).ToUpper();

            // 返回大小写字母和数字混合
            return str.Substring(0 + rnd.Next(61), 1);
        }
    }
}
