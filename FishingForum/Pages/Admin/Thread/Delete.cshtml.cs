using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.Thread
{
    public class DeleteModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public DeleteModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Thread Thread { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thread = await _context.Thread.FirstOrDefaultAsync(m => m.Id == id);

            if (thread == null)
            {
                return NotFound();
            }
            else
            {
                Thread = thread;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thread = await _context.Thread.FindAsync(id);
            if (thread != null)
            {
                Thread = thread;
                _context.Thread.Remove(Thread);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
