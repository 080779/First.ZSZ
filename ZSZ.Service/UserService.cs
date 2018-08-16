using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class UserService : IUserService
    {
        public long AddNew(string phoneNum, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<UserEntity> cs = new CommonService<UserEntity>(dbc);
                bool exists= cs.GetAll().Any(u => u.PhoneNum == phoneNum);
                if(exists)
                {
                    throw new ArgumentException("phoneNum=" + phoneNum + "的数据已经存在");
                }
                UserEntity entity = new UserEntity();
                entity.PhoneNum = phoneNum;
                string  salt=CommonHelper.GetCaptcha(5);
                entity.PasswordSalt = salt;
                entity.PasswordHash = CommonHelper.GetMD5(salt + password);
                dbc.Users.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<UserEntity> cs = new CommonService<UserEntity>(dbc);
                var user= cs.GetAll().SingleOrDefault(u => u.PhoneNum == phoneNum);
                if(user==null)
                {
                    return false;
                }
                string salt = user.PasswordSalt;
                string pwdHash = CommonHelper.GetMD5(salt + password);
                return pwdHash == user.PasswordHash;
            }
        }
        private UserDTO ToDTO(UserEntity user)
        {
            UserDTO dto = new UserDTO();
            dto.CityId = user.CityId;
            dto.CreateDateTime = user.CreateDateTime;
            dto.Id = user.Id;
            dto.PasswordHash = user.PasswordHash;
            dto.PasswordSalt = user.PasswordSalt;
            dto.LastLoginErrorDateTime = user.LastLoginErrorDateTime;
            dto.LoginErrorTimes = user.LoginErrorTimes;
            dto.PhoneNum = user.PhoneNum;
            return dto;
        }
        public UserDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<UserEntity> cs = new CommonService<UserEntity>(dbc);
                var user = cs.GetById(id);
                if(user==null)
                {
                    return null;
                }
                return ToDTO(user);
            }
        }

        public UserDTO GetByPhoneNum(string phoneNum)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<UserEntity> cs = new CommonService<UserEntity>(dbc);
                var user = cs.GetAll().Where(u => u.PhoneNum == phoneNum);
                int count = user.Count();
                if (count == 0)
                {
                    return null;
                }
                else if (count == 1)
                {
                    return ToDTO(user.Single());
                }
                else
                {
                    throw new ArgumentException("找到手机号为：" + phoneNum + "的多条数据");
                }
            }
        }

        public void UpdatePassword(long userId, string newPassword)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<UserEntity> cs = new CommonService<UserEntity>(dbc);
                var user= cs.GetById(userId);
                if(user==null)
                {
                    throw new ArgumentException("userId=" + userId + "的数据不存在");
                }
                string salt = CommonHelper.GetCaptcha(5);
                user.PasswordSalt = salt;
                user.PasswordHash = CommonHelper.GetMD5(salt + newPassword);
                dbc.SaveChanges();
            }
        }

        public void SetUserCityId(long userId, long cityId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<UserEntity> cs = new CommonService<UserEntity>(dbc);
                var user = cs.GetAll().SingleOrDefault(u => u.Id == userId);
                if(user==null)
                {
                    throw new ArgumentException("userId=" + userId + "的数据不存在");
                }
                user.CityId = cityId;
                dbc.SaveChanges();
            }
        }
    }
}
