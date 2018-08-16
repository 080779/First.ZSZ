using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class RegionService : IRegionService
    {
        public RegionDTO[] GetAll(long cityId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RegionEntity> cs = new CommonService<RegionEntity>(dbc);
                return cs.GetAll().Include(r => r.City).AsNoTracking().Where(r => r.CityId == cityId).
                    Select(r => new RegionDTO { Id = r.Id, Name = r.Name, CityId = r.CityId,
                        CityName = r.City.Name, CreateDateTime = r.CreateDateTime }).ToArray();
            }
        }

        public RegionDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RegionEntity> cs = new CommonService<RegionEntity>(dbc);
                var region = cs.GetAll().Include(r => r.City).AsNoTracking().SingleOrDefault(r => r.Id == id);
                if(region==null)
                {
                    return null;
                }
                return new RegionDTO { Id = region.Id, Name = region.Name, CityId = region.Id,
                    CityName = region.City.Name, CreateDateTime = region.CreateDateTime };
            }
        }
    }
}
