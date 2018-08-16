using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class HouseAppointmentService : IHouseAppointmentService
    {
        public long AddNew(long? userId, string name, string phoneNum, long houseId, DateTime visitDate)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                HouseAppointmentEntity houseApp = new HouseAppointmentEntity();
                houseApp.UserId = userId;
                houseApp.Name = name;
                houseApp.PhoneNum = phoneNum;
                houseApp.HouseId = houseId;
                houseApp.VisitDate = visitDate;
                houseApp.Status = "未处理";
                dbc.HouseAppointments.Add(houseApp);
                dbc.SaveChanges();
                return houseApp.Id;
            }            
        }

        public bool Follow(long AdminUserId, long houseAppointmentId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                var app=cs.GetById(houseAppointmentId);
                if(app==null)
                {
                    throw new AggregateException("订单不存在");
                }
                if(app.FollowAdminUserId!=null)
                {
                    return app.FollowAdminUserId == AdminUserId;
                }
                app.FollowAdminUserId = AdminUserId;
                try
                {
                    dbc.SaveChanges();
                    return true;
                }
                catch(DbUpdateConcurrencyException)//如果抛出DbUpdateConcurrencyException异常说明抢单失败，乐观锁
                {
                    return false;
                }
            }
        }
        private HouserAppointmentDTO ToDTO(HouseAppointmentEntity houseApp)
        {
            HouserAppointmentDTO dto = new HouserAppointmentDTO();
            dto.Id = houseApp.Id;
            dto.Name = houseApp.Name;
            dto.PhoneNum = houseApp.PhoneNum;
            dto.RegionName = houseApp.House.Community.Region.Name;
            dto.Status = houseApp.Status;
            dto.UserId = houseApp.UserId;
            dto.VisitDate = houseApp.VisitDate;
            dto.CommunityName = houseApp.House.Community.Name;
            dto.CreateDateTime = houseApp.CreateDateTime;
            dto.FollowAdminUserId = houseApp.FollowAdminUserId;
            if (houseApp.FollowAdminUser != null)
            {
                dto.FollowAdminUserName = houseApp.FollowAdminUser.Name;
            }
            dto.FollowDateTime = houseApp.FollowDateTime;
            dto.HouseId = houseApp.HouseId;
            return dto;
        }
        public HouserAppointmentDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                //var houseApp = cs.GetAll().Include(a => a.House).Include("House.Community").Include("House.Community.Region").
                //Include(a => a.FollowAdminUser).AsNoTracking().SingleOrDefault(a => a.Id == id);
                var houseApp = cs.GetAll().Include(a => a.House).Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community)).
                    Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)).
                    Include(a => a.FollowAdminUser).AsNoTracking().SingleOrDefault(a => a.Id == id);
                if(houseApp==null)
                {
                    return null;
                }
                return ToDTO(houseApp);
            }
        }

        public HouserAppointmentDTO[] GetPagedData(long cityId, string status, int pageSize, int currentIndex)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                var apps = cs.GetAll().Include(a => a.House).Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community)).
                    Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)).
                    Include(a => a.FollowAdminUser).AsNoTracking().Where(a => a.House.Community.Region.CityId == cityId && a.Status == status).
                    OrderBy(a => a.CreateDateTime).Skip(currentIndex).Take(pageSize);
                return apps.ToList().Select(a => ToDTO(a)).ToArray();
            }
        }

        public long GetTotalCount(long cityId, string status)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseAppointmentEntity> cs = new CommonService<HouseAppointmentEntity>(dbc);
                return cs.GetAll().LongCount(a => a.House.Community.Region.CityId == cityId && a.Status == status);
            }
        }
    }
}
