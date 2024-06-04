using Elfie.Serialization;
using FishingForum.Areas.Identity.Data;
using FishingForum.Data;
using FishingForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace FishingForum.DAL
{
    public class AdminManager : UserManager
    {
        private readonly UserManager<FishingForumUser> _userManagerIdentity;
        public AdminManager(FishingForumContext context, ILogger<UserManager> logger, HttpClient httpClient, UserManager<FishingForumUser> userManagerIdentity) : base(context, logger, httpClient)
        {
            _userManagerIdentity = userManagerIdentity;
        }

        public async Task AddCategory(Category category)
        {
            _context.Category.Add(category);
            _context.SaveChanges();
        }

        public async Task AddSubCategory(SubCategory subCategory)
        {
            _context.SubCategory.Add(subCategory);
            _context.SaveChanges();
        }

        public async Task UpdateSubCategory(SubCategory subCategory)
        {
            var existingSubCategory = await _context.SubCategory.SingleOrDefaultAsync(id => id.Id == subCategory.Id);
            if (existingSubCategory != null)
            {
                existingSubCategory.Name = subCategory.Name;
                existingSubCategory.CategoryId = subCategory.CategoryId;
                _context.SaveChanges();
            }
            
        }

        public async Task RemoveCategory(Category category)
        {
            _context.Category.Remove(category);
            _context.SaveChanges();
        }

        public async Task RemoveSubCategory(SubCategory subCategory)
        {
            _context.SubCategory.Remove(subCategory);
            _context.SaveChanges();
        }

        public async Task<List<Post>> GetReportedPostsAsync()
        {
            return await _context.Post.Where(p => p.Reported == true).ToListAsync();
        }

        public async Task RemovePost(Guid postId)
        {
            var post = await _context.Post.SingleOrDefaultAsync(p => p.Id == postId);
            if (post != null)
            {
                _context.Post.Remove(post);
                _context.SaveChanges();
            }
        }

        public async Task UpdateRoleOfUser(string userId, string roleId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                var role = await _context.Roles.FindAsync(roleId);

                if (role != null)
                {
                    // Remove user from all existing roles
                    var userRoles = await _userManagerIdentity.GetRolesAsync(user);
                    await _userManagerIdentity.RemoveFromRolesAsync(user, userRoles);

                    // Assign the user to the selected role
                    await _userManagerIdentity.AddToRoleAsync(user, role.Name);
                    await _context.SaveChangesAsync();
                }
            }
        }


    }
}
