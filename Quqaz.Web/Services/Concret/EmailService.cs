using Quqaz.Web.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Quqaz.Web.Services.Concret
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsHtml(string to, string from, string password, string subject, string message)
        {

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress(from);
            mail.To.Add(to);

            //set the content 
            mail.IsBodyHtml= true;
            mail.Subject = subject;
            mail.Body = message;
            //send the message 
            SmtpClient smtp = new SmtpClient("mail.quqaz.com");

            //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
            NetworkCredential Credentials = new NetworkCredential(from, password);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = Credentials;
            smtp.Port = 8889;    //alternative port number is 8889
            smtp.EnableSsl = false;
            await smtp.SendMailAsync(mail);
        }

        public async Task SendEmailAsHtml(string to, string from, string password, string subject, string message, IFormFile attachment)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            // Create the mail message
            MailMessage mail = new MailMessage();

            // Set the addresses
            mail.From = new MailAddress(from);
            mail.To.Add(to);

            // Set the content
            mail.IsBodyHtml = true;
            mail.Subject = subject;
            mail.Body = message;

            // Attach the file
            if (attachment != null && attachment.Length > 0)
            {
                using var ms = new MemoryStream();
                await attachment.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                Attachment att = new Attachment(new MemoryStream(fileBytes), attachment.FileName);
                mail.Attachments.Add(att);
            }

            // Send the message
            SmtpClient smtp = new SmtpClient("mail.quqaz.com");
            NetworkCredential credentials = new NetworkCredential(from, password);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credentials;
            smtp.Port = 8889; // Alternative port number is 8889
            smtp.EnableSsl = false;

            await smtp.SendMailAsync(mail);
        }

    }
}
