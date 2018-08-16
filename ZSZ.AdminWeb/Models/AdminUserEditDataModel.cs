using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserEditDataModel
    {
        public RoleDTO[] RoleIds { get; set; }
        public CityDTO[] Citys { get; set; }
        public RoleDTO[] Roles { get; set; }
        public AdminUserDTO AdminUser { get; set; }
    }
}