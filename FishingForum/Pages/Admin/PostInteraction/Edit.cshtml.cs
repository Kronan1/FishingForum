using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.PostInteraction
{
    public class EditModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public EditModel(FishingForum.Data.FishingForumContext context)
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

            var postinteraction =  await _context.PostInteraction.FirstOrDefaultAsync(m => m.Id == id);
            if (postinteraction == null)
            {
                return NotFound();
            }
            PostInteraction = postinteraction;
           ViewData["PostId"] = new SelectList(_context.Post, "Id", "Text");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PostInteraction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostInteractionExists(PostInteraction.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PostInteractionExists(Guid id)
        {
            return _context.PostInteraction.Any(e => e.Id == id);
        }
    }
}
