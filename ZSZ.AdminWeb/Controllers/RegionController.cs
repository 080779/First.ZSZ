using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class RegionController : Controller
    {
        public IRegionService regionService { get; set; }
        public ActionResult List(long id)
        {
            var regions = regionService.GetAll(id);
            return View(regions);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string name)
        {
            return View();
        }
    }
}