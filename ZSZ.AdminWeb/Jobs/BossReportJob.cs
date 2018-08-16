using Autofac;
using Autofac.Integration.Mvc;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ZSZ.Common;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Jobs
{
    public class BossReportJob : IJob
    {
        private static ILog log = LogManager.GetLogger(typeof(BossReportJob));
        public void Execute(IJobExecutionContext context)
        {
            log.Debug("准备收集新增房源数量");
            try
            {
                StringBuilder builder = new StringBuilder();
                var container = AutofacDependencyResolver.Current.ApplicationContainer;
                using (container.BeginLifetimeScope())
                {
                    var cityService = container.Resolve<ICityService>();
                    var houseService = container.Resolve<IHouseService>();
                    foreach(var city in cityService.GetAll())
                    {
                        int count = houseService.GetTotalNewHouseCount(city.Id);
                        builder = builder.Append(city.Name).Append("新增房源的数量是：").Append(count).AppendLine();
                    }
                }
                log.Debug("收集新增房源数量完成：" + builder);
                string[] receiveAddress = { "445060968@qq.com", "503709348@qq.com" };
                Dictionary<string, string> dicts = new Dictionary<string, string>();
                dicts["SMTP"] = "smtp.126.com";
                dicts["MaliBody"] = builder.ToString();
                dicts["SendAddress"] = "exceed295@126.com";
                dicts["MailTitle"] = "城市新增房源数量";
                dicts["Password"] = "ss890652";
                CommonHelper.SendEmail(receiveAddress, dicts);
                log.Debug("给老板发送新增房源数量报表完成");
            }
            catch (Exception ex)
            {
                log.Error("给老板发报表出错",ex);
            }
        }
    }
}