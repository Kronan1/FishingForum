using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;


namespace FishingForum.Pages
{
    public class EditSubCategoryModel : PageModel
    {
        private readonly AdminManager _adminManager;


        public SubCategory SubCategory { get; set; }

        [BindProperty]
        public SubCategory UpdatedSubCategory { get; set; }

        public Category Category { get; set; }

        public SelectList CategorySelectList { get; set; }

        public List<Category> Categories { get; set; }

        public string ErrorMessage { get; set; }


        public EditSubCategoryModel(AdminManager adminManager)
        {
            _adminManager = adminManager;
        }

        public async Task<IActionResult> OnGetAsync(int subCategoryId)
        {
            await Initialize(subCategoryId);

            if (SubCategory == null)
            {
                return NotFound();
            }

            UpdatedSubCategory = new SubCategory
            {
                Id = SubCategory.Id,
                Name = SubCategory.Name,
                CategoryId = SubCategory.CategoryId
            };

            CategorySelectList = new SelectList(Categories, "Id", "Name", SubCategory.CategoryId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            await Initialize(UpdatedSubCategory.Id);


            // WORKAROUND Remove the ModelState entry for Category to bypass its validation
            ModelState.Remove("UpdatedSubCategory.Category");

            if (!ModelState.IsValid)
            {

                if (UpdatedSubCategory.Name.IsNullOrEmpty())
                {
                    // TODO Reassign original name if name is Invalid
                    SubCategory = await _adminManager.GetSubCategoryAsync(UpdatedSubCategory.Id);
                    UpdatedSubCategory.Name = SubCategory.Name;
                    ErrorMessage = "Name field was empty";
                }

                Categories = await _adminManager.GetCategoriesAsync();
                CategorySelectList = new SelectList(Categories, "Id", "Name", UpdatedSubCategory.CategoryId);
                return Page();
            }

            if (!UpdatedSubCategory.Name.IsOnlyLetters())
            {
                // TODO Reassign original name if name is Invalid
                ErrorMessage = "Only letters allowed in name";
                SubCategory = await _adminManager.GetSubCategoryAsync(UpdatedSubCategory.Id);
                UpdatedSubCategory.Name = SubCategory.Name;
                Categories = await _adminManager.GetCategoriesAsync();
                CategorySelectList = new SelectList(Categories, "Id", "Name", UpdatedSubCategory.CategoryId);
                return Page();
            }

            var id = UpdatedSubCategory.Id;

            await _adminManager.UpdateSubCategory(UpdatedSubCategory);

            return RedirectToPage("/AdminPages/EditCategory", new { id = UpdatedSubCategory.CategoryId });

        }


        private async Task Initialize(int subCategoryId)
        {
            SubCategory = await _adminManager.GetSubCategoryAsync(subCategoryId);
            Category = await _adminManager.GetCategoryAsync(SubCategory.CategoryId);
            Categories = await _adminManager.GetCategoriesAsync();
        }
    }
}
