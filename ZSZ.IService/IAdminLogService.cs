using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.IService
{
    /// <summary>
    /// 日志管理接口
    /// </summary>
    public interface IAdminLogService:IServiceSupport
    {
        void AddNew(long adminUserId, string message);
    }
}
