using GTBack.Core.DTO;
using GTBack.Core.Services;
using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace EmailService
{
    public class MailService : GTBack.Core.Services.IMailService
    {
        public void SendEmail(MailServiceOptions options)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(options.SenderName, options.SenderEmail));
            message.To.Add(new MailboxAddress(options.ReceiverName, options.ReceiverEmail));
            message.Subject = options.Subject;

            var body = new TextPart("html")
            {
                Text = options.Body
            };

            message.Body = body;

            using (var client = new SmtpClient())
            {
                try
                {
                    // Connect to the SMTP server
                    client.Connect(options.SmtpServer, options.SmtpPort, options.UseSsl ? MailKit.Security.SecureSocketOptions.SslOnConnect : MailKit.Security.SecureSocketOptions.StartTls);

                    // Authenticate
                    client.Authenticate(options.SenderEmail, options.SenderPassword);

                    // Send the email
                    client.Send(message);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
                finally
                {
                    // Disconnect from the SMTP server
                    client.Disconnect(true);
                }
            }
        }
    }

  

    
}