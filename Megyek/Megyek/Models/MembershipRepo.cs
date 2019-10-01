using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public class MembershipRepo : IMembershipRepo
    {
        private MegyekDbContext dbContext;
        private IMeProvider meProvider;
        public MembershipRepo(MegyekDbContext dbContext, IMeProvider meProvider)
        {
            this.dbContext = dbContext;
            this.meProvider = meProvider;
        }

        public async Task<Membership> GetMembershipAsync(int? teamId, int? personId)
        {
            return await dbContext.Membership.FindAsync(personId, teamId);
        }

        public IList<Membership> GetMembershipList(int? teamId)
        {
            Team team = meProvider.GetManagedTeam(teamId);
            return dbContext.Membership
                .Where(x => x.TeamId == team.Id)
                .Include(x => x.Team)
                .Include(x => x.Person)
                .ToList();
        }

        public Membership GetManagedMembership(int? teamId, int? personId)
        {
            Membership returnValue = dbContext.Membership.Include(x => x.Team).Include(x => x.Person).FirstOrDefault(m => m.TeamId == teamId && m.PersonId == personId);
            Team team = meProvider.GetManagedTeam(teamId);
            if (team.Id != returnValue?.TeamId)
            {
                returnValue = null;
            }
            return returnValue;
        }
        public async Task DeleteMembershipAsync(int? teamId, int? personId)
        {
            Membership membership = await GetMembershipAsync(teamId, personId);
            Membership manager = await dbContext.Membership.FirstOrDefaultAsync(x => x.TeamId == teamId && x.Manager && x.PersonId != personId);
            if (membership != null 
                && manager != null // Deletion of last manager is not allowed
                && membership.Person.UserName != meProvider.Me.UserName // Deletion of myself is not allowed
                )
            {
                dbContext.Membership.Remove(membership);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task CreateMembershipAsync(Membership membership)
        {
            Team team = meProvider.GetManagedTeam(membership.TeamId);
            if (team.Id == membership.TeamId)
            {
                // Check person
                Person person = await dbContext.Person.FirstOrDefaultAsync(x => x.UserName == membership.Person.UserName);
                if (person == null && !string.IsNullOrEmpty(membership.Person.UserName))
                {
                    // Create person
                    person = new Person()
                    {
                        UserName = membership.Person.UserName,
                    };
                    dbContext.Person.Add(person);
                    await dbContext.SaveChangesAsync();
                    person = await dbContext.Person.FirstOrDefaultAsync(x => x.UserName == membership.Person.UserName);
                }
                if (person != null)
                {
                    // Check membership
                    Membership x = await GetMembershipAsync(membership.TeamId, person.Id);
                    if (x == null)
                    {
                        membership.PersonId = person.Id;
                        if (string.IsNullOrEmpty(membership.DisplayName))
                        {
                            membership.DisplayName = person.UserName;
                        }
                        dbContext.Membership.Add(membership);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task SaveMembershipAsync(Membership membership)
        {
            if (membership != null)
            {
                Membership manager = await dbContext.Membership.FirstOrDefaultAsync(x => x.TeamId == membership.TeamId && x.Manager && x.PersonId != membership.PersonId);
                if (manager != null || membership.Manager) // Deletion of last manager is not allowed
                {
                    dbContext.Attach(membership).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task SetMailAsync(Membership membership)
        {
            if (membership != null)
            {
                membership.Mail = !membership.Mail;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
