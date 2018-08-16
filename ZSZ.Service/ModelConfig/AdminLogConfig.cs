using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class AdminLogConfig:EntityTypeConfiguration<AdminLogEntity>
    {
        /// <summary>
        /// 配置日志实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public AdminLogConfig()
        {
            ToTable("T_AdminLogs");
            HasRequired(a => a.AdminUser).WithMany().HasForeignKey(a => a.AdminUserId).WillCascadeOnDelete(false);
            Property(a => a.Message).IsRequired();
        }
    }
}
