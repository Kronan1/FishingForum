using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.SubCategory
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
        ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Models.SubCategory SubCategory { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SubCategory.Add(SubCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
