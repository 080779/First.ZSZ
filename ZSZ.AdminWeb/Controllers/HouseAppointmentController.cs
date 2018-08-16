using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class HouseAppointmentController : Controller
    {
        public IHouseAppointmentService appService { get; set; }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Follow(long id)
        {
            bool isOk=appService.Follow(2, id);
            if(isOk)
            {
                return Content("抢单成功");
            }
            else
            {
                return View("Error", (object)"抢单失败");
            }
        }
    }
}