using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Megyek.Email;

namespace Megyek.Models
{
    public class MailService : IMailService
    {
        private readonly MegyekDbContext dbContext;
        private readonly IEmailsSender emailsSender;
        private readonly IParticipationTextProvider participationTextProvider;
        private readonly IEventRepo eventRepo;

        public MailService(MegyekDbContext dbContext
            , IEmailsSender emailsSender
            , IParticipationTextProvider participationTextProvider
            , IEventRepo eventRepo
            )
        {
            this.dbContext = dbContext;
            this.emailsSender = emailsSender;
            this.participationTextProvider = participationTextProvider;
            this.eventRepo = eventRepo;
        }

        //public async Task SendInvitationMailAsync(Membership membership)
        //{
        //    //throw new NotImplementedException();
        //}

        public async Task SendParticipatonMailAsync(Participation participation)
        {
            Models.Event Event = await eventRepo.GetEventAsync(participation.EventId);

            List<string> emails = participation.Event.Team.GetEmailsButPersons(new List<int>() { participation.PersonId, participation.ByPersonId });
            if (emails.Count > 0)
            {
                string subject = "Csapatmozgás - " + participation.Event.Team.Name + " - " + participation.Event.Description;
                string message = "<p>" + ((participation.PersonId != participation.ByPersonId) ? participationTextProvider.GetByPersonDisplayName(participation) + " szerint " :"")
                    + participationTextProvider.GetPersonDisplayName(participation) + " csapattársad " 
                    + (participation.Participate ?? false ? "jönni fog :)" : "nem fog jönni :(</p>")
                    + "<p>Eddig " + Event.Participation.Count(x => x.Participate ?? false).ToString() + " csapattársad jelentkezett.</p>"
                    + "<a href=\"https://megyek.eu/?eventId=" + participation.Event.Id.ToString()+"\">megyek.eu</a>";
                await emailsSender.SendEmailsAsync(emails, subject, message);
            }
            if (participation.PersonId != participation.ByPersonId)
            {
                Membership membership = participation.Event.Team.Membership.FirstOrDefault(x => x.PersonId == participation.PersonId);
                if (membership?.Mail ?? false)
                {
                    emails = new List<string> { participation.Person.UserName };
                    string subject = "Csapatmozgás - " + participation.Event.Team.Name + " - " + participation.Event.Description;
                    string message = "<p>" + ((participation.PersonId != participation.ByPersonId) ? participationTextProvider.GetByPersonDisplayName(participation) + " szerint " : "")
                        + (participation.Participate ?? false ? "jönni fogsz :)" : "nem fogsz jönni :(</p>")
                        + "<p>Eddig " + Event.Participation.Count(x => x.Participate ?? false).ToString() + " csapattársad jelentkezett.</p>"
                        + "<a href=\"https://megyek.eu/?eventId=" + participation.Event.Id.ToString() + "\">megyek.eu</a>";
                    await emailsSender.SendEmailsAsync(emails, subject, message);
                }
            }
        }
        public async Task SendPostMailAsync(Post post)
        {
            List<string> emails = post.Team.GetEmailsButPersons(new List<int>() { post.PersonId });
            if (emails.Count > 0)
            {
                string subject = "Új hozzászólás az üzenőfalon - " + post.Team.Name;
                string message = "<p>" + post.DisplayName + " csapattársad új hozzászólást adott az üzenőfalhoz</p>"
                    + "<p>"+ post.Text +"</p>"
                    + "<a href=\"https://megyek.eu/?teamId=" + post.TeamId.ToString() + "\">megyek.eu</a>";
                await emailsSender.SendEmailsAsync(emails, subject, message);
            }
        }

        public async Task SendTestMailAsync()
        {
            await emailsSender.SendEmailsAsync(new List<string>() { "tbela71@gmail.com", "bela@fx.hu" }, "Megyek test", "Megyek test");
        }

        public async Task SendAlertMailAsync(Event evnt)
        {
            if (evnt?.IsAlertAllowed ?? false)
            {
                List<string> emails = evnt.Team.Membership.Where(x => !evnt.Participation.Where(y => y.PersonId == x.PersonId).Any() && x.Mail).Select(x => x.Person.UserName).ToList();
                string subject = "Figyelmeztetés - " + evnt.Team.Name + " - " + evnt.Description;
                string message = "<p>Eddig " + evnt.Participation.Count(x => x.Participate ?? false).ToString() + " csapattársad jelentkezett.</p>"
                    + "<p>Sajnos, Te még nem nyilatkoztál. Tedd meg kérlek MOST!</p>"
                    + "<a href=\"https://megyek.eu/?eventId=" + evnt.Id.ToString() + "\">megyek.eu</a>";
                await emailsSender.SendEmailsAsync(emails, subject, message);
                await eventRepo.SetLastAlertSent(evnt);
            }
        }
    }
}
