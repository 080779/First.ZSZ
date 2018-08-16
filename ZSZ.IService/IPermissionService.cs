using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    /// 权限管理接口
    /// </summary>
    public interface IPermissionService:IServiceSupport
    {
        long AddNew(string name, string description);
        void UpdatePermission(long id, string name, string description);
        bool MarkDeleted(long id);
        PermissionDTO GetById(long id);
        PermissionDTO[] GetAll();
        PermissionDTO GetByName(string name);
        PermissionDTO[] GetByRoleId(long roleId);
        void AddPermissionIds(long roleId, long[] permissionIds);
        void UpdatePermissionIds(long roleId, long[] permissionIds);
    }
}
