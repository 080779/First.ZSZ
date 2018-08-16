using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class HouseService : IHouseService
    {
        public long AddNew(HouseAddNewDTO house)
        {
            HouseEntity entity = new HouseEntity();
            entity.Address = house.Address;
            entity.Area = house.Area;
            entity.CheckInDateTime = house.CheckInDateTime;
            entity.CommunityId = house.CommunityId;
            entity.DecorateStatusId = house.DecorateStatusId;
            entity.Description = house.Description;
            entity.Direction = house.Direction;
            entity.FloorIndex = house.FloorIndex;
            entity.LookableDateTime = house.LookableDateTime;
            entity.MouthRent = house.MonthRent;
            entity.OwnerName = house.OwnerName;
            entity.OwnerPhoneNum = house.OwnerPhoneNum;
            entity.RoomTypeId = house.RoomTypeId;
            entity.StatusId = house.StatusId;
            entity.TotalFloorCount = house.TotalFloorCount;
            entity.TypeId = house.TypeId;
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AttachmentEntity> cs = new CommonService<AttachmentEntity>(dbc);
                var ams =cs.GetAll().Where(a => house.AttachmentIds.Contains(a.Id));
                foreach(var am in ams)
                {
                    entity.Attachments.Add(am);
                }
                dbc.Houses.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }
        }

        public long AddNewHousePic(HousePicDTO housePic)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                HousePicEntity pic = new HousePicEntity();
                pic.HouseId = housePic.HouseId;
                pic.Url = housePic.Url;
                pic.ThumbUrl = housePic.ThumbUrl;
                dbc.HousePics.Add(pic);
                dbc.SaveChanges();
                return pic.Id;
            }
        }
        private HouseDTO ToDTO(HouseEntity house)
        {
            HouseDTO dto = new HouseDTO();
            dto.Address = house.Address;
            dto.Area = house.Area;
            dto.AttachmentIds = house.Attachments.Select(a => a.Id).ToArray();
            dto.CheckInDateTime = house.CheckInDateTime;
            dto.CityId = house.Community.Region.CityId;
            dto.CityName = house.Community.Region.City.Name;
            dto.CommunityBuiltYear = house.Community.BuiltYear;
            dto.CommunityId = house.CommunityId;
            dto.CommunityLocation = house.Community.Location;
            dto.CommunityName = house.Community.Name;
            dto.CommunityTraffic = house.Community.Traffic;
            dto.CreateDateTime = house.CreateDateTime;
            dto.DecorateStatusId = house.DecorateStatusId;
            dto.DecorateStatusName = house.DecorateStatus.Name;
            dto.Description = house.Description;
            dto.Direction = house.Direction;
            var firstPic = house.HousePics.FirstOrDefault();
            if(firstPic!=null)
            {
                dto.FirstThumbUrl = firstPic.ThumbUrl;
            }
            dto.FloorIndex = house.FloorIndex;
            dto.Id = house.Id;
            dto.LookableDateTime = house.LookableDateTime;
            dto.MonthRent = house.MouthRent;
            dto.OwnerName = house.OwnerName;
            dto.OwnerPhoneNum = house.OwnerPhoneNum;
            dto.RegionId = house.Community.RegionId;
            dto.RegionName = house.Community.Region.Name;
            dto.RoomTypeId = house.RoomTypeId;
            dto.RoomTypeName = house.RoomType.Name;
            dto.StatusId = house.StatusId;
            dto.StatusName = house.Status.Name;
            dto.TotalFloorCount = house.TotalFloorCount;
            dto.TypeId = house.TypeId;
            dto.TypeName = house.Type.Name;
            return dto;
        }
        public HouseDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                var house = cs.GetAll().Include(h => h.Community).Include(h => h.RoomType).Include(h => h.Status).Include(h => h.DecorateStatus).
                    Include(h => h.Type).Include(h => h.Attachments).Include(h => h.HousePics).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City)).
                    AsNoTracking().SingleOrDefault(h => h.Id == id);
                if(house==null)
                {
                    return null;
                }
                return ToDTO(house);
            }
        }

        public HouseDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                return cs.GetAll().Include(h => h.Community).Include(h => h.RoomType).Include(h => h.Status).Include(h => h.DecorateStatus).
                    Include(h => h.Type).Include(h => h.Attachments).Include(h => h.HousePics).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City)).
                    ToArray().Select(h => ToDTO(h)).ToArray();
                //return cs.GetAll().ToArray().Select(h => ToDTO(h)).ToArray();
            }
        }

        public HouseDTO[] GetAll(long cityId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                return cs.GetAll().Include(h => h.Community).Include(h => h.RoomType).Include(h => h.Status).Include(h => h.DecorateStatus).
                    Include(h => h.Type).Include(h => h.Attachments).Include(h => h.HousePics).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City)).
                    ToArray().Select(h => ToDTO(h)).Where(h => h.CityId == cityId).ToArray();
                //return cs.GetAll().ToArray().Select(h => ToDTO(h)).ToArray();
            }
        }

        public long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                return cs.GetAll().LongCount(h => h.Community.Region.
                    CityId == cityId && h.CreateDateTime >= startDateTime && h.CreateDateTime <= endDateTime);
            }
        }

        public HousePicDTO[] GetPics(long houseId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HousePicEntity> cs = new CommonService<HousePicEntity>(dbc);
                var pics=cs.GetAll().Where(h => h.HouseId == houseId).Include(h=>h.House).AsNoTracking();
                if(pics==null)
                {
                    throw new ArgumentException("houseId=" + houseId + "的数据不存在");
                }
                return pics.Select(p => new HousePicDTO { CreateDateTime = p.CreateDateTime, HouseId = p.HouseId,
                    Id = p.Id, ThumbUrl = p.ThumbUrl, Url = p.Url }).ToArray();
            }
        }
        public bool CheckPic(string url)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HousePicEntity> cs = new CommonService<HousePicEntity>(dbc);
                var pic=cs.GetAll().SingleOrDefault(p => p.Url == url);
                if(pic==null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }                
        }
        public long GetTotalCount(long cityId, long typeId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                return cs.GetAll().LongCount(h => h.Community.Region.CityId == cityId && h.TypeId==typeId);
            }
        }

        public void MarkDeleted(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                var house = cs.GetAll().SingleOrDefault(h => h.Id == id);
                if(house!=null)
                {
                    cs.MarkDeleted(id);
                }
            }
        }

        public bool DeletedHousePic(long housePicId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HousePicEntity> cs = new CommonService<HousePicEntity>(dbc);
                var pic=cs.GetById(housePicId);
                //HousePicEntity pic = new HousePicEntity();
                //pic.Id = housePicId;
                if(pic==null)
                {
                    return false;
                }
                else
                {
                    dbc.Entry(pic).State = EntityState.Deleted;
                    dbc.SaveChanges();
                    return true;
                }
                
                /*
                CommonService<HousePicEntity> cs = new CommonService<HousePicEntity>(dbc);
                var pic = cs.GetAll().SingleOrDefault(p => p.Id == housePicId);
                if(pic!=null)
                {
                    cs.MarkDeleted(housePicId);
                    //dbc.HousePics.Remove(pic);
                    //dbc.SaveChanges();
                }*/
            }
        }

        public HouseSearchResult Search(HouseSearchOptions options)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                var items = cs.GetAll().Include(h => h.Community).Include(h => h.RoomType).Include(h => h.Status).Include(h => h.DecorateStatus).
                    Include(h => h.Type).Include(h => h.Attachments).Include(h => h.HousePics).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City)).
                    Where(h => h.Community.Region.CityId == options.CityId && h.TypeId == options.TypeId);
                HouseSearchResult result = new HouseSearchResult();
                result.TotalCount = items.Count();
                if (options.RegionId!=null)
                {
                    items = items.Where(h => h.Community.RegionId == options.RegionId);
                }
                if (options.StartMonthRent != null)
                {
                    items = items.Where(h => h.MouthRent>=options.StartMonthRent);
                }
                if (options.EndMonthRent != null)
                {
                    items = items.Where(h => h.MouthRent <= options.EndMonthRent);
                }
                if(!string.IsNullOrEmpty(options.KeyWords))
                {
                    items = items.Where(h => h.Address.Contains(options.KeyWords) ||
                        h.Description.Contains(options.KeyWords) ||
                        h.Community.Name.Contains(options.KeyWords) ||
                        h.Community.Traffic.Contains(options.KeyWords) ||
                        h.Community.Location.Contains(options.KeyWords));
                }
                switch(options.OrderByType)
                {
                    case OrderByType.AreaAsc:
                        items = items.OrderBy(h => h.Area);
                        break;
                    case OrderByType.AreaDesc:
                        items = items.OrderByDescending(h => h.Area);
                        break;
                    case OrderByType.MonRentAsc:
                        items = items.OrderBy(h => h.MouthRent);
                        break;
                    case OrderByType.MonthRentDesc:
                        items = items.OrderByDescending(h => h.MouthRent);
                        break;
                    case OrderByType.CreateDateDesc:
                        items = items.OrderByDescending(h => h.CreateDateTime);
                        break;
                }
                items = items.Skip((options.CurrentIndex - 1) * options.PageSize).Take(options.PageSize);
                result.Houses = items.ToList().Select(h => ToDTO(h)).ToArray();
                return result;
            }
        }

        public bool Update(HouseEditDTO house)
        {              
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                HouseEntity entity = cs.GetById(house.Id);
                if(entity==null)
                {
                    return false;
                }
                entity.Address = house.Address;
                entity.Area = house.Area;
                entity.Attachments.Clear();
                var ams = dbc.Attachments.Where(a => a.IsDeleted == false && house.AttachmentIds.Contains(a.Id));
                foreach(var am in ams)
                {
                    entity.Attachments.Add(am);
                }
                entity.CheckInDateTime = house.CheckInDateTime;
                entity.CommunityId = house.CommunityId;
                entity.DecorateStatusId = house.DecorateStatusId;
                entity.Description = house.Description;
                entity.Direction = house.Direction;
                entity.FloorIndex = house.FloorIndex;
                entity.LookableDateTime = house.LookableDateTime;
                entity.MouthRent = house.MonthRent;
                entity.OwnerName = house.OwnerName;
                entity.OwnerPhoneNum = house.OwnerPhoneNum;
                entity.RoomTypeId = house.RoomTypeId;
                entity.StatusId = house.StatusId;
                entity.TotalFloorCount = house.TotalFloorCount;
                entity.TypeId = house.TypeId;
                dbc.SaveChanges();
                return true;
            }
        }

        public HouseDTO[] GetPageData(long cityId, long typeId, int pageSize, int currentIndex)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                return cs.GetAll().Include(h => h.Community).Include(h => h.RoomType).Include(h => h.Status).Include(h => h.DecorateStatus).
                    Include(h => h.Type).Include(h => h.Attachments).Include(h => h.HousePics).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region)).
                    Include(nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region) + "." + nameof(RegionEntity.City)).
                    AsNoTracking().OrderBy(h => h.CreateDateTime).Skip((currentIndex-1)* pageSize).Take(pageSize).
                    Where(h => h.Community.Region.CityId == cityId && h.TypeId == typeId).ToList().
                    Select(h => ToDTO(h)).ToArray();
            }
        }

        public int GetTotalNewHouseCount(long cityId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                //int count = cs.GetAll().Include(h => h.Community).Include("Community.Region").Count(h => h.Community.Region.CityId == cityId && (DateTime.Now - h.CreateDateTime).TotalHours <= 24);
                int count = cs.GetAll().Include(h => h.Community).Include("Community.Region").Count(h => h.Community.Region.CityId == cityId && SqlFunctions.DateDiff("hh",h.CreateDateTime,DateTime.Now)<=24);
                return count;
            }
        }
    }
}
