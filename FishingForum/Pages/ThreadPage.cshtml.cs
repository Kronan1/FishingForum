using FishingForum.Areas.Identity.Data;
using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;

namespace FishingForum.Pages
{
    public class ThreadPageModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager<FishingForumUser> _signInManager;
        private readonly UserManager<FishingForumUser> _userManagerIdentity;
        public Models.Thread Thread { get; set; }
        public List<Post> Posts { get; set; }
        public List<PostInteraction> Interactions { get; set; }
        public List<AnonymizedUser> AnonymizedUsers { get; set; }
        [BindProperty]
        public string NewPost { get; set; }

        [BindProperty]
        public Guid ThreadId { get; set; }

        public bool Reported { get; set; }

        public ThreadPageModel(SignInManager<FishingForumUser> signInManager, DAL.UserManager userManager, UserManager<FishingForumUser> userManagerIdentity)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userManagerIdentity = userManagerIdentity;
            AnonymizedUsers = new List<AnonymizedUser>();
        }
        public async Task<IActionResult> OnGetAsync(Guid threadId, Guid postId, Guid reportId, Guid likeId, Guid dislikeId)
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
                    
                }
                return Page();
            }

            if (likeId != Guid.Empty)
            {
                var post = await _userManager.GetPostAsync(likeId);
                if (post != null)
                {
                    var userId = _userManagerIdentity.GetUserId(User);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        await _userManager.AddInteraction(likeId, userId, "Like");
                    }
                    await Initialize(post.ThreadId);
                    ThreadId = post.ThreadId;
                    return Page();
                }
            }

            if (dislikeId != Guid.Empty)
            {
                var post = await _userManager.GetPostAsync(dislikeId);
                if (post != null)
                {
                    var userId = _userManagerIdentity.GetUserId(User);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        await _userManager.AddInteraction(dislikeId, userId, "Dislike");
                    }
                    await Initialize(post.ThreadId);
                    ThreadId = post.ThreadId;
                    return Page();
                }

                return Page();
            }

            await Initialize(threadId);

            ThreadId = Thread.Id;

            return Page();
        }

        private async Task Initialize(Guid threadId)
        {
            Thread = await _userManager.GetThreadAsync(threadId);
            Posts = await _userManager.GetPostsAsync(threadId);
            Interactions = await _userManager.GetInteractionsAsync(Posts);

            
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

                await _userManager.CreatePostAsync(post);
            }
            return RedirectToPage("/ThreadPage", new { threadId = ThreadId });
        }
    }
}
