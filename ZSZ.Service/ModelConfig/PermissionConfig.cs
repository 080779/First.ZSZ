using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class PermissionConfig:EntityTypeConfiguration<PermissionEntity>
    {
        /// <summary>
        /// 配置权项实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public PermissionConfig()
        {
            ToTable("T_Permissions");
            Property(p => p.Description).HasMaxLength(1024).IsRequired();
            Property(p => p.Name).HasMaxLength(50).IsRequired();
        }
    }
}
