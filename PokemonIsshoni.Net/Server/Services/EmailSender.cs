//using MailKit.Net.Smtp;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Net.Mail;
//using System.Net.Mail;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Server.Services
{
    public class EmailSender : IEmailSender
    {

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public static int idx = 0;

        string[,] lp = { { "wanghaosheng@linxrobot.com", "c5xH3tym2ReZge7F" } ,
        { "pokemonisshoni@163.com", "AHUTQWYOJDICNPNE" } ,
        { "pokemonchineselink@163.com", "BEMBROJZTXOWXONL" } ,

        };

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }
        public static async Task<bool> SendMailAsync22(string Name, string receive, string sender, string password, string host, int port, string subject, string body)
        {
            try
            {
                //# MimeMessage代表一封电子邮件的对象
                var message = new MimeMessage();
                //# 添加发件人地址 Name 发件人名字 sender 发件人邮箱
                message.From.Add(new MailboxAddress(Name, sender));
                //# 添加收件人地址
                message.To.Add(new MailboxAddress("", receive));
                //# 设置邮件主题信息
                message.Subject = subject;
                //# 设置邮件内容
                var bodyBuilder = new BodyBuilder() { HtmlBody = body };
                message.Body = bodyBuilder.ToMessageBody();
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    // Note: since we don't have an OAuth2 token, disable  
                    // the XOAUTH2 authentication mechanism.  
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.CheckCertificateRevocation = false;
                    //client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    client.Connect(host, port, MailKit.Security.SecureSocketOptions.Auto);
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(sender, password);
                    await client.SendAsync(message);
                    client.Disconnect(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        public Task Execute(string subject, string message, string email)
        {

            //# MimeMessage代表一封电子邮件的对象
            //string Name = "宝可梦 相连";
            string Name = "临流";
            var message1 = new MimeMessage();
            //# 添加发件人地址 Name 发件人名字 sender 发件人邮箱
            //message1.From.Add(new MailboxAddress(Name, "pokemonupdataer@163.com"));
            message1.From.Add(new MailboxAddress(Name, lp[idx, 0]));
            //# 添加收件人地址
            message1.To.Add(new MailboxAddress("", email));
            //# 设置邮件主题信息
            message1.Subject = subject;
            //# 设置邮件内容
            var bodyBuilder = new BodyBuilder() { HtmlBody = message };
            message1.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                // Note: since we don't have an OAuth2 token, disable  
                // the XOAUTH2 authentication mechanism.  
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.CheckCertificateRevocation = false;
                //client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                client.Connect("smtp.exmail.qq.com", 465, MailKit.Security.SecureSocketOptions.Auto);
                // Note: only needed if the SMTP server requires authentication
                //client.Authenticate("pokemonupdataer@163.com", "bksshellyssj3");
                client.Authenticate(lp[idx, 0], lp[idx, 1]);
                //client.Authenticate("pokemonisshoni@163.com", "AHUTQWYOJDICNPNE");
                client.Send(message1);
                client.Disconnect(true);
            }

            //idx = (idx + 1) % 3;
            return Task.CompletedTask;
        }



        //public Task Execute(string subject, string message, string email)
        //{
        //    MailAddress From = new MailAddress("pokemonupdataer@163.com", "宝可梦网"), Target;
        //    string Password;
        //    SmtpClient client;
        //    MailMessage message1 = new MailMessage(From, new MailAddress(email));
        //    message1.Subject = subject; // 设置邮件的标题
        //    message1.Body = message;
        //    message1.IsBodyHtml = true;
        //    message1.BodyEncoding = System.Text.Encoding.Default;

        //    client = new SmtpClient(string.Format("smtp.{0}.com", "163"));
        //    //创建一个SmtpClient 类的新实例,并初始化实例的SMTP 事务的服务器
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.UseDefaultCredentials = false;
        //    client.EnableSsl = true;
        //    client.Port = 587;
        //    //身份认证
        //    client.Credentials = new System.Net.NetworkCredential(From.Address, "bksshellyssj3");
        //    //client.SendAsync(message1, "test message1");
        //    client.Send(message1);
        //    return Task.CompletedTask;
        //    //var msg = new SendGridMessage()
        //    //{
        //    //    From = new EmailAddress("Joe@contoso.com", Options.SendGridUser),
        //    //    Subject = subject,
        //    //    PlainTextContent = message,
        //    //    HtmlContent = message
        //    //};
        //    //msg.AddTo(new EmailAddress(email));

        //    //// Disable click tracking.
        //    //// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        //    //msg.SetClickTracking(false, false);

        //    //return client.SendEmailAsync(msg);
        //}
    }
}
