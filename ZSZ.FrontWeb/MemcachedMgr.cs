using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class MemcachedMgr
    {
        //单例模式
        private MemcachedClient client;
        public static MemcachedMgr Instance { get; private set; } = new MemcachedMgr();
        private MemcachedMgr()
        {
            MemcachedClientConfiguration config = new MemcachedClientConfiguration();
            var settingService=DependencyResolver.Current.GetService<ISettingService>();
            string[] servers=settingService.GetValue("MemcachedServers").Split(';');
            foreach(string server in servers)
            {
                config.Servers.Add(new IPEndPoint(IPAddress.Parse(server), 11211));
            }            
            config.Protocol = MemcachedProtocol.Binary;
            client = new MemcachedClient(config);
        }            
        public void SetValue(string key,object value,TimeSpan expires)
        {
            if(!value.GetType().IsSerializable)
            {
                throw new ArgumentException("value必须是可序列化的对象");
            }
            client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, value,expires);//还可以指定第四个参数指定数据的过期时间。
        }
        public T GetValue<T>(string key)
        {
            return client.Get<T>(key);
        }
    }
}