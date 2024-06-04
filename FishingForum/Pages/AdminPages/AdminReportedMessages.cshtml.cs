using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FishingForum.Pages.AdminPages
{
    [Authorize(Roles = "Admin")]
    public class AdminReportedMessagesModel : PageModel
    {
        private readonly AdminManager _adminManager;
        public List<Post> Posts { get; set; }

        public AdminReportedMessagesModel(AdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        public async Task<IActionResult> OnGetAsync(Guid deleteId)
        {
            if (deleteId != Guid.Empty)
            {
                await _adminManager.RemovePost(deleteId);
            }

            await Initialize();
            return Page();
        }

        private async Task Initialize()
        {
            Posts = await _adminManager.GetReportedPostsAsync();
        }
    }
}
