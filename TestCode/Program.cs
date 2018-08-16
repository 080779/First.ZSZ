using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.IService;
using ZSZ.Service;
using ZSZ.Service.Entities;
using ZSZ.WebCommon;

namespace TestCode
{
    class Program
    {
        static void Main1(string[] args)
        {
            /*
            using (MyDbContext dbc = new MyDbContext())
            {
                dbc.Database.Delete();
                dbc.Database.Create();
            }*/
            Console.WriteLine("ok");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            //string[] receiveAddress = { "445060968@qq.com","503709348@qq.com"};
            //Dictionary<string, string> dicts = new Dictionary<string, string>();
            //dicts["SMTP"] = "smtp.126.com";
            //dicts["MaliBody"] = "公车上我睡过了车站，一路上我望着霓虹的北京，我的理想把我丢在这个拥挤的人潮";
            //dicts["SendAddress"] = "exceed295@126.com";
            //dicts["MailTitle"] = "理想";
            //dicts["Password"] = "ss890652";
            //CommonHelper.SendEmail(receiveAddress, dicts);
            //NameValueCollection nvc = new NameValueCollection();            
            //nvc.Add("aa","bb");
            //nvc.Add("cc", "http://www.alipay.com/go?aa=11123");
            //string str= MVCHelper.ToQueryString(nvc);
            IAdminUserService adminService = new AdminUserService();
            ICityService cityService = new CityService();
            long i = 1;
            long id = adminService.AddAdminUser("aaa", "18918918989", "123456", "edfe@qq.com", i);
            Console.WriteLine(id);
            Console.ReadKey();
        }
    }
}
