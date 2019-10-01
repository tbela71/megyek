using System;
using System.Linq;

namespace Megyek.Models
{
    public class ParticipationTextProvider : IParticipationTextProvider
    {
        private MegyekDbContext dbContext;
        public ParticipationTextProvider(MegyekDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private string getPersonDisplayName(Participation participation, int personId)
        {
            return participation?.Event?.Team?.Membership?.FirstOrDefault(x => x.PersonId == personId)?.DisplayName
                ?? dbContext.Person.Find(personId)?.UserName
                ?? "?";
        }

        public string GetPersonDisplayName(Participation participation)
        {
            return getPersonDisplayName(participation, participation.PersonId);
        }

        public string GetByPersonDisplayName(Participation participation)
        {
            return getPersonDisplayName(participation, participation.ByPersonId);
        }

        public string GetPersonDisplayNameX(Participation participation)
        {
            return GetPersonDisplayName(participation).Replace(" ", "");
        }
        public string GetByPersonDisplayNameX(Participation participation)
        {
            return GetByPersonDisplayName(participation).Replace(" ", "");
        }

        public string GetDisplayText(Participation participation)
        {
            if (participation.PersonId == participation.ByPersonId)
            {
                if (participation.Participate ?? false)
                {
                    return "You applied at " + participation.Date.ToString("yyyy.MM.dd H:mm:ss");
                }
                return "You rejected at " + participation.Date.ToString("yyyy.MM.dd H:mm:ss");
            }
            else
            {
                if (participation.Participate ?? false)
                {
                    return "You have been applied by " + GetByPersonDisplayName(participation) + " at " + participation.Date.ToString("yyyy.MM.dd H:mm:ss");
                }
                return "You have been rejected by " + GetByPersonDisplayName(participation) + " at " + participation.Date.ToString("yyyy.MM.dd H:mm:ss");
            }
        }

    }
}
