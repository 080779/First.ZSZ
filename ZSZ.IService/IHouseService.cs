using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    /// 房源服务接口
    /// </summary>
    public interface IHouseService:IServiceSupport
    {
        HouseDTO GetById(long id);
        long GetTotalCount(long cityId, long typeId);
        //long AddNew(HouseDTO house);
        long AddNew(HouseAddNewDTO house);
        HouseDTO[] GetAll();
        HouseDTO[] GetAll(long cityId);
        bool Update(HouseEditDTO house);
        void MarkDeleted(long id);
        HousePicDTO[] GetPics(long houseId);
        bool CheckPic(string url);
        long AddNewHousePic(HousePicDTO housePic);
        bool DeletedHousePic(long housePicId);
        HouseDTO[] GetPageData(long cityId, long typeId, int pageSize, int currentIndex);
        HouseSearchResult Search(HouseSearchOptions options);
        long GetCount(long cityId, DateTime startDateTime, DateTime endDateTime);
        int GetTotalNewHouseCount(long cityId);
    }
    public class HouseSearchOptions
    {
        public long CityId { get; set;}
        public long TypeId { get; set; }
        public long? RegionId { get; set; }
        public int? StartMonthRent { get; set; }
        public int? EndMonthRent { get; set; }
        public OrderByType OrderByType { get; set; } = OrderByType.MonRentAsc;
        public string KeyWords { get; set; }
        public int PageSize { get; set; }
        public int CurrentIndex { get; set; }
    }
    public enum OrderByType
    {
        MonthRentDesc=1,MonRentAsc=2,AreaDesc=4,AreaAsc=8,CreateDateDesc=16
    }
    public class HouseSearchResult
    {
        public HouseDTO[] Houses { get; set; }//当前页的数据
        public long TotalCount { get; set; }//搜索的结果总条数
    }
}
