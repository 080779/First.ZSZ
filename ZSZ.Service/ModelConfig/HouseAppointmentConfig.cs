using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class HouseAppointmentConfig:EntityTypeConfiguration<HouseAppointmentEntity>
    {
        /// <summary>
        /// 配置预约看房实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public HouseAppointmentConfig()
        {
            ToTable("T_HouseAppointments");
            HasRequired(h => h.User).WithMany().HasForeignKey(u => u.UserId).WillCascadeOnDelete(false);
            HasRequired(h => h.House).WithMany().HasForeignKey(h => h.HouseId).WillCascadeOnDelete(false);
            HasOptional(h => h.FollowAdminUser).WithMany().HasForeignKey(f => f.FollowAdminUserId).WillCascadeOnDelete(false);
            Property(h => h.Name).HasMaxLength(20).IsRequired();
            Property(h => h.PhoneNum).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(h => h.Status).HasMaxLength(20).IsRequired();
            Property(h => h.RowVersion).IsRequired().IsRowVersion();
        }
    }
}
