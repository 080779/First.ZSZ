using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    public class AttachmentService : IAttachmentService
    {
        private AttachmentDTO ToDTO(AttachmentEntity am)
        {
            AttachmentDTO dto = new AttachmentDTO();
            dto.Id = am.Id;
            dto.Name = am.Name;
            dto.IconName = am.IconName;
            dto.CreateDateTime = am.CreateDateTime;
            return dto;
        }
        public AttachmentDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<AttachmentEntity> cs = new CommonService<AttachmentEntity>(dbc);
                return cs.GetAll().AsNoTracking().ToList().Select(a => ToDTO(a)).ToArray();
            }
        }

        public AttachmentDTO[] GetAttachments(long houseId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<HouseEntity> cs = new CommonService<HouseEntity>(dbc);
                var house = cs.GetAll().Include(a => a.Attachments).AsNoTracking().SingleOrDefault(h => h.Id == houseId);
                if(house==null)
                {
                    throw new ArgumentException("houseId="+houseId+"的数据不存在");
                }
                return house.Attachments.ToList().Select(a =>ToDTO(a)).ToArray();
            }
        }
    }
}
