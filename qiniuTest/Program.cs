using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Util;
using Qiniu.IO.Model;
using Qiniu.IO;

namespace qiniuTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Mac mac = new Mac("2hrivlc6eBhOJdE4wd-n0oXlg_m6Bz5pG-PJW4lB", "q9tYfpo4JkpqYQfW5FiY1okFHeuGT7ylMkcNND_U");            
            string bucket = "exceed295";
            string saveKey = "a.jpg";
            string localFile = "D:\\1.png";
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            Qiniu.Common.Config.AutoZone("2hrivlc6eBhOJdE4wd-n0oXlg_m6Bz5pG-PJW4lB", bucket, true);
            PutPolicy putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;
            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            string jstr = putPolicy.ToJsonString();
            string token = Auth.CreateUploadToken(mac, jstr);
            UploadManager um = new UploadManager();
            HttpResult result = um.UploadFile(localFile, saveKey, token);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
