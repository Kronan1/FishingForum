using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.UserPicture
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
            return Page();
        }

        [BindProperty]
        public Models.UserPicture UserPicture { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.UserPicture.Add(UserPicture);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
