using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class CityController : Controller
    {
        public ICityService cityService { get; set; }
        public ActionResult List()
        {
            var cities = cityService.GetAll();
            return View(cities);
        }
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