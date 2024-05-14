using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.ForumPicture
{
    public class DetailsModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public DetailsModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        public Models.ForumPicture ForumPicture { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumpicture = await _context.ForumPicture.FirstOrDefaultAsync(m => m.Id == id);
            if (forumpicture == null)
            {
                return NotFound();
            }
            else
            {
                ForumPicture = forumpicture;
            }
            return Page();
        }
    }
}
