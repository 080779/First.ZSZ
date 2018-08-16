using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class HouseStaticModel
    {
        public HousePicDTO[] HousePics { get; set; }
        public HouseDTO House { get; set; }
        public AttachmentDTO[] Attachments { get; set; }
    }
}