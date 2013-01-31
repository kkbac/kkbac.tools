using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Kkbac.Tools.Captchas
{
    public class Captcha
    {
        /// <summary>
        /// 取得验证码.
        /// </summary>
        /// <param name="captcheChar"></param>
        /// <returns></returns>
        public byte[] Get(string captcheChar)
        {
            // 随机数生成器，使用静态变量
            Random rnd = new Random();

            // 验证码图片大小
            int width = 68;
            int height = 30;

            // 位图
            Bitmap img = new Bitmap(width, height);

            // 画板
            Graphics g = Graphics.FromImage(img);

            // 单色画笔（画背景色，字符串）
            Brush b;

            // 初始化为背景色（红绿蓝都在200到250之间）
            b = new SolidBrush(Color.FromArgb(rnd.Next(200, 250), rnd.Next(200, 250), rnd.Next(200, 250)));

            // 画背景
            g.FillRectangle(b, 0, 0, width, height);

            // 边框
            // g.FillRectangle(b, 0, 0, width - 1, height - 1);           

            // 画笔（画直线或曲线）（红绿蓝都在160到200之间）
            Pen p = new Pen(Color.FromArgb(rnd.Next(160, 200), rnd.Next(160, 200), rnd.Next(160, 200)));

            // 画干扰线
            for (int i = 0; i < 100; i++)
            {
                int x1 = rnd.Next(width);
                int y1 = rnd.Next(height);

                // 线条长度在 12 以内
                int x2 = x1 + rnd.Next(12);
                int y2 = y1 + rnd.Next(12);

                // 绘制两个坐标之间的连线
                // 参数：起点(x1, y1)，终点(x2, y2);
                g.DrawLine(p, x1, y1, x2, y2);
            }

            // 验证码字体
            Font f = new Font("Courier New", 18, FontStyle.Bold);

            // 验证码位置（左上角坐标）
            PointF pf;

            var j = 0;
            // 验证码生成
            foreach (var s in captcheChar)
            {
                // 验证码颜色（红绿蓝都在20到130之间）
                b = new SolidBrush(Color.FromArgb(rnd.Next(20, 130), rnd.Next(20, 130), rnd.Next(20, 130)));

                // 可设置字符在图片中的位置
                pf = new PointF(15 * j, 3);
                g.DrawString(s.ToString(), f, b, pf);
                j++;
            }

            // 输出验证码
            // response.ContentType = "image/pjpeg";
            //img.Save(response.OutputStream, ImageFormat.Jpeg);

            byte[] byteArray = new byte[0];
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Jpeg);
                //stream.Flush();
                byteArray = stream.GetBuffer();
            }

            // 释放资源
            b.Dispose();
            g.Dispose();
            img.Dispose();

            return byteArray;
        }
    }

}
