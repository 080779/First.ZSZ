using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ZSZ.Common;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;
using ZSZ.WebCommon;

namespace ZSZ.FrontWeb.Controllers
{
    public class MainController : Controller
    {
        public ICityService CityService { get; set; }
        public ISettingService SettingService { get; set; }
        public IHouseService houseService { get; set; }
        public IIdNameService idNameService { get; set; }
        public IRegionService regionService { get; set; }
        public IHouseAppointmentService appointmentService { get; set; }
        public IAttachmentService attachmentService { get; set; }
        public IUserService userService { get; set; }
        ITestService userService1 { get; set; }

        public ActionResult Index()
        {
            IndexModel model = new IndexModel();
            model.Houses= houseService.GetPageData(4, 18, 10, 1);
            model.Types = idNameService.GetAll("房屋类别");
            //var houses = houseService.GetAll(1);
            return View(model);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string phoneNum,string password)
        {
            var user= userService.GetByPhoneNum(phoneNum);
            string passwordHash = CommonHelper.GetMD5(user.PasswordSalt + password);
            if(user==null)
            {
                return Json(new AjaxResult{Status="error",ErrorMsg="用户名密码错误" });
            }
            if(user.PasswordHash!=passwordHash)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "用户名密码错误" });
            }
            Session["UserId"] = user.Id;
            return Json(new AjaxResult { Status="ok"});
        }
        [HttpGet]
        public ActionResult Register()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            long userId=userService.AddNew(model.PhoneNum, model.Password);
            return Json(new AjaxResult { Status="ok"});
        }
        [HttpGet]
        public ActionResult Modify()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Modify(string phoneNum, string verifyCode)
        {
            string VerifyCode = (string)TempData["verifyCode"];
            if (verifyCode != VerifyCode)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }

            string userName = "080779";
            string appKey = "be4e558596caefa4720eb8";
            string templateId = "436";
            string code = new Random().Next(100000, 999999).ToString();
            TempData["code"] = code;
            TempData["phoneNum"] = phoneNum;
            var result = MVCHelper.SendMessage(userName, appKey, templateId, code, phoneNum);
            //ViewBag.Phone = GetPhoneNum(phoneNum);
            result.phoneNum = phoneNum;
            return Json(result);
        }
        public ActionResult SendMessage(string phoneNum)
        {
            string model=  GetPhoneNum(phoneNum);

            return View((object)model);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Complete()
        {
            return View();
        }
        public ActionResult CreateVerifyCode()
        {
            string verifyCode = CommonHelper.GetCaptcha(4);
            TempData["verifyCode"] = verifyCode;
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 20, 6);
            return File(ms, "image/jpeg");
        }
        public ActionResult PhoneMessage(string phoneNum,string verifyCode)
        {            
            string VerifyCode = (string)TempData["verifyCode"];
            if(verifyCode!=VerifyCode)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "验证码错误" });
            }

            string userName = "080779";
            string appKey = "be4e558596caefa4720eb8";
            string templateId = "436";
            string code =new Random().Next(100000, 999999).ToString();
            TempData["code"] = code;
            TempData["phoneNum"] = phoneNum;
            var result= MVCHelper.SendMessage(userName, appKey, templateId, code, phoneNum);
            //ViewBag.Phone = GetPhoneNum(phoneNum);
            result.phoneNum = phoneNum;
            return Json(result);                      
        }
        public string GetPhoneNum(string phoneNum)
        {
            StringBuilder builder = new StringBuilder();
            for(int i=0;i<phoneNum.Length;i++)
            {
                if(i>=3 && i<=6)
                {
                    builder = builder.Append("*");
                }
                else
                {
                    builder = builder.Append(phoneNum[i]);
                }                
            }
            return builder.ToString();
        }
        public ActionResult Search(long typeId,long? regionId,string monthRent,string orderByType)
        {

            SearchViewModel model = new Models.SearchViewModel();
            HouseSearchResult result = new HouseSearchResult();
            HouseSearchOptions options = new HouseSearchOptions();
            options.CityId = 4;
            options.TypeId = typeId;
            options.RegionId = regionId;
            options.CurrentIndex = 1;
            options.PageSize = 10;
            int? startMonthRent;
            int? endMonthRent;
            GetMonthRent(monthRent, out startMonthRent, out endMonthRent);
            options.StartMonthRent = startMonthRent;
            options.EndMonthRent = endMonthRent;
            switch (orderByType)
            {
                case "AscMonthRent":
                    options.OrderByType = OrderByType.MonRentAsc;
                    break;
                case "DescMonthRent":
                    options.OrderByType = OrderByType.MonthRentDesc;
                    break;
                case "AscArea":
                    options.OrderByType = OrderByType.AreaAsc;
                    break;
                case "DescArea":
                    options.OrderByType = OrderByType.AreaDesc;
                    break;
                case "DescCreateDateTime":
                    options.OrderByType = OrderByType.CreateDateDesc;
                    break;
            }
            result= houseService.Search(options);
            model.Regions = regionService.GetAll(4);
            model.Houses = result.Houses;
            return View(model);
        }
        public ActionResult Search2(long typeId)
        {
            var regions = regionService.GetAll(4);
            return View(regions);
        }
        public ActionResult LoadMore(long typeId, long? regionId, string monthRent, string orderByType,int currentIndex)
        {            
            HouseSearchResult result = new HouseSearchResult();
            HouseSearchOptions options = new HouseSearchOptions();
            options.CityId = 4;
            options.TypeId = typeId;
            options.RegionId = regionId;
            options.CurrentIndex =currentIndex;
            options.PageSize = 10;
            int? startMonthRent;
            int? endMonthRent;
            GetMonthRent(monthRent, out startMonthRent, out endMonthRent);
            options.StartMonthRent = startMonthRent;
            options.EndMonthRent = endMonthRent;
            switch (orderByType)
            {
                case "AscMonthRent":
                    options.OrderByType = OrderByType.MonRentAsc;
                    break;
                case "DescMonthRent":
                    options.OrderByType = OrderByType.MonthRentDesc;
                    break;
                case "AscArea":
                    options.OrderByType = OrderByType.AreaAsc;
                    break;
                case "DescArea":
                    options.OrderByType = OrderByType.AreaDesc;
                    break;
                case "DescCreateDateTime":
                    options.OrderByType = OrderByType.CreateDateDesc;
                    break;
            }
            result = houseService.Search(options);            
            var Houses = result.Houses;
            return Json(new AjaxResult { Status="ok",Data=Houses});
        }
        public ActionResult House(long id)
        {
            /*
            HouseModel model = new HouseModel();
            model.House = houseService.GetById(id);
            model.HousePics = houseService.GetPics(id);
            model.Attachments = attachmentService.GetAttachments(id);
            return View(model);*/
            string cacheKey = "House_" + id;
            HouseModel model = MemcachedMgr.Instance.GetValue<HouseModel>(cacheKey);
            if(model==null)
            {
                var house= houseService.GetById(id);
                if(house==null)
                {
                    return View("Error", (object)"不存在的房源id");
                }
                model = new HouseModel();
                model.House = house;
                model.HousePics = houseService.GetPics(id);
                model.Attachments = attachmentService.GetAttachments(id);
                MemcachedMgr.Instance.SetValue(cacheKey, model, TimeSpan.FromMinutes(1));
            }            
            return View(model);
        }
        public ActionResult MakeAppointment(HouseMakeAppointment model)
        {
            if(!ModelState.IsValid)
            {
                string msg = MVCHelper.GetValidMsg(ModelState);
                return Json(new AjaxResult { Status = "error", ErrorMsg = msg });
            }
            long? userId = FrontHelper.GetUserId(HttpContext);
            appointmentService.AddNew(userId, model.Name, model.PhoneNum, model.HouseId, model.VisitDate);
            return Json(new AjaxResult { Status = "ok" });
        }
        public void GetMonthRent(string value,out int? startMonthRent,out int? endMonthRent)
        {
            if(string.IsNullOrEmpty(value))
            {
                startMonthRent = null;
                endMonthRent = null;
                return;
            }
            string[] result = value.Split('-');
            if(result[0]=="*")
            {
                startMonthRent = null;
            }
            else
            {
                startMonthRent = Convert.ToInt32(result[0]);
            }
            if (result[1] == "*")
            {
                endMonthRent = null;
            }
            else
            {
                endMonthRent = Convert.ToInt32(result[0]);
            }
        }
    }
}