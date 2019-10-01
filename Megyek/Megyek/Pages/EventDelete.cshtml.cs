using System.Linq;
using System.Threading.Tasks;
using Megyek.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Megyek.Pages.Event
{
    public class DeleteModel : PageModel
    {
        private readonly IEventRepo repo;
        public DeleteModel(IEventRepo repo)
        {
            this.repo = repo;
        }

        [BindProperty]
        public Megyek.Models.Event Event { get; set; }

        public void OnGet(int? id)
        {
            Event = repo.GetManagedEvent(id);
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await repo.DeleteEventAsync(id);
            return RedirectToPage("EventIndex", new { teamId = Event?.TeamId });
        }
    }
}
