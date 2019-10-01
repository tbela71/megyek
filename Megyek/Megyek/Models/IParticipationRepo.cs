using System.Threading.Tasks;

namespace Megyek.Models
{
    public interface IParticipationRepo
    {
        Task AddParticipationAsync(Participation participation);
        Task SaveParticipationAsync(Participation participation);
    }
}
