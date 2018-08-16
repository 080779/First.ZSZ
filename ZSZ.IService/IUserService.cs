using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    /// 用户管理服务接口
    /// </summary>
    public interface IUserService:IServiceSupport
    {
        long AddNew(string phoneNum, string password);
        UserDTO GetById(long id);
        UserDTO GetByPhoneNum(string phoneNum);
        bool CheckLogin(string phoneNum, string password);
        void UpdatePassword(long userId, string newPassword);
        void SetUserCityId(long userId, long cityId);
    }
}
