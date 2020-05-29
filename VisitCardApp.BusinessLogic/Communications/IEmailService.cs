namespace VisitCardApp.BusinessLogic.Communications
{
    using System.Threading.Tasks;
    
    public interface IEmailService
    {
        Task SendEmailAsync(Message message);
    }
}
