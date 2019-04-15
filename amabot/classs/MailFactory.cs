using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace amabot.classs
{
    class MailFactory
    {

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="account">帐户</param>
        /// <param name="password">密码</param>
        /// <param name="reciver">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="content">内容</param>
        public static void Send(string account,string password,string smtp,int port,string reciver,string subject,string content) { 
        
         
            MailMessage message = new MailMessage();

            MailAddress fromAddr = new MailAddress(account);
            message.From = fromAddr;
            message.To.Add(reciver);
            message.Subject = subject;
            message.Body = content;

            SmtpClient client = new SmtpClient(smtp, port);

            client.Credentials = new NetworkCredential(account,password);
            client.EnableSsl = true;

            client.Send(message);
        
        }
    }
}
