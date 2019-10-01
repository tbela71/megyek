using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Megyek.Models;

namespace Megyek.Pages.Membership
{
    public class IndexModel : PageModel
    {
        private readonly IMembershipRepo repo;
        private readonly IMeProvider meProvider;
        public Models.Team Team;

        public IndexModel(IMembershipRepo repo, IMeProvider meProvider)
        {
            this.repo = repo;
            this.meProvider = meProvider;
        }

        public IList<Megyek.Models.Membership> Membership { get;set; }

        public void OnGet(int? teamId)
        {
            Team = meProvider.GetManagedTeam(teamId);
            Membership = repo.GetMembershipList(Team?.Id);
        }
    }
}
