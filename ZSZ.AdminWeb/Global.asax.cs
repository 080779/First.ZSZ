using Autofac;
using Autofac.Integration.Mvc;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Jobs;
using ZSZ.IService;
using ZSZ.WebCommon;

namespace ZSZ.AdminWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            ModelBinders.Binders.Add(typeof(string), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(int), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(long), new TrimToDBCModelBinder());
            ModelBinders.Binders.Add(typeof(double), new TrimToDBCModelBinder());

            var builder = new ContainerBuilder();//把当前程序集中的 Controller 都注册,不要忘了.PropertiesAutowired()            
            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            
            Assembly[] assemblies = new Assembly[] { Assembly.Load("ZSZ.Service")};// 获取所有相关类库的程序集
            builder.RegisterAssemblyTypes(assemblies).
                Where(type => !type.IsAbstract && typeof(IServiceSupport).IsAssignableFrom(type)).AsImplementedInterfaces().PropertiesAutowired();
            //type1.IsAssignableFrom(type2):Assign赋值，type1类型的变量是否可以指向type2类型的对象。也就是type2是否实现type1接口/type2是否继承自type1

            //注册系统级别的 DependencyResolver，这样当 MVC 框架创建 Controller 等对象的时候都是管 Autofac 要对象。            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalFilters.Filters.Add(new JsonNetActionFilter());
            GlobalFilters.Filters.Add(new ZSZExceptionFilter());
            GlobalFilters.Filters.Add(new ZSZAuthorizationFilter());
            
            StartQuartz();
        }
        private void StartQuartz()
        {
            IScheduler sched = new StdSchedulerFactory().GetScheduler();
            //给老板的报表开始
            JobDetailImpl jdBossReport = new JobDetailImpl("jdBossReport", typeof(BossReportJob));
            IMutableTrigger triggerBossReport = CronScheduleBuilder.DailyAtHourAndMinute(18, 07).Build();//每天 23:45 执行一次
            triggerBossReport.Key = new TriggerKey("triggerBossReport");
            sched.ScheduleJob(jdBossReport, triggerBossReport);
            //给老板的报表结束

            sched.Start();
        }
    }
}
