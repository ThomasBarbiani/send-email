namespace BACK_SENDEMAIL.Interfaces
{
    public interface IEmailService
    {
        string SendEmail(IEmailBody request);
    }
}
