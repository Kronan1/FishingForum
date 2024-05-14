using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FishingForum.Data;
using FishingForum.Models;

namespace FishingForum.Pages.Admin.Post
{
    public class IndexModel : PageModel
    {
        private readonly FishingForum.Data.FishingForumContext _context;

        public IndexModel(FishingForum.Data.FishingForumContext context)
        {
            _context = context;
        }

        public IList<Models.Post> Post { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Post = await _context.Post
                .Include(p => p.Thread).ToListAsync();
        }
    }
}
