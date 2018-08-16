using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;
using ZSZ.WebCommon;

namespace ZSZ.AdminWeb.Models
{
    public class HouseListModel
    {
        public CityDTO[] Cities { get; set; }
        public HouseDTO[] Houses { get; set; }
        public long SelectedCityId { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
        public string UrlPattern { get; set; }
    }
}