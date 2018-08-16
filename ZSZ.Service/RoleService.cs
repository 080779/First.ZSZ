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
    public class RoleService : IRoleService
    {
        public long AddNew(string roleName)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RoleEntity> cs = new CommonService<RoleEntity>(dbc);
                bool exists= cs.GetAll().Any(r => r.Name == roleName);                    
                if(exists)
                {
                    throw new ArgumentException("roleName=" + roleName + "的数据已经存在");
                }
                RoleEntity entity = new RoleEntity();
                entity.Name = roleName;
                dbc.Roles.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }
        }

        public void AddRoleIds(long adminUserId, long[] roleIds)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                //不能有.AsNoTracking()，加了的数据不能修改
                var user = cs.GetAll().Include(u=>u.Roles).SingleOrDefault(u => u.Id == adminUserId);
                //var user = cs.GetById(adminUserId);
                if (user == null)
                {
                    throw new ArgumentException("adminUserId=" + adminUserId + "的数据已经存在");
                }
                CommonService<RoleEntity> roleCs = new CommonService<RoleEntity>(dbc);
                var roles = roleCs.GetAll().Where(r => roleIds.Contains(r.Id)).ToArray();
                foreach (var role in roles)
                {
                    user.Roles.Add(role);
                }
                dbc.SaveChanges();
            }
        }

        public RoleDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RoleEntity> cs = new CommonService<RoleEntity>(dbc);
                return cs.GetAll().Select(r => new RoleDTO { Id = r.Id, Name = r.Name, CreateDateTime = r.CreateDateTime }).ToArray();
            }
        }

        public RoleDTO[] GetByAdminUserId(long adminUserId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().Include(u => u.Roles).AsNoTracking().SingleOrDefault(u => u.Id == adminUserId);
                if(user==null)
                {
                    throw new ArgumentException("adminUserId=" + adminUserId + "的数据不存在");
                }
                return user.Roles.Select(r => new RoleDTO { Id = r.Id, Name = r.Name, CreateDateTime = r.CreateDateTime }).ToArray();
            }
        }

        public RoleDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RoleEntity> cs = new CommonService<RoleEntity>(dbc);
                var role = cs.GetAll().SingleOrDefault(r => r.Id == id);
                if(role==null)
                {
                    return null;
                }
                return new RoleDTO { Id = role.Id, Name = role.Name, CreateDateTime = role.CreateDateTime };
            }
        }

        public RoleDTO GetByName(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RoleEntity> cs = new CommonService<RoleEntity>(dbc);
                var role = cs.GetAll().SingleOrDefault(r => r.Name == name);
                if (role == null)
                {
                    return null;
                }
                return new RoleDTO { Id = role.Id, Name = role.Name, CreateDateTime = role.CreateDateTime };
            }
        }

        public bool MarkDeleted(long roleId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RoleEntity> cs = new CommonService<RoleEntity>(dbc);
                var role = cs.GetAll().SingleOrDefault(r => r.Id == roleId);
                if(role==null)
                {
                    return false;
                }
                cs.MarkDeleted(roleId);
                return true;
            }
        }

        public void Update(long roleId, string roleName)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RoleEntity> cs = new CommonService<RoleEntity>(dbc);
                var role = cs.GetAll().SingleOrDefault(r => r.Id == roleId);
                if (role == null)
                {
                    throw new ArgumentException("roleId=" + roleId + "的数据不存在");
                }
                if (cs.GetAll().Any(r => r.Name == roleName && r.Id == roleId))
                {
                    throw new ArgumentException("roleName=" + roleName + "的数据已经存在");
                }
                role.Name = roleName;
                dbc.SaveChanges();
            }
        }

        public void UpdateRoleIds(long adminUserId, long[] roleIds)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AdminUserEntity> cs = new CommonService<AdminUserEntity>(dbc);
                var user = cs.GetAll().Include(u => u.Roles).SingleOrDefault(u => u.Id == adminUserId);
                if(user==null)
                {
                    throw new ArgumentException("adminUserId=" + adminUserId + "的数据不存在");
                }
                user.Roles.Clear();
                CommonService<RoleEntity> roleCs = new CommonService<RoleEntity>(dbc);
                var roles = roleCs.GetAll().Where(r => roleIds.Contains(r.Id));
                foreach (var role in roles)
                {
                    user.Roles.Add(role);
                }
                dbc.SaveChanges();
            }
        }
    }
}
