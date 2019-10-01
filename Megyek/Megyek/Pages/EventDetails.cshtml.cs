using Microsoft.AspNetCore.Mvc.RazorPages;
using Megyek.Models;

namespace Megyek.Pages.Event
{
    public class DetailsModel : PageModel
    {
        private readonly IEventRepo repo;
        public DetailsModel(IEventRepo repo)
        {
            this.repo = repo;
        }

        public Megyek.Models.Event Event { get; set; }

        public void OnGet(int? id)
        {
            Event = repo.GetManagedEvent(id);
        }
    }
}