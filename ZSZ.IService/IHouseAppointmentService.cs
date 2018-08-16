using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    /// 预约看房服务接口
    /// </summary>
    public interface IHouseAppointmentService:IServiceSupport
    {
        //新增一个预约：userId用户id（可以为null，表示匿名用户）
        long AddNew(long? userId, string name, string phoneNum, long houseId, DateTime visitDate);
        bool Follow(long AdminUserId, long houseAppointmentId);
        HouserAppointmentDTO GetById(long id);
        long GetTotalCount(long cityId, string status);
        HouserAppointmentDTO[] GetPagedData(long cityId, string status, int pageSize, int currentIndex);
    }
}
