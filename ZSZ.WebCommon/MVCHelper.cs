using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ZSZ.WebCommon
{
    public static class MVCHelper
    {
        /// <summary>
        /// mvc中model属性验证
        /// </summary>
        public static string GetValidMsg(ModelStateDictionary modelSatae)
        {
            StringBuilder builer = new StringBuilder();
            foreach (var propName in modelSatae.Keys)
            {
                if (modelSatae[propName].Errors.Count <= 0)
                {
                    continue;
                }
                builer.Append("属性错误：").Append(propName).Append("：");
                foreach (ModelError modelError in modelSatae[propName].Errors)
                {
                    builer.Append(modelError.ErrorMessage);
                }
            }
            return builer.ToString();
        }
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="appKey">密钥</param>
        /// <param name="templateId">模板id</param>
        /// <param name="code">要返回的短信验证码</param>
        /// <param name="phoneNum">发送的手机号</param>
        /// <returns></returns>
        public static MessageResult SendMessage(string userName, string appKey, string templateId, string code, string phoneNum)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string url = "http://sms.rupeng.cn/SendSms.ashx?userName=" + Uri.EscapeDataString(userName) + "&appKey=" + Uri.EscapeDataString(appKey) + "&templateId=" + Uri.EscapeDataString(templateId) + "&code=" + Uri.EscapeDataString(code) + "&phoneNum=" + Uri.EscapeDataString(phoneNum);
            string resp = wc.DownloadString(url);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<MessageResult>(resp);
        }
        /// <summary>
        /// 转换成QueryString字符串
        /// </summary>
        public static string ToQueryString(this NameValueCollection queryString)
        {
            StringBuilder builder = new StringBuilder();
            for(int i=0;i<queryString.Keys.Count;i++)
            {
                string key = queryString.Keys[i];
                builder= builder.Append(key).Append("=").Append(Uri.EscapeDataString(queryString[key]));
                if(i!= queryString.Keys.Count-1)
                {
                    builder = builder.Append("&");
                }
            }
            return builder.ToString();
        }
        public static string UpdateQueryString(NameValueCollection queryString,string name,object value)
        {
            NameValueCollection newQueryString = new NameValueCollection(queryString);
            if(newQueryString.AllKeys.Contains(name))
            {
                newQueryString[name] = Convert.ToString(value);
            }
            else
            {
                newQueryString.Add(name, Convert.ToString(value));
            }
            return newQueryString.ToQueryString();
        }
        public static string RemoveQueryString(NameValueCollection queryString, string name)
        {
            NameValueCollection newQueryString = new NameValueCollection(queryString);
            newQueryString.Remove(name);
            return ToQueryString(newQueryString);
        }
        public static string RenderViewToString(ControllerContext context, string viewPath,object model = null)
        {
            ViewEngineResult viewEngineResult =ViewEngines.Engines.FindView(context, viewPath, null);
            if (viewEngineResult == null)
            {
                throw new FileNotFoundException("View" + viewPath + "cannot be found.");
            }                
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,context.Controller.ViewData,context.Controller.TempData,sw);
                view.Render(ctx, sw);
                return sw.ToString();
            }
        }
    }
}
