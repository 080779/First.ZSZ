using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class CommunityService : ICommunityService
    {
        public CommunityDTO[] GetByRegionId(long regionId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<CommunityEntity> cs = new CommonService<CommunityEntity>(dbc);
                return cs.GetAll().Where(c => c.RegionId == regionId).
                    Select(c => new CommunityDTO { Id = c.Id, Name = c.Name, RegionId = regionId, BuiltYear = c.BuiltYear, CreateDateTime = c.CreateDateTime, Location = c.Location, Traffic = c.Traffic }).ToArray();
            }
        }
    }
}
