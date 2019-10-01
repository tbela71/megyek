using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Megyek.Models;

namespace Megyek.Pages.Event
{
    public class IndexModel : PageModel
    {
        private readonly IEventRepo repo;
        private readonly IMeProvider meProvider;
        public Models.Team Team;

        public IndexModel(IEventRepo repo, IMeProvider meProvider)
        {
            this.repo = repo;
            this.meProvider = meProvider;
        }

        public IList<Megyek.Models.Event> Event { get;set; }

        public void OnGet(int? teamId)
        {
            Team = meProvider.GetManagedTeam(teamId);
            Event = repo.GetEventList(Team?.Id);
        }
    }
}
