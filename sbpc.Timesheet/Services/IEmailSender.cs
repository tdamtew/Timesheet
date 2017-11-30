
using System.Threading.Tasks;

namespace sbpc.Timesheet.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
