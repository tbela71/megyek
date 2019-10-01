using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Megyek.Models;

namespace Megyek.Pages.Event
{
    public class CreateModel : PageModel
    {
        private readonly IEventRepo repo;
        private readonly IMeProvider meProvider;
        public Models.Team Team;

        public CreateModel(IEventRepo repo, IMeProvider meProvider)
        {
            this.repo = repo;
            this.meProvider = meProvider;
        }

        public void OnGet(int? teamId)
        {
            Team = meProvider.GetManagedTeam(teamId);
        }

        [BindProperty]
        public Megyek.Models.Event Event { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await repo.CreateEventAsync(Event);

            return RedirectToPage("EventIndex", new { teamId = Event?.TeamId });
        }
    }
}