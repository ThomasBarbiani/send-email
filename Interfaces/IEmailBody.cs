namespace BACK_SENDEMAIL.Interfaces
{
    public class IEmailBody
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public IFormFile File { get; set; }
    }
}
