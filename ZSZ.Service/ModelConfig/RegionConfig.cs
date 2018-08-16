using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class RegionConfig:EntityTypeConfiguration<RegionEntity>
    {
        /// <summary>
        /// 配置区域实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public RegionConfig()
        {
            ToTable("T_Regions");
            HasRequired(r => r.City).WithMany().HasForeignKey(r => r.CityId).WillCascadeOnDelete(false);
            Property(r => r.Name).HasMaxLength(50).IsRequired();
        }
    }
}
