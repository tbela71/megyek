using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Megyek.Models;

namespace Megyek.Pages.Membership
{
    public class EditModel : PageModel
    {
        private readonly IMembershipRepo repo;
        public EditModel(IMembershipRepo repo)
        {
            this.repo = repo;
        }

        [BindProperty]
        public Megyek.Models.Membership Membership { get; set; }

        public void OnGet(int? teamId, int? personId)
        {
            Membership = repo.GetManagedMembership(teamId, personId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await repo.SaveMembershipAsync(Membership);
            }
            catch (DbUpdateConcurrencyException)
            {
                Models.Membership x = await repo.GetMembershipAsync(Membership.TeamId, Membership.PersonId);
                if (x == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("MembershipIndex", new { teamId = Membership?.TeamId });
        }
    }
}
