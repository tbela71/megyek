using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public class ParticipationRepo : IParticipationRepo
    {
        private MegyekDbContext dbContext;
        private IMeProvider meProvider;
        public ParticipationRepo(MegyekDbContext dbContext, IMeProvider meProvider)
        {
            this.dbContext = dbContext;
            this.meProvider = meProvider;
        }


        public async Task AddParticipationAsync(Participation participation)
        {
            Event evnt = await dbContext.Event.FindAsync(participation.EventId);
            if (meProvider.IsMemberOfTeam(evnt?.TeamId))
            {
                dbContext.Participation.Add(participation);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveParticipationAsync(Participation participation)
        {
            Event evnt = await dbContext.Event.FindAsync(participation.EventId);
            if (meProvider.IsMemberOfTeam(evnt?.TeamId))
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
