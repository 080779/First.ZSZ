using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.IService;
using ZSZ.WebCommon;

namespace ZSZ.AdminWeb.Controllers
{
    public class IdNameController : Controller
    {
        public IIdNameService idNameService { get; set; }
        public ActionResult List()
        {
            var idName= idNameService.GetAll();
            return View(idName);
        }
        public ActionResult Delete(long id)
        {
            return View();
        }
        public ActionResult BatchDelete(long[] ids)
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Add(IdNameAddModel model)
        {
            long id= idNameService.AddNew(model.TypeName, model.Name,model.PngUrl);
            return Json(new AjaxResult { Status = "ok" });
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dto= idNameService.GetById(id);
            return View(dto);
        }
        [HttpPost]
        public ActionResult Edit(IdNameEditModel model)
        {
            return View();
        }
    }
}