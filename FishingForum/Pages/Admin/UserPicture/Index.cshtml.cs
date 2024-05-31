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
    public class IndexModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public IndexModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        public IList<Models.UserPicture> UserPicture { get;set; } = default!;

        public async Task OnGetAsync()
        {
            UserPicture = await _context.UserPicture
                .Include(u => u.FishingForumUser).ToListAsync();
        }
    }
}
