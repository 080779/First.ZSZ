using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class HouseAddDataModel
    {
        public RegionDTO[] Regions { get; set; }        
        public AttachmentDTO[] Attachments { get; set; }       
        public IdNameDTO[] RoomTypes { get; set; }
        public IdNameDTO[] Status { get; set; }
        public IdNameDTO[] DecorateStatus { get; set; }
        public IdNameDTO[] Types { get; set; }
    }
}