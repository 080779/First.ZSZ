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
    public class RoleController : Controller
    {
        public IRoleService roleService { get; set; }
        public IPermissionService permissionService { get; set; }
        [CheckPermission("Role.List")]
        public ActionResult List()
        {
            var roles= roleService.GetAll();
            return View(roles);
        }
        [CheckPermission("Role.Delete")]
        public ActionResult Delete(long id)
        {
            if (roleService.MarkDeleted(id))
            {
                return Json(new AjaxResult { Status = "ok" });
            }
            return Json(new AjaxResult { Status = "error" });
        }
        [CheckPermission("Role.Delete")]
        public ActionResult BatchDelete(long[] selectedIds)
        {
            foreach (long selectedId in selectedIds)
            {
                roleService.MarkDeleted(selectedId);
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        [CheckPermission("Role.Add")]
        public ActionResult Add()
        {
            var permission = permissionService.GetAll();
            return View(permission);
        }
        [HttpPost]
        [CheckPermission("Role.Add")]
        public ActionResult Add(RoleAddModel model)
        {
            long roleId=roleService.AddNew(model.Name);
            permissionService.AddPermissionIds(roleId, model.PermissionIds);
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        [CheckPermission("Role.Edit")]
        public ActionResult Edit(long id)
        {
            RoleEditModels models = new RoleEditModels();
            models.role = roleService.GetById(id);
            models.RolePermissions = permissionService.GetByRoleId(id);
            models.Permissions = permissionService.GetAll();
            return View(models);
        }
        [HttpPost]
        [CheckPermission("Role.Edit")]
        public ActionResult Edit(RoleEditModel model)
        {
            roleService.Update(model.Id, model.Name);
            permissionService.UpdatePermissionIds(model.Id, model.PermissionIds);
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}