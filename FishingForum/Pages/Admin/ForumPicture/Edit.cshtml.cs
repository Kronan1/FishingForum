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

namespace FishingForum.Pages.Admin.ForumPicture
{
    public class EditModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public EditModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.ForumPicture ForumPicture { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumpicture =  await _context.ForumPicture.FirstOrDefaultAsync(m => m.Id == id);
            if (forumpicture == null)
            {
                return NotFound();
            }
            ForumPicture = forumpicture;
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

            _context.Attach(ForumPicture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumPictureExists(ForumPicture.Id))
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

        private bool ForumPictureExists(int id)
        {
            return _context.ForumPicture.Any(e => e.Id == id);
        }
    }
}
