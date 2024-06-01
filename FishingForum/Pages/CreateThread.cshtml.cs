using FishingForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using FishingForum.Areas.Identity.Data;


namespace FishingForum.Pages
{
    public class CreateThreadModel : PageModel
    {
        public Areas.Identity.Data.FishingForumUser MyUser { get; set; }

        private readonly UserManager<Areas.Identity.Data.FishingForumUser> _userManagerIdentity;

        private readonly DAL.UserManager _userManager;

        [BindProperty]
        public FishingForum.Models.Thread NewThread { get; set; }

        [BindProperty]
        public Post NewPost { get; set; }

        [BindProperty]
        public int SubCategoryId { get; set; }

        public string ErrorMessage { get; set; }



        public CreateThreadModel(UserManager<Areas.Identity.Data.FishingForumUser> userManagerIdentity, DAL.UserManager userManager)
        {
            _userManagerIdentity = userManagerIdentity;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int subCategoryId)
        {
            MyUser = await _userManagerIdentity.GetUserAsync(User);

            if (MyUser == null)
            {
                return NotFound("User not found");
            }

            if (!Guid.TryParse(MyUser.Id, out Guid userId))
            {
                return BadRequest("Invalid user ID");
            }

            SubCategoryId = subCategoryId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MyUser = await _userManagerIdentity.GetUserAsync(User);


            if (MyUser == null)
            {
                return NotFound("User not found");
            }

            if (!Guid.TryParse(MyUser.Id, out Guid userId))
            {
                return BadRequest("Invalid user ID");
            }

            NewThread.DateCreated = DateTime.Now;
            NewThread.UserId = MyUser.Id;
            NewThread.SubCategoryId = SubCategoryId;

            var threadsInSubCategory = await _userManager.GetThreadsAsync(SubCategoryId);

            foreach (var thread in threadsInSubCategory)
            {
                if (thread.Title == NewThread.Title)
                {
                    ErrorMessage = "Invalid, Title already exists";
                    return Page();
                }
            }

            if (string.IsNullOrEmpty(NewThread.Title))
            {
                ErrorMessage = "Invalid, Title is empty";
                return Page();
            }

            if (NewThread.SubCategoryId == 0)
            {
                return NotFound("Subcategory not found");
            }


            await _userManager.CreateThreadAsync(NewThread);
            var createdThread = await _userManager.GetThreadAsync(NewThread.Title, NewThread.SubCategoryId);

            if (createdThread == null)
            {
                return NotFound("Thread not found");
            }

            
            // Possible TODO Use modelstate.IsValid instead

            NewPost.DateCreated = DateTime.Now;
            NewPost.UserId = MyUser.Id;
            NewPost.ThreadId = createdThread.Id;

            await _userManager.CreatePostAsync(NewPost);


            // TODO Return to created post
            return RedirectToPage("/ThreadPage", new { threadId = createdThread.Id });
        }


    }
}
