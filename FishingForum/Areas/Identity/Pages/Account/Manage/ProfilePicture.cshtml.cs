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
        private readonly IWebHostEnvironment _environment;

        [Required]
        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public bool UploadSuccess { get; set; }

        public string ImgUrl { get; set; }

        public ProfilePictureModel(IWebHostEnvironment environment, UserManager<FishingForumUser> userManagerIdentity, DAL.UserManager userManager)
        {
            _userManagerIdentity = userManagerIdentity;
            _userManager = userManager;
            _environment = environment;

        }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            // Check file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Image not found");
                return Page();
            }

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

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            var filePath = Path.Combine(uploadsFolder, profilePicture.FileName);

            var filePathToSave = "/uploads/" + profilePicture.FileName;

            profilePicture.FilePath = filePathToSave;

            await _userManager.UploadProfilePictureAsync(profilePicture, userId, uploadsFolder);

            

            ScaleImage(UploadedFile, filePath);

            var profilePicturePath = await _userManager.GetProfilePicturePathAsync(userId);

            if (string.IsNullOrEmpty(profilePicturePath))
            {
                ModelState.AddModelError(string.Empty, "Could not retrieve Profile Picture");
                return Page();

            }

            ImgUrl = profilePicturePath;

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
            using (var image = SixLabors.ImageSharp.Image.Load(UploadedFile.OpenReadStream()))
            {
                int maxWidth = 150;
                int maxHeight = 150;
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

                        using (var outputStream = System.IO.File.OpenWrite(filePath))
                        {
                            image.Save(outputStream, encoder);
                        }
                }
            }
          
        }

    }
}
