using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;

namespace FishingForum.Pages
{
    public class EditCategoryModel : PageModel
    {
        private readonly AdminManager _adminManager;

        public Category Category { get; set; }
        public List<SubCategory> SubCategories { get; set; }


        [BindProperty]
        public SubCategory NewSubCategory { get; set; }




        public EditCategoryModel(AdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        public async Task<IActionResult> OnGetAsync(int id, int editSubCategoryId, int deleteSubCategoryId)
        {
            if (editSubCategoryId != 0)
            {
                var subCategory = await _adminManager.GetSubCategoryAsync(editSubCategoryId);
                if (subCategory == null)
                {
                    throw new ArgumentNullException(nameof(subCategory));
                }

                return RedirectToPage("/AdminPages/EditSubCategory", new { subCategoryId = subCategory.Id });


            }

            if (deleteSubCategoryId != 0)
            {
                var subCategory = await _adminManager.GetSubCategoryAsync(deleteSubCategoryId);
                var categoryId = subCategory.CategoryId;

                if (categoryId == null)
                {
                    return NotFound("Category not found");
                }

                if (subCategory != null)
                {
                    await _adminManager.RemoveSubCategory(subCategory);
                }

                id = categoryId;
            }


            await Initialize(id);

            return Page();
        }



        public async Task<IActionResult> OnPostAsync(int id)
        {

            NewSubCategory.CategoryId = id;


            // WORKAROUND Remove the ModelState entry for Category to bypass its validation
            ModelState.Remove("NewSubCategory.Category");


            if (ModelState.IsValid)
            {
                await _adminManager.AddSubCategory(NewSubCategory);

            }

            

            return RedirectToPage("/AdminPages/EditCategory", new { id = NewSubCategory.CategoryId });
        }


        private async Task Initialize(int id)
        {
            Category = await _adminManager.GetCategoryAsync(id);

            if (Category == null)
            {
                throw new ArgumentNullException(nameof(Category));
            }

            SubCategories = await _adminManager.GetSubCategoriesAsync(id);

            if (SubCategories == null)
            {
                throw new ArgumentNullException(nameof(SubCategories));
            }
        }

    }
}
