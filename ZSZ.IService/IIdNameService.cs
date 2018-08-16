using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    /// 数据字典接口
    /// </summary>
    public interface IIdNameService:IServiceSupport
    {
        long AddNew(string typeName, string name,string pngUrl);
        IdNameDTO GetById(long id);
        IdNameDTO[] GetAll(string typeName);
        IdNameDTO[] GetAll();
    }
}
