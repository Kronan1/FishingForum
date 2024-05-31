using Elfie.Serialization;
using FishingForum.Data;
using FishingForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace FishingForum.DAL
{
    public class AdminManager : UserManager
    {
        public AdminManager(FishingForumContext context, ILogger<UserManager> logger) : base(context, logger)
        {

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


    }
}
