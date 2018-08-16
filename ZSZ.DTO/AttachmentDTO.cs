using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{

    [Serializable]
    public class AttachmentDTO:BaseDTO
    {
        public string Name { get; set; }
        public string IconName { get; set; }
    }
}
