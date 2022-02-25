using MailKit.Net.Smtp;
using MimeKit;
using MMNVS.Model;

namespace MMNVS.Services
{
    public class MailService : IMailService
    {
		private readonly IDbService _dbService;

        public MailService(IDbService dbService)
        {
            _dbService = dbService;
        }

        public void SendMail(string subject, string body)
        {
			AppSettings settings = _dbService.GetSettingsWithoutInclude();

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("MMNVS - " + settings.Name, settings.SmtpUser));
			message.To.Add(new MailboxAddress("MMNVS - administrator", settings.AdministratorEmail));
			message.Subject = subject;

			message.Body = new TextPart("plain")
			{
				Text = body + @"
				-- Tato zpráva byla automaticky vygenerována systémem MMNVS (Monitoring a management napájení virtuálních serverů)"
			};

			using (var client = new SmtpClient())
			{
				client.Connect(settings.SmtpServer, settings.SmtpPort ?? 25, settings.SmtpIsSecure);

				// Note: only needed if the SMTP server requires authentication
				client.Authenticate(settings.SmtpUser, settings.SmtpPassword);

				client.Send(message);
				client.Disconnect(true);
			}

		}
	}
    public interface IMailService
    {
        void SendMail(string subject, string body);
    }
}
