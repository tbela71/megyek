using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Megyek.Models;

namespace Megyek.Pages.Membership
{
    public class CreateModel : PageModel
    {
        private readonly IMembershipRepo repo;
        private readonly IMeProvider meProvider;

        public CreateModel(IMembershipRepo repo, IMeProvider meProvider)
        {
            this.repo = repo;
            this.meProvider = meProvider;
        }

        public void OnGet(int? teamId)
        {
            Team team = meProvider.GetManagedTeam(teamId);
            Membership = new Models.Membership()
            {
                Team = team,
                TeamId = team.Id,
                Person = new Person()
            };
        }

        [BindProperty]
        public Models.Membership Membership { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await repo.CreateMembershipAsync(Membership);

            return RedirectToPage("MembershipIndex", new { teamId = Membership?.TeamId } );
        }
    }
}