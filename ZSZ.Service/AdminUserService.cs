using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class AdminUserService : IAdminUserService
    {
        public long AddAdminUser(string name, string phoneNum, string password, string email, long? cityId)
        {
            AdminUserEntity user = new AdminUserEntity();
            user.Name = name;
            user.PhoneNum = phoneNum;
            user.Email = email;
            user.CityId = cityId;
            string salt = CommonHelper.GetCaptcha(5);
            user.PasswordSalt = salt;
            user.PasswordHash = CommonHelper.GetMD5(salt + password);
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                bool exists = cs.GetAll().Any(a => a.PhoneNum==phoneNum);
                if (exists)
                {
                    throw new ArgumentException("手机号:"+phoneNum+"已经存在");
                }
                else
                {
                    dbc.AdminUsers.Add(user);
                    dbc.SaveChanges();
                    return user.Id;
                }
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().SingleOrDefault(u => u.PhoneNum == phoneNum);
                if (user == null)
                {
                    return false;
                }
                string pwdHash = CommonHelper.GetMD5(user.PasswordSalt + password);
                return pwdHash == user.PasswordHash;
            }
        }
        private AdminUserDTO ToDTO(AdminUserEntity user)
        {
            AdminUserDTO dto = new AdminUserDTO();
            dto.Id = user.Id;
            dto.Name = user.Name;
            dto.PhoneNum = user.PhoneNum;
            dto.CityId = user.CityId;
            if (user.City != null)
            {
                dto.CityName = user.City.Name;
            }
            else
            {
                dto.CityName = "总部";
            }
            dto.CreateDateTime = user.CreateDateTime;
            dto.Email = user.Email;            
            return dto;
        }
        public AdminUserDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                return cs.GetAll().Include(u => u.City).AsNoTracking().ToList().Select(u => ToDTO(u)).ToArray();
            }
        }

        public AdminUserDTO[] GetAll(long? cityId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                return cs.GetAll().Include(u => u.City).AsNoTracking().Where(u=>u.CityId==cityId).
                    ToList().Select(u => ToDTO(u)).ToArray();
            }
        }

        public AdminUserDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().SingleOrDefault(u => u.Id == id);
                if(user==null)
                {
                    throw new ArgumentException("id=" + id + "的数据不存在");
                }
                else
                {
                    return ToDTO(user);
                }
            }
        }

        public AdminUserDTO GetByPhoneNum(string phoneNum)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().Where(u => u.PhoneNum == phoneNum);
                int count = user.Count();
                if (count == 0)
                {
                    return null;
                }
                else if(count==1)
                {
                    return ToDTO(user.Single());
                }
                else
                {
                    throw new ArgumentException("找到手机号为："+phoneNum+"的多条数据");
                }
            }
        }

        public bool HasPermission(long adminUserId, string permissionName)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                //var user = cs.GetById(adminUserId);
                var user = cs.GetAll().Include(u => u.Roles).AsNoTracking().SingleOrDefault(u => u.Id == adminUserId);
                if (user == null)
                {
                    throw new ArgumentException("找不到adminUserId为：" + adminUserId + "的数据");
                }
                return user.Roles.SelectMany(r => r.Permissions).Any(p => p.Name == permissionName);
            }
        }
        public bool MarkDeleted(long adminUserId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                if(!cs.GetAll().Any(u=>u.Id==adminUserId))
                {
                    return false;
                }
                cs.MarkDeleted(adminUserId);
                return true;             
            }
        }

        public void RecordLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void ResetLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdminUser(long id, string name, string email, long? cityId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().SingleOrDefault(u => u.Id == id);
                if(user==null)
                {
                    throw new ArgumentException("找不到id=" + id + "的管理员用户");
                }
                user.Name = name;
                user.Email = email;
                user.CityId = cityId;                
                dbc.SaveChanges();
            }
        }

        public bool UpdatePassword(long id, string Password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user=cs.GetAll().SingleOrDefault(u => u.Id == id);
                if (user==null)
                {
                    return false;
                }
                user.PasswordHash = CommonHelper.GetMD5(user.PasswordSalt + Password);
                dbc.SaveChanges();
                return true;
            }
        }
    }
}
