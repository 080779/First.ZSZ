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
    public class CityService : ICityService
    {
        public long AddNew(string cityName)
        {                        
            using(MyDbContext dbc=new MyDbContext())
            {
                CommonService<CityEntity> cs = new CommonService<CityEntity>(dbc);
                bool exists= cs.GetAll().Any(c => c.Name == cityName);
                if(exists)
                {
                    throw new ArgumentException("城市已经存在");
                }
                CityEntity city = new CityEntity();
                city.Name = cityName;
                dbc.Cities.Add(city);
                dbc.SaveChanges();
                return city.Id;
            }           
        }

        public CityDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<CityEntity> cs = new CommonService<CityEntity>(dbc);
                return cs.GetAll().Select(c => new CityDTO { Id = c.Id, Name = c.Name, CreateDateTime = c.CreateDateTime }).ToArray();
            }
        }

        public CityDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<CityEntity> cs = new CommonService<CityEntity>(dbc);
                var city = cs.GetAll().SingleOrDefault(c => c.Id == id);
                if(city==null)
                {
                    return null;
                }
                else
                {
                    CityDTO dto = new CityDTO();
                    dto.Id = id;
                    dto.Name = city.Name;
                    dto.CreateDateTime = city.CreateDateTime;
                    return dto;                    
                }
            }
        }
    }
}
