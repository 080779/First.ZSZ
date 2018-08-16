using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
    /// <summary>
    /// 区域
    /// </summary>
    public class RegionEntity:BaseEntity
    {
        public string Name { get; set; }
        public long CityId { get; set; }
        public virtual CityEntity City { get; set; }
    }
}
