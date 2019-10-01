using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Megyek.Pages
{
    public class AboutModel : PageModel
    {
        public string Message1 { get; set; }
        public string Message2 { get; set; }

        public void OnGet()
        {
            Message1 = "This app is for sports teams.";
            Message2 = "Allows team members to indicate their willingness to attend each event with a single click.";
        }
    }
}
