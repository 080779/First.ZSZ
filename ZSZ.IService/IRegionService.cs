using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    /// 区域接口
    /// </summary>
    public interface IRegionService:IServiceSupport
    {
        RegionDTO GetById(long id);
        RegionDTO[] GetAll(long cityId);
    }
}
