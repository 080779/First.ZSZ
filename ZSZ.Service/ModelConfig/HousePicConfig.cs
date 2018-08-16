using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class HousePicConfig:EntityTypeConfiguration<HousePicEntity>
    {
        /// <summary>
        /// 配置房屋图片实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public HousePicConfig()
        {
            ToTable("T_HousePics");
            HasRequired(h => h.House).WithMany(p=>p.HousePics).HasForeignKey(h => h.HouseId).WillCascadeOnDelete(false);
            Property(h => h.Url).HasMaxLength(1024).IsRequired();
            Property(h => h.ThumbUrl).HasMaxLength(1024).IsRequired();
        }
    }
}
