using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class AttachmentConfig:EntityTypeConfiguration<AttachmentEntity>
    {
        /// <summary>
        /// 配置房屋配套设施实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public AttachmentConfig()
        {
            ToTable("T_Attachments");
            HasMany(h => h.Houses).WithMany(a => a.Attachments).Map(m => m.ToTable("T_AttachmentHouses").MapLeftKey("AttachmentId").MapRightKey("HouseId"));
            Property(a => a.Name).HasMaxLength(50).IsRequired();
            Property(a => a.IconName).HasMaxLength(50).IsRequired().IsUnicode(false);
        }
    }
}
