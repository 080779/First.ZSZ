using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.WebCommon
{
    /// <summary>
    /// json返回的模型类
    /// </summary>
    public class AjaxResult
    {
        public string Status { get; set; }
        public string ErrorMsg { get; set; }
        public object Data { get; set; }
    }
}
