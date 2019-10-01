using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Megyek.Models;

namespace Megyek.Pages.Post
{
    public class IndexModel : PageModel
    {
        private readonly IPostRepo repo;
        public Models.Team Team;

        public IndexModel(IPostRepo repo)
        {
            this.repo = repo;
        }

        public IList<Megyek.Models.Post> Post { get;set; }

        public void OnGet(int? teamId)
        {
            Team = repo.GetManagedTeam(teamId);
            Post = repo.GetPostList(Team?.Id, 1000);
        }
    }
}
