using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.IService;
using ZSZ.WebCommon;

namespace ZSZ.AdminWeb.Controllers
{
    
    public class PermissionController : Controller
    {
        public IPermissionService PmService { get; set; }
        [CheckPermission("Permission.List")]
        public ActionResult List()
        {
            var pms=PmService.GetAll();
            return View(pms);
        }
        [CheckPermission("Permission.Delete")]
        public ActionResult Delete(long id)
        {
            PmService.MarkDeleted(id);
            return RedirectToAction(nameof(List));
        }
        [CheckPermission("Permission.Delete")]
        public ActionResult JsonToDelete(long id)
        {
            bool exists= PmService.MarkDeleted(id);
            if (exists)
            {
                return Json(new AjaxResult() { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult() { Status = "error" });
            }
        }
        [HttpGet]
        [CheckPermission("Permission.Add")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [CheckPermission("Permission.Add")]
        public ActionResult Add(PermissionAddModel model)
        {
            PmService.AddNew(model.Name, model.Description);
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        [CheckPermission("Permission.Edit")]
        public ActionResult Edit(long id)
        {
            var pm= PmService.GetById(id);
            return View(pm);
        }
        [HttpPost]
        [CheckPermission("Permission.Edit")]
        public ActionResult Edit(PermissionEditModel model)
        {
            PmService.UpdatePermission(model.Id, model.Name, model.Description);
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}