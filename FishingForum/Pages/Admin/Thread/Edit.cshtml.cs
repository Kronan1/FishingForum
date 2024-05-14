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

namespace FishingForum.Pages.Admin.Thread
{
    public class EditModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public EditModel(FishingForum.Data.FishingForumContext context)
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

            var thread =  await _context.Thread.FirstOrDefaultAsync(m => m.Id == id);
            if (thread == null)
            {
                return NotFound();
            }
            Thread = thread;
           ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Id");
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

            _context.Attach(Thread).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadExists(Thread.Id))
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

        private bool ThreadExists(Guid id)
        {
            return _context.Thread.Any(e => e.Id == id);
        }
    }
}
