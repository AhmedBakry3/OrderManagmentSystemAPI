


namespace Demo.Presentation.Helper
{
    public class MailService(IOptions<MailSettings> _options) : IMailService
    {
        public async Task SendAsync(Email email)
        {
            var mail = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_options.Value.Email),
                Subject = email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Email));

            var builder = new BodyBuilder { TextBody = email.Body };
            mail.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_options.Value.Email, _options.Value.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }

    }
}
