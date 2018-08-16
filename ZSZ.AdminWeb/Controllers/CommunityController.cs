using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class CommunityController : Controller
    {
        public ICommunityService communityService { get; set; }
        public ActionResult List(long id)
        {
            var communities = communityService.GetByRegionId(id);
            return View(communities);
        }
    }
}