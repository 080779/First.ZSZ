using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.IService;

namespace TestCode
{
    public class UserService1 : ITestService
    {
        public bool IsOk()
        {
            return true;
        }
    }
}
