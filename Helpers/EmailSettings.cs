using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CreditVillageBackend
{
    public interface IEmailSender
    {
        Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message);
    }

    public class SendEmailResponse
    {
        public bool Successful => this.ErrorMsg == null;

        public string ErrorMsg { get; set; }
    }

    public class SendGridEmailSender : IEmailSender
    {
        private readonly AppSettings _appsettings;

        public SendGridEmailSender(IOptions<AppSettings> appsettings)
        {
            _appsettings = appsettings.Value;
        }

        public async Task<SendEmailResponse> SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            var apiKey = _appsettings.SendGridKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("olowofelayinka@gmail.com", "CREDIT VILLAGE");
            var subject = emailSubject;
            var to = new EmailAddress(userEmail, "Email Confirmation");
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return new SendEmailResponse();
        }
    }
}