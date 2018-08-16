using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class CommunityConfig:EntityTypeConfiguration<CommunityEntity>
    {
        /// <summary>
        /// 配置小区实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public CommunityConfig()
        {
            ToTable("T_Communities");
            HasRequired(r => r.Region).WithMany().HasForeignKey(r => r.RegionId).WillCascadeOnDelete(false);
            Property(c => c.Name).HasMaxLength(20).IsRequired();
            Property(c => c.Location).HasMaxLength(1024).IsRequired();
            Property(c => c.Traffic).HasMaxLength(200);
        }
    }
}
