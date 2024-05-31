using FishingForum.Areas.Identity.Data;
using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;

namespace FishingForum.Pages
{
    public class UsersPageModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager<FishingForumUser> _signInManager;
        public List<AnonymizedUser> AnonymizedUsers { get; set; }



        public UsersPageModel(SignInManager<FishingForumUser> signInManager, DAL.UserManager userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            AnonymizedUsers = new List<AnonymizedUser>();
        }
        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/SendMessagePage", new { userId });
            }

            AnonymizedUsers = await _userManager.GetAnonymizedUsersAsync();

            return Page();
        }
    }
}
