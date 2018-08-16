using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class UserConfig:EntityTypeConfiguration<UserEntity>
    {
        /// <summary>
        /// 配置用户实体类对应的数据库表、关联表结构、字段属性
        /// </summary>
        public UserConfig()
        {
            ToTable("T_Users");
            HasOptional(u => u.City).WithMany().HasForeignKey(u => u.CityId).WillCascadeOnDelete(false);
            Property(u => u.PhoneNum).HasMaxLength(20).IsRequired().IsUnicode(false);
            Property(u => u.PasswordSalt).HasMaxLength(20).IsRequired();
            Property(u => u.PasswordHash).HasMaxLength(100).IsRequired().IsUnicode(false);
        }
    }
}
