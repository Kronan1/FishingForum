using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.PostInteraction
{
    public class CreateModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public CreateModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["PostId"] = new SelectList(_context.Post, "Id", "Text");
            return Page();
        }

        [BindProperty]
        public Models.PostInteraction PostInteraction { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.PostInteraction.Add(PostInteraction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
