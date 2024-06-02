using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.ProfilePicture
{
    public class DeleteModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public DeleteModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.ProfilePicture ProfilePicture { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profilepicture = await _context.ProfilePicture.FirstOrDefaultAsync(m => m.Id == id);

            if (profilepicture == null)
            {
                return NotFound();
            }
            else
            {
                ProfilePicture = profilepicture;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profilepicture = await _context.ProfilePicture.FindAsync(id);
            if (profilepicture != null)
            {
                ProfilePicture = profilepicture;
                _context.ProfilePicture.Remove(ProfilePicture);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
