using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using FishingForum.Areas.Identity.Data;
using FishingForum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FishingForum.Areas.Identity.Pages.Account.Manage
{
    public class ProfilePictureModel : PageModel
    {
        private readonly UserManager<FishingForumUser> _userManagerIdentity;
        private readonly DAL.UserManager _userManager;

        [Required]
        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public bool UploadSuccess { get; set; }

        public ProfilePictureModel(UserManager<FishingForumUser> userManagerIdentity, DAL.UserManager userManager)
        {
            _userManagerIdentity = userManagerIdentity;
            _userManager = userManager;

        }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // Check file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(UploadedFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError(string.Empty, "The file must be a JPG, JPEG, PNG, or GIF image.");
                return Page();
            }

            // Check content type
            if (!UploadedFile.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError(string.Empty, "The file must be an image.");
                return Page();
            }

            // Check file header
            if (!IsImage(UploadedFile))
            {
                ModelState.AddModelError(string.Empty, "The file is not a valid image.");
                return Page();
            }

            var id = Guid.NewGuid();
            var userId =  _userManagerIdentity.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "User does not exist");
                return Page();
            }

            int width;
            int height;
            using (var image = System.Drawing.Image.FromStream(UploadedFile.OpenReadStream()))
            {

                width = image.Width;
                height = image.Height;

            }

            var profilePicture = new ProfilePicture { FileName = id + extension,
            FileType = extension,
            UserId = userId,
            Id = id,
            Height = height,
            Width = width};

            await _userManager.UploadProfilePictureAsync(profilePicture);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            var filePath = Path.Combine(uploadsFolder, profilePicture.FileName);

            ScaleImage(UploadedFile, filePath);

            //using (var fileStream = new FileStream(filePath, FileMode.Create))
            //{
            //    await UploadedFile.CopyToAsync(fileStream);
            //}





            UploadSuccess = true;

            return Page();
        }

        private bool IsImage(IFormFile file)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    var bytes = stream.ToArray();
                    var bmp = System.Drawing.Image.FromStream(stream) as System.Drawing.Bitmap;
                    return bmp != null;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ScaleImage(IFormFile imageFile, string filePath)
        {
            FormFile formFileToReturn;
            using (var image = SixLabors.ImageSharp.Image.Load(UploadedFile.OpenReadStream()))
            {
                int maxWidth = 100;
                int maxHeight = 100;
                var newWidth = image.Width;
                var newHeight = image.Height;

                if (newWidth > maxWidth)
                {
                    newWidth = maxWidth;
                    newHeight = (int)(((double)image.Height / image.Width) * maxWidth);
                }

                if (newHeight > maxHeight)
                {
                    newHeight = maxHeight;
                    newWidth = (int)(((double)image.Width / image.Height) * maxHeight);
                }

                // Resize the image
                image.Mutate(x => x.Resize(newWidth, newHeight));

                using (var memoryStream = new MemoryStream())
                {
                    var encoder = new JpegEncoder
                    {
                        Quality = 90 // Adjust quality as needed
                    };

                    image.Save(memoryStream, encoder);

                    //// Create a new MemoryStream from the resized image data
                    //using (var resizedMemoryStream = new MemoryStream(memoryStream.ToArray()))
                    //{
                    //    // Reset the position of the memory stream to the beginning
                    //    resizedMemoryStream.Seek(0, SeekOrigin.Begin);

                    //    // Create a new FormFile object from the resized memory stream
                    //    formFileToReturn = new FormFile(resizedMemoryStream, 0, resizedMemoryStream.Length, "ResizedImage", UploadedFile.FileName);

                        using (var outputStream = System.IO.File.OpenWrite(filePath))
                        {
                            image.Save(outputStream, encoder);
                        }
                    //}
                }
            }
          
        }

    }
}
