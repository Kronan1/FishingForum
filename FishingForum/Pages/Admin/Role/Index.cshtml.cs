using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FishingForum.Pages.Admin.Role
{
    public class IndexModel : PageModel
    {
        public List<Areas.Identity.Data.FishingForumUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }


        [BindProperty]
        public string RoleName { get; set; }

        public readonly UserManager<Areas.Identity.Data.FishingForumUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<Areas.Identity.Data.FishingForumUser> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task OnGetAsync()
        {
            Roles = await _roleManager.Roles.ToListAsync();
            Users = await _userManager.Users.ToListAsync();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (RoleName != null)
            {
                await CreateRoleAsync(RoleName);
            }
            return RedirectToPage("./Index");
        }


        public async Task CreateRoleAsync(string roleName)
        {
            bool roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var Role = new IdentityRole
                {
                    Name = roleName
                };
                await _roleManager.CreateAsync(Role);
            }
        }

    }
}
