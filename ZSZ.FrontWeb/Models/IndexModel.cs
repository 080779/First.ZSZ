using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.FrontWeb.Models
{
    public class IndexModel
    {
        public HouseDTO[] Houses { get; set; }
        public IdNameDTO[] Types { get; set; }
    }
}