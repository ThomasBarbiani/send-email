using BACK_SENDEMAIL.Interfaces;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace BACK_SENDEMAIL.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _config;
        private readonly SmtpClient _smtpClient;

        public EmailService(IConfiguration config)
        {
            _config = config;
            _smtpClient = new SmtpClient();
            ConfigureSmtpClient();
        }

        private void ConfigureSmtpClient()
        {
            _smtpClient.Connect(
                _config.GetSection("EmailHost").Value,
                465,
                SecureSocketOptions.SslOnConnect);

            _smtpClient.Authenticate(
                _config.GetSection("EmailUsername").Value,
                _config.GetSection("EmailPassword").Value);
        }

        public string SendEmail(IEmailBody request) {

            try
            {
                if (request.File != null && request.File.ContentType == "application/pdf")
                {
                    var email = new MimeMessage();

                    email.From.Add
                    (
                        MailboxAddress.Parse(_config.GetSection("EmailUsername").Value)
                    );
                    email.To.Add
                    (
                        MailboxAddress.Parse(_config.GetSection("EmailUsername").Value)
                    );
                    email.Subject = $"Email de {request.Nome}";

                    var textPart = new TextPart(TextFormat.Plain)
                    {
                        Text = $"Nome: {request.Nome}\nEmail: {request.Email}"
                    };

                    var filePart = new MimePart()
                    {
                        Content = new MimeContent(request.File.OpenReadStream()),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = request.File.FileName
                    };

                    var multipart = new Multipart("mixed");
                    multipart.Add(textPart);
                    multipart.Add(filePart);

                    // Adiciona as partes à mensagem
                    email.Body = multipart;
                    _smtpClient.Send(email);
                    _smtpClient.Disconnect(true);

                    return null;
                }
                else
                {
                    return "Tipo de arquivo não permitido.\nApenas arquivos PDF são permitidos.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _smtpClient.Disconnect(true);
                return "Erro ao enviar o e-mail: " + ex.Message;
            }
        }
    }
}
