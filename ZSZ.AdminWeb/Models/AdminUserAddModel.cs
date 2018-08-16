﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserAddModel
    {
        public string PhoneNum { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? CityId { get; set; }
        public long[] RoleIds { get; set; }
    }
}