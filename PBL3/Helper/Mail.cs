using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PBL3.Helper
{
    public static class Mail
    {
        public static async Task<bool> SendMail(string toEmailAddress, string subject, string body)
        {
            try
            {
                string fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
                string fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
                string fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
                string smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
                bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

                // MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(fromEmailAddress, fromEmailDisplayName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(new MailAddress(toEmailAddress));

                SmtpClient client = new SmtpClient
                {
                    Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword),
                    Host = smtpHost,
                    EnableSsl = enabledSsl,
                    Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0
                };
                await client.SendMailAsync(message);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public static string GetMailContent(string[] content)
        {
            StringBuilder html = new StringBuilder("<p>Bạn cần xác nhận tài khoản của mình</p>\n");
            html.Append($"<p>Mở Ứng dụng và nhập mã xác nhận: <b>{content[0]}</b></p>" + "\n");
            return html.ToString();
        }
    }
}