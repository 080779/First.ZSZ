using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class HouseConfig:EntityTypeConfiguration<HouseEntity>
    {
        /// <summary>
        /// 配置房屋实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public HouseConfig()
        {
            ToTable("T_Houses");
            HasRequired(h => h.Community).WithMany().HasForeignKey(h => h.CommunityId).WillCascadeOnDelete(false);
            HasRequired(h => h.RoomType).WithMany().HasForeignKey(h => h.RoomTypeId).WillCascadeOnDelete(false);
            HasRequired(h => h.Status).WithMany().HasForeignKey(h => h.StatusId).WillCascadeOnDelete(false);
            HasRequired(h => h.Type).WithMany().HasForeignKey(h => h.TypeId).WillCascadeOnDelete(false);
            HasRequired(h => h.DecorateStatus).WithMany().HasForeignKey(h => h.DecorateStatusId).WillCascadeOnDelete(false);
            Property(h => h.Address).HasMaxLength(128).IsRequired();
            Property(h => h.Direction).HasMaxLength(20).IsRequired();
            Property(h => h.OwnerName).HasMaxLength(30).IsRequired();
            Property(h => h.OwnerPhoneNum).HasMaxLength(20).IsRequired().IsUnicode(false);
        }
    }
}
