using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    /// 房屋小区接口
    /// </summary>
    public interface ICommunityService:IServiceSupport
    {
        CommunityDTO[] GetByRegionId(long regionId);
    }
}
