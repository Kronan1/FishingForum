using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.PostUserPicture
{
    public class DetailsModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public DetailsModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        public Models.PostUserPicture PostUserPicture { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postuserpicture = await _context.PostUserPicture.FirstOrDefaultAsync(m => m.PostId == id);
            if (postuserpicture == null)
            {
                return NotFound();
            }
            else
            {
                PostUserPicture = postuserpicture;
            }
            return Page();
        }
    }
}
