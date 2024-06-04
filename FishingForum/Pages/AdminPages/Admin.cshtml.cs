using FishingForum.DAL;
using FishingForum.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FishingForum.Pages.AdminPages
{
    //[Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        private readonly AdminManager _adminManager;

        public List<Category> Categories { get; set; }


        [BindProperty]
        public Category NewCategory { get; set; }



        public AdminModel(AdminManager adminManager)
        {
            _adminManager = adminManager;
        }


        public async Task<IActionResult> OnGetAsync(int editCategoryId, int deleteCategoryId, bool seeReported, bool roleManager)
        {
            await Initialize();

            if (editCategoryId != 0)
            {
                
                var categoryToEdit = Categories.FirstOrDefault(c => c.Id == editCategoryId);
                if (categoryToEdit != null)
                {
                    // Redirect to EditCategory pass CategoryId
                    return RedirectToPage("/AdminPages/EditCategory", new { id = categoryToEdit.Id });
                }
            }


            if (deleteCategoryId != 0)
            {
                var categoryToDelete = await _adminManager.GetCategoryAsync(deleteCategoryId);
                if (categoryToDelete != null)
                {
                    await _adminManager.RemoveCategory(categoryToDelete);
                }
            }

            if (seeReported)
            {
                return RedirectToPage("/AdminPages/AdminReportedMessages");
            }

            if (roleManager)
            {
                return RedirectToPage("/AdminPages/RolePage");
            }

            await Initialize();

            // TODO Category is not removed from list unless page is refreshed
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _adminManager.AddCategory(NewCategory);
            }

            return Redirect("/AdminPages/Admin");
        }



        private async Task Initialize()
        {
            Categories = await _adminManager.GetCategoriesAsync();
        }


    }
}
