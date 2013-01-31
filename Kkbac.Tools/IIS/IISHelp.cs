using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.ServiceProcess;

namespace Kkbac.Tools.IIS
{
    public class IISHelp
    {
        public IISHelp()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        #region IIS操作辅助类

        ServiceController sc = new ServiceController("iisadmin");
        /// <summary>
        /// 停止IIS服务
        /// </summary>
        /// <returns>返回TRUE和FALSE</returns>
        public bool StopIIS()
        {
            bool SpIIs = false;
            try
            {
                sc.Stop();
                SpIIs = true;
            }
            catch (Exception)
            {
                SpIIs = false;
            }
            return SpIIs;
        }
        /// <summary>
        /// 启动IIS服务
        /// </summary>
        /// <returns>返回TRUE和FALSE</returns>
        public bool StartIIS()
        {
            bool si = false;
            try
            {
                sc.Start();
                si = true;
            }
            catch (Exception)
            {
                si = false;
            }
            return si;
        }
        /// <summary>
        ///  重启IIS服务
        /// </summary>
        /// <returns>返回TRUE和FALSE</returns>
        public bool ResetIIS()
        {
            bool ri = false;
            try
            {
                Process.Start("iisreset");
                ri = true;
            }
            catch (Exception)
            {
                ri = false;
            }
            return ri;
        }
        #endregion
    }
}
