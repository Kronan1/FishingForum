using FishingForum.Areas.Identity.Data;
using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FishingForum.Pages
{
    public class ThreadPageModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager<FishingForumUser> _signInManager;
        public Models.Thread Thread { get; set; }
        public List<Post> Posts { get; set; }
        public List<AnonymizedUser> AnonymizedUsers { get; set; }
        [BindProperty]
        public string NewPost { get; set; }

        [BindProperty]
        public Guid ThreadId { get; set; }

        public bool Reported { get; set; }

        public ThreadPageModel(SignInManager<FishingForumUser> signInManager, DAL.UserManager userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            AnonymizedUsers = new List<AnonymizedUser>();
        }
        public async Task<IActionResult> OnGetAsync(Guid threadId, Guid postId, Guid reportId)
        {
            if (postId != Guid.Empty)
            {
                var post = await _userManager.GetPostAsync(postId);
                if (post != null)
                {
                    NewPost = "“" +  post.Text + "”";
                    await Initialize(post.ThreadId);
                    ThreadId = post.ThreadId;
                    return Page();
                }
                
            }

            if (reportId != Guid.Empty)
            {
                var post = await _userManager.GetPostAsync(reportId);
                if (post != null)
                {
                    if (!post.Reported)
                    {
                        await _userManager.ReportPostAsync(post);
                    }
                    await Initialize(post.ThreadId);
                    ThreadId = post.ThreadId;
                    Reported = true;
                    return Page();
                }
            }

            await Initialize(threadId);

            ThreadId = Thread.Id;

            return Page();
        }

        private async Task Initialize(Guid threadId)
        {
            Thread = await _userManager.GetThreadAsync(threadId);
            Posts = await _userManager.GetPostsAsync(threadId);
            
            List<string> userIdList = new List<string>();
            foreach (var post in Posts)
            {
                if (!userIdList.Contains(post.UserId))
                {
                    userIdList.Add(post.UserId);
                }
            }

            AnonymizedUsers = await _userManager.GetAnonymizedUsersAsync(userIdList);

            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Post post = new();
                post.Text = NewPost;
                post.ThreadId = ThreadId;
                post.DateCreated = DateTime.Now;
                post.UserId = _signInManager.UserManager.GetUserId(User);
                if (string.IsNullOrEmpty(post.UserId))
                {
                    return NotFound("User Id not found");
                }

                _userManager.CreatePostAsync(post);
            }
            return RedirectToPage("/ThreadPage", new { threadId = ThreadId });
        }
    }
}
