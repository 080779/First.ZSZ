using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class CityConfig:EntityTypeConfiguration<CityEntity>
    {
        /// <summary>
        /// 配置城市实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public CityConfig()
        {
            ToTable("T_Cities");
            Property(c => c.Name).HasMaxLength(50).IsRequired();
        }
    }
}
