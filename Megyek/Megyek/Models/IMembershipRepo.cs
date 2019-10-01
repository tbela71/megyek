using System.Collections.Generic;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public interface IMembershipRepo
    {
        IList<Membership> GetMembershipList(int? teamId);
        Membership GetManagedMembership(int? teamId, int? personId);
        Task<Membership> GetMembershipAsync(int? teamId, int? personId);
        Task DeleteMembershipAsync(int? teamId, int? personId);
        Task SaveMembershipAsync(Membership membership);
        Task CreateMembershipAsync(Membership membership);
        Task SetMailAsync(Membership membership);
    }
}
