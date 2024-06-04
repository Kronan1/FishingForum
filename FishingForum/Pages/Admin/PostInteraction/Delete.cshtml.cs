using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.PostInteraction
{
    public class DeleteModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public DeleteModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.PostInteraction PostInteraction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postinteraction = await _context.PostInteraction.FirstOrDefaultAsync(m => m.Id == id);

            if (postinteraction == null)
            {
                return NotFound();
            }
            else
            {
                PostInteraction = postinteraction;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postinteraction = await _context.PostInteraction.FindAsync(id);
            if (postinteraction != null)
            {
                PostInteraction = postinteraction;
                _context.PostInteraction.Remove(PostInteraction);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
