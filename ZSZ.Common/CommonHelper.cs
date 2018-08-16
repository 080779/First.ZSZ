using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Common
{
    public static class CommonHelper
    {
        public static string GetMD5(this string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return GetMD5(bytes);
        }
        public static string GetMD5(byte[] bytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(bytes);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" +
                   computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }
        public static string GetMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(stream);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" +
                   computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }
        public static string GetCaptcha(int length)
        {
            char[] data = { 'a', 'c', 'd', 'e', 'f', 'g', 'k', 'm', 'p', 'r', 's', 't', 'w', 'x', 'y', '3', '4', '5', '7', '8' };
            StringBuilder sbCode = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                char ch = data[rand.Next(data.Length)];
                sbCode.Append(ch);
            }
            return sbCode.ToString();
        }
        public static void SendEmail(string[] receiveAddress,Dictionary<string,string> dicts)
        {
            using (MailMessage mailMessage = new MailMessage())
            using (SmtpClient smtpClient = new SmtpClient(dicts["SMTP"]))
            {
                for(int i=0;i< receiveAddress.Length;i++)
                {
                    mailMessage.To.Add(receiveAddress[i]);
                }                
                mailMessage.Body = dicts["MaliBody"];
                mailMessage.From = new MailAddress(dicts["SendAddress"]);
                mailMessage.Subject = dicts["MailTitle"];
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(dicts["SendAddress"], dicts["Password"]);//如果启用了“客户端授权码”，要用授权码代替密码
                smtpClient.Send(mailMessage);
            }
        }
    }
}
