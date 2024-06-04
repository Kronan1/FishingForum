using Azure;
using FishingForum.Areas.Identity.Data;
using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data;

namespace FishingForum.Pages.AdminPages
{
    //[Authorize(Roles = "Admin")]
    public class RolePageModel : PageModel
    {
        private readonly UserManager<Areas.Identity.Data.FishingForumUser> _userManagerIdentity;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AdminManager _adminManager;
        public IList<UserRoleViewModel> UserRoles { get; set; }
        public List<IdentityRole> Roles { get; set; }


        [BindProperty]
        public string RoleName { get; set; }



        public RolePageModel(RoleManager<IdentityRole> roleManager, UserManager<Areas.Identity.Data.FishingForumUser> userManagerIdentity, AdminManager adminManager)
        {
            _userManagerIdentity = userManagerIdentity;
            _roleManager = roleManager;
            _adminManager = adminManager;
        }

        public async Task OnGetAsync()
        {
            
            Roles = await _roleManager.Roles.ToListAsync();

            var Users = await _userManagerIdentity.Users.ToListAsync();
            UserRoles = new List<UserRoleViewModel>();

            foreach (var user in Users)
            {
                var rolesForUser = await _userManagerIdentity.GetRolesAsync(user);
                var role = rolesForUser.FirstOrDefault();
                UserRoles.Add(new UserRoleViewModel

                {
                    Alias = user.Alias,
                    Role = role,
                    Id = user.Id
                });
            }

            //foreach (var user in UserRoles)
            //{

            //    foreach (var role in Roles)
            //    {
            //        var test = user.Roles.Contains(role.Id) ? "checked" : "";
            //    }
            //}
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (RoleName != null)
            {
                await CreateRoleAsync(RoleName);
            }
            return Page();
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

        public async Task<IActionResult> OnPostUpdateRole(string userId, string roleId)
        {
            await _adminManager.UpdateRoleOfUser(userId, roleId);

            // Here you need to implement the logic to update the user's role based on the userId and roleId.
            // You can use the provided userId and roleId to update the user's role in your data store.

            // After updating the user's role, you can redirect the user to another page or refresh the current page.

            return RedirectToPage("/AdminPages/RolePage");
        }
    }
}
