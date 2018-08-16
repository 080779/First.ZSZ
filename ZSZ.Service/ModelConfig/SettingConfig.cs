using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class SettingConfig:EntityTypeConfiguration<SettingEntity>
    {
        /// <summary>
        /// 配置通用配置实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public SettingConfig()
        {
            ToTable("T_Settings");
            Property(s => s.Name).HasMaxLength(1024).IsRequired();
            Property(s => s.Value).IsRequired();
        }                
    }
}
