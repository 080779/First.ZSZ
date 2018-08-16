using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class AdminLogService : IAdminLogService
    {
        public void AddNew(long adminUserId, string message)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                //CommonService<AdminLogEntity> cs = new CommonService<AdminLogEntity>(dbc);                
                AdminLogEntity log = new AdminLogEntity();
                log.AdminUserId = adminUserId;
                log.Message = message;
                dbc.AdminLogs.Add(log);
                dbc.SaveChanges(); 
            }
        }
    }
}
