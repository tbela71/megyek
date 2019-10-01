using System.Linq;
using System.Threading.Tasks;
using Megyek.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Megyek.Pages.Membership
{
    public class DeleteModel : PageModel
    {
        private readonly IMembershipRepo repo;
        public DeleteModel(IMembershipRepo repo)
        {
            this.repo = repo;
        }

        [BindProperty]
        public Megyek.Models.Membership Membership { get; set; }

        public void OnGet(int? teamId, int? personId)
        {
            Membership = repo.GetManagedMembership(teamId, personId);
        }

        public async Task<IActionResult> OnPostAsync(int? teamId, int? personId)
        {
            if (teamId == null || personId == null)
            {
                return NotFound();
            }
            await repo.DeleteMembershipAsync(teamId, personId);
            return RedirectToPage("MembershipIndex", new { teamId = teamId });
        }
    }
}
