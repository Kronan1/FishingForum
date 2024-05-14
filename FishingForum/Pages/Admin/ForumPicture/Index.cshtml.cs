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
    public class IndexModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public IndexModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        public IList<Models.ForumPicture> ForumPicture { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ForumPicture = await _context.ForumPicture.ToListAsync();
        }
    }
}
