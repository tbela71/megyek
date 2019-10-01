using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megyek.Models
{
    public class EventRepo : IEventRepo
    {
        private MegyekDbContext dbContext;
        private IMeProvider meProvider;

        public EventRepo(MegyekDbContext dbContext, IMeProvider meProvider)
        {
            this.dbContext = dbContext;
            this.meProvider = meProvider;
        }

        public async Task<Event> GetEventAsync(int? eventId)
        {
            return await dbContext.Event.FindAsync(eventId);
        }

        public IList<Event> GetEventList(int? teamId)
        {
            Team team = meProvider.GetManagedTeam(teamId);

            return dbContext.Event
                .Where(x => x.TeamId == team.Id && x.Date > DateTime.Now.AddHours(-1))
                .OrderBy(x => x.Date)
                .Take(50)
                .Include(x => x.Team)
                .ToList();
        }

        public Event GetManagedEvent(int? eventId)
        {
            Event returnValue = dbContext.Event.Include(x => x.Team).FirstOrDefault(m => m.Id == eventId);
            Team team = meProvider.GetManagedTeam(returnValue?.TeamId);
            if (team.Id != returnValue?.TeamId)
            {
                returnValue = null;
            }
            return returnValue;
        }
        public async Task DeleteEventAsync(int? id)
        {
            Event evnt = await GetEventAsync(id);
            if (evnt != null)
            {
                dbContext.Participation.RemoveRange(dbContext.Participation.Where(x => x.EventId == evnt.Id));
                dbContext.Event.Remove(evnt);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task SaveEventAsync(Event evnt)
        {
            dbContext.Attach(evnt).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateEventAsync(Event evnt)
        {
            evnt.EndDate = evnt.Date;
            evnt.Special = false;
            dbContext.Event.Add(evnt);
            await dbContext.SaveChangesAsync();
        }

        public Event GetEvent(int? eventId)
        {
            return dbContext.Event.FirstOrDefault(x => x.Id == eventId);
        }

        public async Task SetLastAlertSent(Event evnt)
        {
            evnt.LastAlertSent = DateTime.Now;
            await dbContext.SaveChangesAsync();
        }
    }
}
