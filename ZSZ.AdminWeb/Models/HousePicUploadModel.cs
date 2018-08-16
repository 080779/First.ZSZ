using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class HousePicUploadModel
    {
        public long HouseId { get; set; }
        public HousePicDTO[] Pics { get; set; }
    }
}