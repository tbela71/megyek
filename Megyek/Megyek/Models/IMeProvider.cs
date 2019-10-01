using System.Threading.Tasks;

namespace Megyek.Models
{
    public interface IMeProvider
    {
        Person Me { get; }

        Team GetManagedTeam(int? teamId);
        Team GetTeam(int? teamId);
        Task<bool> IsMemberOfTeamAsync(int? teamId);
        bool IsMemberOfTeam(int? teamId);
    }
}
