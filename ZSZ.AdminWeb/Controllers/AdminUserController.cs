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
    public class AdminUserController : Controller
    {
        public IAdminUserService adminUserService { get; set; }
        public IRoleService roleService { get; set; }
        public ICityService cityService { get; set; }
        [CheckPermission("AdminUser.List")]
        public ActionResult List()
        {
            var adminUser= adminUserService.GetAll();
            return View(adminUser);
        }
        [CheckPermission("AdminUser.Delete")]
        public ActionResult Delete(long id)
        {
            if(adminUserService.MarkDeleted(id))
            {
                return Json(new AjaxResult { Status = "ok" });
            }
            return Json(new AjaxResult { Status = "error" });
        }
        [CheckPermission("AdminUser.Delete")]
        public ActionResult BatchDelete(long[] selectedIds)
        {
            foreach(long selectedId in selectedIds)
            {
                adminUserService.MarkDeleted(selectedId);
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        [CheckPermission("AdminUser.Add")]
        public ActionResult Add()
        {
            AdminUserAddDataModel addData = new AdminUserAddDataModel();
            addData.Citys = cityService.GetAll();
            addData.Roles = roleService.GetAll();
            return View(addData);
        }
        [HttpPost]
        [CheckPermission("AdminUser.Add")]
        public ActionResult Add(AdminUserAddModel model)
        {
            long uid= adminUserService.AddAdminUser(model.Name, model.PhoneNum, model.Password, model.Email, model.CityId);
            roleService.AddRoleIds(uid, model.RoleIds);
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        [CheckPermission("AdminUser.Edit")]
        public ActionResult Edit(long id)
        {
            AdminUserEditDataModel Data = new AdminUserEditDataModel();
            Data.Citys = cityService.GetAll();
            Data.Roles = roleService.GetAll();
            Data.RoleIds = roleService.GetByAdminUserId(id);
            Data.AdminUser = adminUserService.GetById(id);
            return View(Data);
        }
        [HttpPost]
        [CheckPermission("AdminUser.Edit")]
        public ActionResult Edit(AdminUserEditModel model)
        {
            adminUserService.UpdateAdminUser(model.Id, model.Name, model.Email, model.CityId);
            roleService.UpdateRoleIds(model.Id, model.RoleIds);
            return Json(new AjaxResult { Status = "ok" });
        }
    }
}