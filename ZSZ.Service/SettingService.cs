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
    class SettingService : ISettingService
    {
        public SettingDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                List<SettingDTO> list = new List<SettingDTO>();
                foreach (var setting in cs.GetAll())
                {
                    SettingDTO dto = new SettingDTO();
                    dto.CreateDateTime = setting.CreateDateTime;
                    dto.Id = setting.Id;
                    dto.Name = setting.Name;
                    dto.Value = setting.Value;                    
                    list.Add(dto);
                }
                return list.ToArray();
                //下面是用lambda表达式的写法
                //return cs.GetAll().Select(e => new SettingDTO {Id=e.Id,CreateDateTime=e.CreateDateTime, Name = e.Name, Value = e.Value }).ToArray();
            }
        }

        public bool? GetBoolValue(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                var setting = cs.GetAll().SingleOrDefault(s => s.Name == name);
                if (setting == null)
                {
                    return null;
                }
                else
                {
                    return Convert.ToBoolean(setting.Value);
                }
            }
        }
        public int? GetIntValue(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                var setting = cs.GetAll().SingleOrDefault(s => s.Name == name);
                if (setting == null)
                {
                    return null;
                }
                else
                {
                    return Convert.ToInt32(setting.Value);
                }
            }
        }

        public string GetValue(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                var setting = cs.GetAll().SingleOrDefault(s => s.Name == name);
                if (setting == null)
                {
                    return null;
                }
                else
                {
                    return setting.Value;
                }
            }
        }

        public void SetBoolValue(string name, bool value)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                var setting = cs.GetAll().SingleOrDefault(e => e.Name == name);
                if (setting == null)
                {
                    dbc.Settings.Add(new SettingEntity { Name = name, Value = value.ToString()});
                }
                else
                {
                    setting.Value = value.ToString();
                    dbc.Settings.Add(setting);
                }
                dbc.SaveChanges();
            }
        }

        public void SetIntValue(string name, int value)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                var setting = cs.GetAll().SingleOrDefault(e => e.Name == name);
                if (setting == null)
                {
                    dbc.Settings.Add(new SettingEntity { Name = name, Value = value.ToString() });
                }
                else
                {
                    setting.Value = value.ToString();
                    dbc.Settings.Add(setting);
                }
                dbc.SaveChanges();
            }
        }

        public void SetValue(string name, string value)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<SettingEntity> cs = new CommonService<SettingEntity>(dbc);
                var setting = cs.GetAll().SingleOrDefault(e => e.Name == name);
                if (setting == null)
                {
                    dbc.Settings.Add(new SettingEntity { Name = name,Value=value});
                }
                else
                {
                    setting.Value = value;
                    dbc.Settings.Add(setting);
                }
                dbc.SaveChanges();
            }
        }
    }
}
