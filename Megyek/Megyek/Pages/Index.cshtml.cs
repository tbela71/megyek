using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Megyek.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Megyek.Pages
{
    public class IndexModel : PageModel
    {
        //private MegyekDbContext dbContext;
        private readonly IMeProvider meProvider;
        private readonly IEventRepo eventRepo;
        private readonly IMembershipRepo membershipRepo;
        private readonly IParticipationRepo participationRepo;
        private readonly IParticipationTextProvider participationTextProvider;
        private readonly IPostRepo postRepo;
        private readonly IMailService mailService;

        public Person Me;
        public IList<Team> MyTeams;
        public Team Team;
        public Models.Event Event;
        public IList<Models.Event> NextEvents;
        public IList<Models.Event> SpecialEvents;
        public Models.Membership Membership;
        public Participation Participation;

        public bool? Participate { get { return Participation?.Participate; } }
        public int GoingCount { get { return Event?.Participation?.Count(x => x.Participate ?? false) ?? 0; } }
        public int SkippingCount { get { return Event?.Participation?.Count(x => !(x.Participate ?? true)) ?? 0; } }

        public IList<Models.Post> Posts;
        public Models.Post MyLastPost;

        public IndexModel
            (IMeProvider meProvider
            , IEventRepo eventRepo
            , IMembershipRepo membershipRepo
            , IParticipationRepo participationRepo
            , IParticipationTextProvider participationTextProvider
            , IPostRepo postRepo
            , IMailService mailService
            )
        {
            //this.dbContext = dbContext;
            this.meProvider = meProvider;
            this.eventRepo = eventRepo;
            this.membershipRepo = membershipRepo;
            this.participationRepo = participationRepo;
            this.participationTextProvider = participationTextProvider;
            this.postRepo = postRepo;
            this.mailService = mailService;
        }

        public async Task OnGetAsync(int? teamId, int? eventId)
        {
            await LoadContextAsync(teamId, eventId);
        }

        private async Task LoadContextAsync(int? teamId, int? eventId)
        {
            Me = meProvider.Me;
            MyTeams = Me.Membership.Select(x => x.Team).ToList() ?? new List<Team>();

            Event = await eventRepo.GetEventAsync(eventId);
            if (Event == null)
            {
                Team = meProvider.GetTeam(teamId);
                Event = Team?.Event.OrderBy(x => x.Date).FirstOrDefault(x => x.Date > DateTime.Now);
            }
            else
            {
                Team = meProvider.GetTeam(Event.TeamId);
                if (Team?.Id != Event.TeamId)
                {
                    Event = null;
                }
            }
            Membership = Me.Membership.FirstOrDefault(x => x.TeamId == Team?.Id);
            Participation = Event?.Participation.FirstOrDefault(x => x.PersonId == Me.Id);

            NextEvents = Team.Event
                .OrderBy(x => x.Date)
                .Where(x => !x.Special && x.Date > DateTime.Now.AddHours(-1) && x.Id != Event?.Id)
                .Take(5)
                .ToList();
            SpecialEvents = Team.Event
                .OrderBy(x => x.Date)
                .Where(x => x.Special && x.Date > DateTime.Now.AddHours(-1) && x.Id != Event?.Id)
                .ToList();

            Posts = Team.Post.TakeLast(50).OrderByDescending(x => x.Id).ToList();
            if (Posts.Count > 0 && Posts[0].PersonId == Me?.Id) MyLastPost = Posts[0];
        }

        public async Task<IActionResult> OnPostGoing(int? eventId, int? personId = null)
        {
            await Going(eventId, personId);
            return RedirectToPage("./Index", new { eventId });
        }
        public async Task OnGetGoing(int? eventId, int? personId = null)
        {
            await Going(eventId, personId);
            await LoadContextAsync(null, eventId);
        }
        public async Task Going(int? eventId, int? personId = null)
        {
            await LoadContextAsync(null, eventId);
            if (Event != null 
                && ((Membership?.Manager ?? false) || personId == null))
            {
                Participation participation = Event?.Participation.FirstOrDefault(x => x.PersonId == (personId ?? Me.Id));
                if (!(participation?.Participate ?? false))
                {
                    if (participation == default(Participation))
                    {
                        participation = new Participation()
                        {
                            PersonId = personId ?? Me.Id,
                            ByPersonId = Me.Id,
                            EventId = Event.Id,
                            Participate = true,
                            Date = DateTime.Now
                        };
                        await participationRepo.AddParticipationAsync(participation);
                    }
                    else
                    {
                        participation.ByPersonId = Me.Id;
                        participation.Participate = true;
                        participation.Date = DateTime.Now;
                        await participationRepo.SaveParticipationAsync(participation);
                    }
                    await mailService.SendParticipatonMailAsync(participation);
                }
            }
        }

        public async Task<IActionResult> OnPostSkipping(int? eventId, int? personId = null)
        {
            await Skipping(eventId, personId);
            return RedirectToPage("./Index", new { eventId });
        }
        public async Task OnGetSkipping(int? eventId, int? personId = null)
        {
            await Skipping(eventId, personId);
            await LoadContextAsync(null, eventId);
        }
        public async Task Skipping(int? eventId, int? personId = null)
        {
            await LoadContextAsync(null, eventId);
            if (Event != null
                && ((Membership?.Manager ?? false) || personId == null))
            { 
                Participation participation = Event?.Participation.FirstOrDefault(x => x.PersonId == (personId ?? Me.Id));
                if (participation?.Participate ?? true)
                {
                    if (participation == default(Participation))
                    {
                        participation = new Participation()
                        {
                            PersonId = personId ?? Me.Id,
                            ByPersonId = Me.Id,
                            EventId = Event.Id,
                            Participate = false,
                            Date = DateTime.Now
                        };
                        await participationRepo.AddParticipationAsync(participation);
                    }
                    else
                    {
                        participation.ByPersonId = Me.Id;
                        participation.Participate = false;
                        participation.Date = DateTime.Now;
                        await participationRepo.SaveParticipationAsync(participation);
                    }
                    await mailService.SendParticipatonMailAsync(participation);
                }
            }
        }

        public async Task<IActionResult> OnPostPostingAsync(string postText, int? teamId, int? eventId)
        {
            if (string.IsNullOrEmpty(postText)) return Page();
            await LoadContextAsync(teamId, eventId);
            if (Event == null) return Page();

            Models.Post post = new Models.Post()
            {
                TeamId = Event.TeamId,
                PersonId = Me.Id,
                Date = DateTime.Now,
                Text = postText
            };
            await postRepo.AddPostAsync(post);
            await mailService.SendPostMailAsync(post);
            return RedirectToPage("./Index", new { teamId, eventId });
        }

        public async Task<IActionResult> OnPostDeletePost(int? postId, int? teamId, int? eventId)
        {
            await LoadContextAsync(teamId, eventId);
            if (Event == null) return Page();

            Models.Post post = await postRepo.GetPostAsync(postId);
            if (post != null && post.PersonId == Me?.Id)
            {
                await postRepo.RemovePostAsync(post);
            }
            return RedirectToPage("./Index", new { teamId, eventId });
        }
        public async Task<IActionResult> OnPostMail(int? teamId, int? eventId)
        {
            await LoadContextAsync(teamId, eventId);
            await membershipRepo.SetMailAsync(this.Membership);
            return RedirectToPage("./Index", new { teamId, eventId });
        }

        public async Task<IActionResult> OnPostAlert(int? teamId, int? eventId)
        {
            await LoadContextAsync(teamId, eventId);
            if (Event == null) return Page();

            await mailService.SendAlertMailAsync(Event);
            return RedirectToPage("./Index", new { teamId, eventId });
        }

        public string GetParticipationDisplayNameX(Participation participation)
        {
            return participationTextProvider.GetPersonDisplayNameX(participation);
        }

        public string GetParticipationDisplayText()
        {
            return participationTextProvider.GetDisplayText(Participation);
        }

    }
}