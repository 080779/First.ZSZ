using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Models
{
    public class SearchViewModel
    {
        public HouseDTO[] Houses { get; set; }
        public RegionDTO[] Regions { get; set; }
    }
}