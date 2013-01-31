using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace ConsoleApplicationTest
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Math.Pow(1.01, 365));
            Console.WriteLine(Math.Pow(0.99, 365));

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void NewMethod()
        {
            byte[] buffer = null;
            var testString = "Stream!Hello World!";
            using (var stream = new MemoryStream())
            {
                Console.WriteLine(testString);
                buffer = Encoding.Default.GetBytes(testString);
                stream.Write(buffer, 0, 3);
                var nowposition = (int)stream.Seek(3, SeekOrigin.Current);
                stream.Write(buffer, nowposition, buffer.Length - nowposition);
                var resultBuffer = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(resultBuffer, 0, (int)stream.Length);
                var resultString = Encoding.Default.GetString(resultBuffer);
                Console.WriteLine(resultString);
            }
        }

        private static void NewMethod5()
        {
            var context = new Kkbac.Tools.Datas.Mssql.DbContext();
        }

        private static void NewMethod4()
        {
            var times = 1000000;
            Program program = new Program();
            object[] parameters = new object[] { new object(), new object(), new object() };
            program.Call(null, null, null); // force JIT-compile
            var watch1 = new Stopwatch();
            watch1.Start();
            for (int i = 0; i < times; i++)
            {
                program.Call(parameters[0], parameters[1], parameters[2]);
            }
            watch1.Stop();
            Console.WriteLine(watch1.Elapsed + " (Directly invoke)");

            MethodInfo methodInfo = typeof(Program).GetMethod("Call");
            var watch2 = new Stopwatch();
            watch2.Start();
            for (int i = 0; i < times; i++)
            {
                methodInfo.Invoke(program, parameters);
            }
            Console.WriteLine(watch2.Elapsed + " (Reflection invoke)");
        }

        public void Call(object o1, object o2, object o3) { }

        private static void NewMethod3()
        {
            string s = null;
            string se = string.Empty;
            string ses = "";

            List<string> stringEmpty = new List<string>()
            {
                null,
                string.Empty,
                ""
            };
            stringEmpty.ForEach(x =>
            {
                Console.WriteLine(x);
                Console.WriteLine("x == null: " + (x == null));
                Console.WriteLine("x == string.Empty: " + (x == string.Empty));
                Console.WriteLine("x == \"\": " + (x == ""));
                Console.WriteLine();
            });
            Console.ReadKey(true);
        }
         
          
    }
}
