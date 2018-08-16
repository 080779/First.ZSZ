using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
    /// <summary>
    /// 房屋预约类
    /// </summary>
    public class HouseAppointmentEntity:BaseEntity
    {
        public long? UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        public DateTime VisitDate { get; set; }
        public long HouseId { get; set; }
        public virtual HouseEntity House { get; set; }
        public string Status { get; set; }
        public long? FollowAdminUserId { get; set; }
        public AdminUserEntity FollowAdminUser { get; set; }
        public DateTime? FollowDateTime { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
