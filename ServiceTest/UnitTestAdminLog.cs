using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZSZ.Service;

namespace ServiceTest
{
    [TestClass]
    public class UnitTestAdminLog
    {
        [TestMethod]
        public void TestAddNew()
        {
            new AdminLogService().AddNew(1,"测试消息");
        }
    }
}
