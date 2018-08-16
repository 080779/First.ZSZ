using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ZSZ.WebCommon
{
    public class TrimToDBCModelBinder:DefaultModelBinder
    {
        //在mvc中ModelBinder负责把表单中的东西赋值到model上
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {/*
           object value = base.BindModel(controllerContext, bindingContext);
           if(value is string)
            {
                string strValue = (string)value;
                return ToDBC(strValue).Trim();
            }
            return value;*/
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if(valueResult==null)
            {
                //如果值为null，不知道怎么处理，让父类去处理
                return base.BindModel(controllerContext, bindingContext);
            }
            string rawValue = valueResult.AttemptedValue;
            if(rawValue==null)
            {
                return null;
            }
            else
            {
                string value = ToDBC(rawValue.Trim());
                object finalValue = Convert.ChangeType(value, bindingContext.ModelType);
                return finalValue;
            }
        }
        /// <summary>
        /// 中文输入法全角转半角
        /// </summary>
        private static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }
    }
}
