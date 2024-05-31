using FishingForum.Areas.Identity.Data;
using FishingForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Editing;
using System.Security.Claims;

namespace FishingForum.Pages
{
    public class MessagePageModel : PageModel
    {
        private readonly DAL.UserManager _userManager;
        private readonly SignInManager<FishingForumUser> _signInManager;
        private readonly UserManager<FishingForumUser> _userManagerIdentity;
        public List<Models.Message> Messages { get; set; }
        public List<AnonymizedUser> AnonymizedUsers { get; set; }


        public MessagePageModel(SignInManager<FishingForumUser> signInManager, DAL.UserManager userManager, UserManager<FishingForumUser> userManagerIdentity)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userManagerIdentity = userManagerIdentity;
            AnonymizedUsers = new List<AnonymizedUser>();

        }
        public async Task<IActionResult> OnGetAsync(string senderId, string deleteId)
        {
            if (!string.IsNullOrEmpty(senderId))
            {
                return RedirectToPage("/SendMessagePage", new { userId = senderId });
            }

            if (!string.IsNullOrEmpty(deleteId))
            {
                if (Guid.TryParse(deleteId, out Guid messageId))
                {
                    await _userManager.DeleteMessage(messageId);
                }
            }


            await Initialize();

            return Page();
        }

        private async Task Initialize()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                Messages = await _userManager.GetMessagesAsync(userId);

                List<string> userIdList = new List<string>();
                foreach (var message in Messages)
                {
                    if (!userIdList.Contains(message.SenderId))
                    {
                        userIdList.Add(message.SenderId);
                    }
                }

                AnonymizedUsers = await _userManager.GetAnonymizedUsersAsync(userIdList);
            }
        }
    }
}
