using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;
using ZSZ.WebCommon;

namespace ZSZ.FrontWeb.App_Start
{
    /// <summary>
    /// 自定义权项控制ZSZAuthorizationFilter
    /// </summary>
    public class ZSZAuthorizationFilter : IAuthorizationFilter
    {
        public IAdminUserService userService = DependencyResolver.Current.GetService<IAdminUserService>();//单独进行Atuoface属性注入
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            CheckPermissionAttribute[] attributes = (CheckPermissionAttribute[])filterContext.ActionDescriptor.GetCustomAttributes(typeof(CheckPermissionAttribute), false);
            long? adminUserId = (long?)filterContext.HttpContext.Session["AdminUserId"];
            if (attributes.Length <= 0)
            {
                return;
            }
            if (adminUserId == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())//判断是否是ajax请求
                {
                    filterContext.Result = new JsonNetResult { Data = new AjaxResult { Status = "redirect", Data = "/Main/Login" } };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Main/Login");
                }
                return;
            }
            foreach (var attr in attributes)
            {
                if (!userService.HasPermission(adminUserId.Value, attr.Permission))
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonNetResult { Data = new AjaxResult { Status = "error", ErrorMsg = "没有" + attr.Permission + "这个权限" } };
                    }
                    else
                    {
                        filterContext.Result = new ContentResult() { Content = "没有" + attr.Permission + "这个权限" };
                    }
                    return;
                }
            }
        }
    }
}