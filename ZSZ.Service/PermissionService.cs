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
    public class PermissionService : IPermissionService
    {
        public long AddNew(string name,string description)
        {
            PermissionEntity entity = new PermissionEntity();
            entity.Name = name;
            entity.Description = description;
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);                
                if(cs.GetAll().Any(p=>p.Name==name))
                {
                    throw new ArgumentException("name="+name+"的数据已经存在");
                }
                dbc.Permissions.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }
        }
        public void AddPermissionIds(long roleId, long[] permissionIds)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RoleEntity> roleCs = new CommonService<RoleEntity>(dbc);
                var role = roleCs.GetAll().Include(r => r.Permissions).SingleOrDefault(r => r.Id == roleId);
                if (role == null)
                {
                    throw new ArgumentException("roleId=" + roleId + "的数据不存在");
                }
                role.Permissions.Clear();
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                var pms=cs.GetAll().Where(p => permissionIds.Contains(p.Id)).ToArray();
                foreach(var pm  in  pms)
                {
                    role.Permissions.Add(pm);
                }
                dbc.SaveChanges();
            }
        }

        public PermissionDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                return cs.GetAll().Select(p => new PermissionDTO { Id = p.Id, Name = p.Name,
                    Description = p.Description, CreateDateTime = p.CreateDateTime }).ToArray();
            }
        }
        public PermissionDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                var pm = cs.GetAll().SingleOrDefault(p => p.Id == id);
                if(pm==null)
                {
                    return null;
                }
                return new PermissionDTO { Id = pm.Id, Name = pm.Name, Description = pm.Description, CreateDateTime = pm.CreateDateTime };
            }
        }

        public PermissionDTO GetByName(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                var pm = cs.GetAll().SingleOrDefault(p => p.Name == name);
                if (pm == null)
                {
                    return null;
                }
                return new PermissionDTO { Id = pm.Id, Name = pm.Name, Description = pm.Description, CreateDateTime = pm.CreateDateTime };
            }
        }

        public PermissionDTO[] GetByRoleId(long roleId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<RoleEntity> roleCs = new CommonService<RoleEntity>(dbc);
                var role=roleCs.GetAll().Include(r=>r.Permissions).SingleOrDefault(r => r.Id == roleId);
                if(role==null)
                {
                    return null;
                }
                return role.Permissions.Select(p => new PermissionDTO { Id = p.Id, Name = p.Name,
                    Description = p.Description, CreateDateTime = p.CreateDateTime }).ToArray();
            }
        }

        public bool MarkDeleted(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                if(!cs.GetAll().Any(p => p.Id == id))
                {
                    return false;
                }
                cs.MarkDeleted(id);
                return true;
            }
        }

        public void UpdatePermission(long id, string name, string description)
        {            
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                var pm = cs.GetAll().SingleOrDefault(p => p.Id == id);
                if (pm==null)
                {
                    throw new ArgumentException("id=" + id + "的数据不存在");
                }
                pm.Name = name;
                pm.Description = description;
                dbc.SaveChanges();
            }
        }

        public void UpdatePermissionIds(long roleId, long[] permissionIds)
        {
            using(MyDbContext dbc=new MyDbContext())
            {
                CommonService<RoleEntity> roleCs = new CommonService<RoleEntity>(dbc);
                var role= roleCs.GetAll().Include(r => r.Permissions).SingleOrDefault(r => r.Id == roleId);
                if(role==null)
                {
                    throw new ArgumentException("roleId=" + roleId + "的数据不存在");
                }
                role.Permissions.Clear();
                CommonService<PermissionEntity> cs = new CommonService<PermissionEntity>(dbc);
                var pms=cs.GetAll().Where(p => permissionIds.Contains(p.Id)).ToArray();
                foreach(var pm  in  pms)
                {
                    role.Permissions.Add(pm);
                }
                dbc.SaveChanges();
            }
        }
    }
}
