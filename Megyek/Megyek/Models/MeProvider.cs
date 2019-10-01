using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public class MeProvider : IMeProvider
    {
        private readonly IHttpContextAccessor HthpContextAccessor;
        private MegyekDbContext dbContext;

        public MeProvider(MegyekDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.HthpContextAccessor = httpContextAccessor;
        }

        public string ContextUserName
        {
            get
            {
                return HthpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value ?? "?";
            }
        }

        public Person Me
        {
            get { return dbContext.Person.FirstOrDefault(x => x.UserName == ContextUserName) ?? new Models.Person() { UserName = ContextUserName }; }
        }

        public Team GetManagedTeam(int? teamId)
        {
            return Me?.Membership.FirstOrDefault(x => x.TeamId == teamId && x.Manager)?.Team
                    ?? Me?.Membership.FirstOrDefault(x => x.Manager)?.Team
                    ?? new Models.Team() { Name = "You have no team to manage" };
        }

        public Team GetTeam(int? teamId)
        {
            return Me?.Membership.FirstOrDefault(x => x.TeamId == teamId)?.Team
                    ?? Me?.Membership.FirstOrDefault()?.Team
                    ?? new Models.Team() { Name = "You have no team" };
        }

        public bool IsMemberOfTeam(int? teamId)
        {
            Person me = Me;
            return (dbContext.Membership.FirstOrDefault(x => x.PersonId == me.Id && x.TeamId == teamId)) != null;
        }

        public async Task<bool> IsMemberOfTeamAsync(int? teamId)
        {
            Person me = Me;
            return (await dbContext.Membership.FirstOrDefaultAsync(x => x.PersonId == me.Id && x.TeamId == teamId)) != null;
        }
    }
}
