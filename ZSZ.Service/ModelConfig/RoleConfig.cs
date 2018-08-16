using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class RoleConfig:EntityTypeConfiguration<RoleEntity>
    {
        /// <summary>
        /// 配置角色实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public RoleConfig()
        {
            ToTable("T_Roles");
            HasMany(r => r.Permissions).WithMany(p => p.Roles).
                Map(m => m.ToTable("T_RolePermissions").MapLeftKey("RoleId").MapRightKey("PermissionId"));
            Property(r => r.Name).HasMaxLength(50).IsRequired();
        }
    }
}
