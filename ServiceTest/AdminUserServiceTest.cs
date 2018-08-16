using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.Service;

namespace ServiceTest
{
    [TestClass]
    public class AdminUserServiceTest
    {
        private AdminUserService userService = new AdminUserService();
        private PermissionService psService = new PermissionService();
        private RoleService roleService = new RoleService();
        [TestMethod]
        public void TestAdminUser()
        {
            long id = userService.AddAdminUser("abc", "13132131131", "123456", "abc@163.com", null);
            var user= userService.GetById(id);
            Assert.AreEqual(user.Name, "abc");
            Assert.AreEqual(user.PhoneNum, "13132131131");
            Assert.AreEqual(user.Email, "abc@163.com");
            Assert.IsNull(user.CityId);
            Assert.IsTrue(userService.CheckLogin("13132131131", "123456"));
            Assert.IsFalse(userService.CheckLogin("13132131131", "324"));
            userService.GetAll();
            Assert.IsNull(userService.GetByPhoneNum("2325252"));
            userService.GetAll(1);
            userService.UpdateAdminUser(id, "bbc", "bbb@qq.com", null);
            userService.MarkDeleted(id);
        }
        [TestMethod]
        public void TestHasPermission()
        {
            string permName1 = Guid.NewGuid().ToString();
            long permId1 = psService.AddNew(permName1, permName1);
            string permName2 = Guid.NewGuid().ToString();
            long permId2= psService.AddNew(permName2, permName2);

            string roleName = Guid.NewGuid().ToString();
            long roleId = roleService.AddNew(roleName);

            string phone = CommonHelper.GetCaptcha(11);
            long uid = userService.AddAdminUser("abc", phone, "123456", "abc@163.com", null);

            roleService.AddRoleIds(uid, new long[] { roleId });
            psService.AddPermissionIds(roleId, new long[] { permId1 });
            psService.GetByRoleId(5);
            psService.UpdatePermissionIds(roleId, new long[] { permId2 });
            Assert.IsFalse(userService.HasPermission(uid, permName1));
            Assert.IsTrue(userService.HasPermission(uid, permName2));
        }
    }
}
