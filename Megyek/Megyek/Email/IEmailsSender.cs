using System.Collections.Generic;
using System.Threading.Tasks;

namespace Megyek.Email
{
    public interface IEmailsSender
    {
        Task SendEmailsAsync(List<string> emails, string subject, string message);
    }
}
