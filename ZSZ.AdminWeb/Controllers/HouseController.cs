using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.WebCommon;

namespace ZSZ.AdminWeb.Controllers
{
    public class HouseController : Controller
    {
        public IHouseService houseService { get; set; }
        public IAdminUserService userService { get; set; }
        public IAttachmentService attachmentService { get; set; }
        public IIdNameService idNameService { get; set; }
        public IRegionService regionService { get; set; }
        public ICommunityService communityService { get; set; }
        public ICityService cityService { get; set; }
        [CheckPermission("House.List")]
        [HttpGet]
        public ActionResult List()
        {
            long userId=(long)HttpContext.Session["AdminUserId"];
            long? cityId = userService.GetById(userId).CityId;
            if(cityId==null)
            {
                return View("Error", (object)"总部的人不能进行房源管理");
            }
            HouseListModel model = new HouseListModel();
            model.Houses = houseService.GetAll((long)cityId);
            model.Cities = cityService.GetAll();
            model.SelectedCityId = (long)cityId;
            return View(model);
        }
        [CheckPermission("House.List")]
        public ActionResult HouseList(long cityId,int pageIndex)
        {
            HouseListModel model = new HouseListModel();
            var houses= houseService.GetAll(cityId);
            foreach(var house in houses)
            {
                CreateStaticPage(house.Id);
            }
            model.Houses = houses;
            model.Cities = cityService.GetAll();
            model.SelectedCityId = cityId;
            DataPaging pager = new DataPaging();
            pager.PageIndex = pageIndex;
            pager.TotalCount = houseService.GetAll(cityId).Count();
            pager.UrlPattern = "/House/HouseList?cityId=" + cityId + "&pageIndex={pn}";
            ViewBag.pager = pager.GetPager();
            return View(model);
        }
        [CheckPermission("House.Delete")]
        public ActionResult Delete()
        {
            return View();
        }
        [CheckPermission("House.Delete")]
        public ActionResult BatchDelete()
        {
            return View();
        }
        [HttpGet]
        [CheckPermission("House.Add")]        
        public ActionResult Add()
        {
            long userId = (long)HttpContext.Session["AdminUserId"];
            long? cityId = userService.GetById(userId).CityId;
            if (cityId == null)
            {
                return View("Error", (object)"总部的人不能进行房源管理");
            }
            HouseAddDataModel model = new HouseAddDataModel();
            model.Attachments = attachmentService.GetAll();
            model.DecorateStatus = idNameService.GetAll("装修状态");
            model.RoomTypes = idNameService.GetAll("户型");
            model.Status = idNameService.GetAll("房屋状态");
            model.Types = idNameService.GetAll("房屋类别");
            model.Regions = regionService.GetAll((long)cityId);
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]        
        [CheckPermission("House.Add")]
        //加上[ValidateInput(false)]用ueditor无效是因为要在web.config中加上配置<system.web><httpRuntime requestValidationMode = "2.0" /></ system.web >
        //requestValidationMode = "2.0",这个版本的[ValidateInput(false)]才不会校验是否含有html标签
        public ActionResult Add(HouseAddNewDTO model)
        {
            long id=houseService.AddNew(model);
            if(id>0)
            {
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult { Status="error",ErrorMsg="添加失败"});
            }
        }
        private void CreateStaticPage(long houseId)
        {
            var house = houseService.GetById(houseId);
            var pics = houseService.GetPics(houseId);
            var attachments = attachmentService.GetAttachments(houseId);

            HouseStaticModel model = new HouseStaticModel();
            model.House = house;
            model.HousePics = pics;
            model.Attachments = attachments;
            string html = MVCHelper.RenderViewToString(this.ControllerContext, "~/Views/House/HouseStatic.cshtml", model);
            System.IO.File.WriteAllText(@"D:\项目代码\ZSZ\ZSZ.FrontWeb\"+houseId+".html", html);
        }
        [HttpGet]
        [CheckPermission("House.Edit")]
        public ActionResult Edit(long id)
        {            
            long userId = (long)HttpContext.Session["AdminUserId"];
            long? cityId = userService.GetById(userId).CityId;
            if (cityId == null)
            {
                return View("Error", (object)"总部的人不能进行房源管理");
            }
            HouseEditDataModel model = new HouseEditDataModel();
            model.Attachments = attachmentService.GetAll();
            model.DecorateStatus = idNameService.GetAll("装修状态");
            model.RoomTypes = idNameService.GetAll("户型");
            model.Status = idNameService.GetAll("房屋状态");
            model.Types = idNameService.GetAll("房屋类别");
            var house= houseService.GetById(id);
            model.Regions = regionService.GetAll(house.CityId);
            model.house = house;
            return View(model);
        }
        [HttpPost]
        [CheckPermission("House.Edit")]
        public ActionResult Edit(HouseEditDTO model)
        {
            if(houseService.Update(model))
            {
                return Json(new AjaxResult { Status="ok"});
            }
            else
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "更新失败" });
            }
        }
        public ActionResult GetCommunity(long id)
        {
            var communities=communityService.GetByRegionId(id);
            return Json(new AjaxResult { Status = "ok", Data = communities });
        }
        [HttpGet]
        public ActionResult PicUpload(long id)
        {
            return View(id);
        }
        [HttpPost]
        public ActionResult PicUpload(long houseId,HttpPostedFileBase file)
        {
            string md5 = CommonHelper.GetMD5(file.InputStream);
            string ext = Path.GetExtension(file.FileName);
            string path = "/upload/" + DateTime.Now.ToString("yyyy/MM/dd")+"/"+md5+ext;
            string thumbPath= "/upload/" + DateTime.Now.ToString("yyyy/MM/dd") + "/" + md5+"_thumb"+ext;            
            string fullPath = HttpContext.Server.MapPath("~"+path);
            string thumbFullPath = HttpContext.Server.MapPath("~"+thumbPath);
            new FileInfo(fullPath).Directory.Create();
            //file.SaveAs(fullPath);
            //缩略图
            file.InputStream.Position = 0;
            ImageProcessingJob jobThumb = new ImageProcessingJob();
            jobThumb.Filters.Add(new FixedResizeConstraint(200, 200));//缩略图尺寸 200*200
            jobThumb.SaveProcessedImageToFileSystem(file.InputStream, thumbFullPath);
            //水印
            file.InputStream.Position = 0;
            ImageWatermark imgWatermark = new ImageWatermark(HttpContext.Server.MapPath("~/images/fb.png"));
            imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;//水印位置
            imgWatermark.Alpha = 50;//透明度，需要水印图片是背景透明的 png 图片
            ImageProcessingJob jobNormal = new ImageProcessingJob();
            jobNormal.Filters.Add(imgWatermark);//添加水印
            jobNormal.Filters.Add(new FixedResizeConstraint(600, 600));//限制图片的大小，避免生成
            jobNormal.SaveProcessedImageToFileSystem(file.InputStream,fullPath);

            if(!houseService.CheckPic(path))
            {
                houseService.AddNewHousePic(new HousePicDTO { HouseId = houseId, Url = path, ThumbUrl = thumbPath });                
            }
            return Json(new AjaxResult { Status = "ok" });
        }
        public ActionResult PicDelete(long[] ids)
        {
            foreach(long id in ids)
            {
                houseService.DeletedHousePic(id);
            }
            return Json(new AjaxResult { Status="ok"});
        }
        public ActionResult PicList(long id)
        {
            HousePicUploadModel model= new HousePicUploadModel();
            model.Pics= houseService.GetPics(id);
            model.HouseId = id;
            return View(model);
        }
    }
}