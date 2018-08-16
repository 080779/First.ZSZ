using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class RegionDTO:BaseDTO
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
        public string Name { get; set; }
    }
}
