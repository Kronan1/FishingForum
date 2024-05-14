using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.UserPicture
{
    public class DetailsModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public DetailsModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        public Models.UserPicture UserPicture { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userpicture = await _context.UserPicture.FirstOrDefaultAsync(m => m.Id == id);
            if (userpicture == null)
            {
                return NotFound();
            }
            else
            {
                UserPicture = userpicture;
            }
            return Page();
        }
    }
}
