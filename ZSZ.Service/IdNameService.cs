using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class IdNameService : IIdNameService
    {
        public long AddNew(string typeName, string name,string pngUrl)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                IdNameEntity entity = new IdNameEntity();
                var idName= dbc.IdNames.SingleOrDefault(i => i.TypeName == typeName && i.Name == name);
                if(idName!=null)
                {
                    throw new ArgumentException("typeName="+typeName+",name="+name+"的数据已经存在");
                }
                entity.TypeName = typeName;
                entity.Name = name;
                entity.PngUrl = pngUrl;
                dbc.IdNames.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }
        }

        public IdNameDTO[] GetAll(string typeName)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<IdNameEntity> cs = new CommonService<IdNameEntity>(dbc);
                return cs.GetAll().Where(i => i.TypeName == typeName).Select(i => new IdNameDTO { Id = i.Id,
                    Name = i.Name, TypeName = i.TypeName,PngUrl=i.PngUrl, CreateDateTime = i.CreateDateTime }).ToArray();
            }
        }

        public IdNameDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<IdNameEntity> cs = new CommonService<IdNameEntity>(dbc);
                return cs.GetAll().Select(i => new IdNameDTO { Id = i.Id, Name = i.Name,
                    TypeName = i.TypeName,PngUrl=i.PngUrl, CreateDateTime = i.CreateDateTime }).ToArray();
            }
        }

        public IdNameDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<IdNameEntity> cs = new CommonService<IdNameEntity>(dbc);
                var idName = cs.GetAll().SingleOrDefault(i => i.Id == id);
                if(idName==null)
                {
                    return null;
                }
                return new IdNameDTO { Id = idName.Id, Name = idName.Name,
                    TypeName = idName.TypeName,PngUrl=idName.PngUrl, CreateDateTime = idName.CreateDateTime };
            }
        }
    }
}
