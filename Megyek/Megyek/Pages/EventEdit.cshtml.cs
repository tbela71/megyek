using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Megyek.Models;

namespace Megyek.Pages.Event
{
    public class EditModel : PageModel
    {
        private readonly IEventRepo repo;
        public EditModel(IEventRepo repo)
        {
            this.repo = repo;
        }

        [BindProperty]
        public Megyek.Models.Event Event { get; set; }

        public void OnGet(int? id)
        {
            Event = repo.GetManagedEvent(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await repo.SaveEventAsync(Event);
            }
            catch (DbUpdateConcurrencyException)
            {
                Models.Event x = await repo.GetEventAsync(Event.Id);
                if (x == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("EventIndex", new { teamId = Event?.TeamId });
        }
    }
}
