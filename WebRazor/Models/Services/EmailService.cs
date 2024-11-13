using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using WebRazor.Models.SystemModels;

public class EmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendVerificationEmail(string recipientEmail, string verificationLink)
    {
        var subject = "Xác thực tài khoản của bạn";
        var body = $"Vui lòng xác thực email của bạn bằng cách nhấp vào liên kết sau: <a href='{verificationLink}'>Xác thực email</a>";

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.SenderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(recipientEmail);

        using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}