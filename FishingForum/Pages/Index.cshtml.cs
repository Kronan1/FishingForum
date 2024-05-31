using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FishingForum.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager _userManager;
        public List<Category> Categories { get; set; }
        public List<SubCategory> SubCategories { get; set; }

        public IndexModel(UserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int subCategoryId)
        {
            if (subCategoryId != 0)
            {
                return RedirectToPage("/SubCategoryPage", new { subCategoryId = subCategoryId });
            }

            await Initialize();

            return Page();
        }

        private async Task Initialize()
        {

            Categories ??= await _userManager.GetCategoriesAsync();
            SubCategories = new List<SubCategory>();

            foreach (var category in Categories)
            {
                var subCategories = await _userManager.GetSubCategoriesAsync(category.Id);
                SubCategories.AddRange(subCategories);
            }
            
        }
    }
}
