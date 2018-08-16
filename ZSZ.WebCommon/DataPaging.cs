using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.WebCommon
{
    public class DataPaging
    {
        /// <summary>
        /// 每页数据条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总数据条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 当前页码（从 1 开始）
        /// </summary>
        public int PageIndex { get; set; }
        public string UrlPattern { get; set; } 
        // "/Role/List?pageIndex={pn}"
        /// <summary>
        /// 最多的页码数
        /// </summary>
        public int MaxPagerCount { get; set; }
        public string CurrentLinkClassName { get; set; }
        public DataPaging()
        {
            this.PageSize = 10;
            this.MaxPagerCount = 10;
        }
        public string GetPager()
        {
            StringBuilder sb = new StringBuilder();
            //算出来的页数
            int pageCount = (int)Math.Ceiling(TotalCount * 1.0f / PageSize);
            int startPageIndex = Math.Max(1, PageIndex - MaxPagerCount / 2);//第一个页码
            int endPageIndex = Math.Min(pageCount, startPageIndex + MaxPagerCount - 1);//最后一个页码
            sb.AppendLine("<ul>");
            for (int i = startPageIndex; i <= endPageIndex; i++)
            {
                if (i == PageIndex)
                {
                    sb.AppendLine("<li class='" + CurrentLinkClassName + "'>" + i + "</li>");
                }
                else
                {
                    sb.AppendLine("<li><a href='" + UrlPattern.Replace("{pn}", i.ToString()) + "'>" + i +
                   "</a></li>");
                }
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }
    }
}
