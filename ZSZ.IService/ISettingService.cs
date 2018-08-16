using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    /// <summary>
    /// 配置管理接口
    /// </summary>
    public interface ISettingService:IServiceSupport
    {
        //设置配置项name的值我value
        void SetValue(string name, string value);
        string GetValue(string name);//获取配置项name的值
        void SetIntValue(string name, int value);
        int? GetIntValue(string name);
        void SetBoolValue(string name, bool value);
        bool? GetBoolValue(string name);
        SettingDTO[] GetAll();
    }
}
