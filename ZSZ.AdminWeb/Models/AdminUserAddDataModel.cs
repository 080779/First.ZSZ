using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserAddDataModel
    {
        public CityDTO[] Citys { get; set; }
        public RoleDTO[] Roles { get; set; }
    }
}