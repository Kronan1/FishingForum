using FishingForum.Areas.Identity.Data;
using FishingForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FishingForum.Pages
{
    public class SendMessagePageModel : PageModel
    {
        private readonly DAL.UserManager _userManager;
        private readonly SignInManager<FishingForumUser> _signInManager;

        [BindProperty]
        public string RecieverId { get; set; }
        [BindProperty]
        public string NewMessage { get; set; }

        public string RecieverAlias { get; set; }

        public SendMessagePageModel(SignInManager<FishingForumUser> signInManager, DAL.UserManager userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> OnGetAsync(string userId)
        {
            RecieverId = userId;
            var user = await _userManager.GetAnonymizedUserAsync(userId);
            RecieverAlias = user.Alias;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Message message = new() { RecieverId = RecieverId, DateCreated = DateTime.Now, Text = NewMessage, SenderId = _signInManager.UserManager.GetUserId(User) };
            await _userManager.SendMessageAsync(message);

            return RedirectToPage("/Index");
        }
    }
}
