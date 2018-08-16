using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class RoleEditModels
    {
        public RoleDTO role { get; set; }
        public  PermissionDTO[] RolePermissions { get; set; }
        public PermissionDTO[] Permissions { get; set; }
    }
}