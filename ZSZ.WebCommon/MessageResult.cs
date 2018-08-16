using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.WebCommon
{
    /// <summary>
    /// 短信验证返回结果类
    /// </summary>
    public  class MessageResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public string phoneNum { get; set; }
    }
}
