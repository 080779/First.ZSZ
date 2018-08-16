using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.FrontWeb.App_Start
{
    /// <summary>
    /// 自定义的ZSZExceptionFilter异常处理。出现异常记录到Log4Net日志中，在Global中全局处理
    /// </summary>
    public class ZSZExceptionFilter : IExceptionFilter
    {
        private static ILog log = LogManager.GetLogger(typeof(ZSZExceptionFilter));//Log4Net记录异常
        public void OnException(ExceptionContext filterContext)
        {
            log.ErrorFormat("出现未处理异常：{0}", filterContext.Exception);
        }
    }
}