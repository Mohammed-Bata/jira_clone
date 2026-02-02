using Application.Interfaces;
using Domain;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailService:IEmailService
    {
        public async Task SendEmail(string recipient,string link)
        {
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse("mohammedmu7.20@gmail.com"),
            };

            email.To.Add(MailboxAddress.Parse(recipient));
            email.From.Add(MailboxAddress.Parse("mohammedmu7.20@gmail.com"));

            email.Subject = "Project Invitation";


            email.Body = new TextPart("html")
            {
                Text = $@"
                    <p>You have been invited to a project.</p>
                    <p>
                        <a href='{link}' 
                           style='
                              display: inline-block;
                              padding: 10px 20px;
                              font-size: 16px;
                              color: white;
                              background-color: #1868DB;
                              text-decoration: none;
                              border-radius: 5px;
                           '>
                           Accept Invitation
                        </a>
                    </p>
                "
            };

            using var Smtp = new SmtpClient();
            await Smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            // Note: Ethereal is a fake SMTP service for testing purposes.
            await Smtp.AuthenticateAsync("mohammedmu7.20@gmail.com", "gaem kllf vser dnmn");
            await Smtp.SendAsync(email);
            await Smtp.DisconnectAsync(true);
        }
    }
}
