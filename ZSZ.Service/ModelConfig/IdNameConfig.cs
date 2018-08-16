using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class IdNameConfig:EntityTypeConfiguration<IdNameEntity>
    {
        /// <summary>
        /// 配置数据字典实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public IdNameConfig()
        {
            ToTable("T_IdNames");
            Property(i => i.TypeName).HasMaxLength(1024).IsRequired();
            Property(i => i.Name).HasMaxLength(1024).IsRequired();
            Property(i => i.PngUrl).HasMaxLength(50);
        }
    }
}
