using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    ///房屋配置设施管理接口
    /// </summary>
    public interface IAttachmentService:IServiceSupport
    {
        AttachmentDTO[] GetAll();
        AttachmentDTO[] GetAttachments(long houseId);
    }
}
