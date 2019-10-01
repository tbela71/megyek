using System.Threading.Tasks;

namespace Megyek.Models
{
    public interface IMailService
    {
        Task SendParticipatonMailAsync(Participation participation);
        //Task SendInvitationMailAsync(Membership membership);
        Task SendPostMailAsync(Post post);
        Task SendTestMailAsync();
        Task SendAlertMailAsync(Event evnt);
    }
}
