using FishingForum.Areas.Identity.Data;
using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.CodeDom;
using System.Threading;

namespace FishingForum.Pages
{
    public class SubCategoryPageModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager<FishingForumUser> _signInManager;
        public SubCategory SubCategory { get; set; }
        public FishingForum.Models.Thread NewThread { get; set; }
        public List<Models.Thread> Threads { get; set; }

        public SubCategoryPageModel(SignInManager<FishingForumUser> signInManager, DAL.UserManager userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(int subCategoryId, int createthreadId, string threadId)
        {
            if (!string.IsNullOrEmpty(threadId))
            {
                if (Guid.TryParse(threadId, out Guid id))
                {
                    return RedirectToPage("/ThreadPage", new { ThreadId = id });
                }

                return NotFound("Thread not found");
            }

            if (createthreadId != 0)
            {
                return RedirectToPage("/CreateThread", new { subCategoryId = createthreadId });
            }


            await Initialize(subCategoryId);


            return Page();
        }

        

        private async Task Initialize(int subCategoryId)
        {
            SubCategory = await _userManager.GetSubCategoryAsync(subCategoryId);
            Threads = await _userManager.GetThreadsAsync(subCategoryId);

        }
    }
}
