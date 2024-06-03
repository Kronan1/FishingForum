using FishingForum.Data;
using FishingForum.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace FishingForum.DAL
{
    public class UserManager
    {
        protected readonly FishingForumContext _context;
        protected readonly ILogger<UserManager> _logger;
        private readonly HttpClient _httpClient;


        public UserManager(Data.FishingForumContext context, ILogger<UserManager> logger, HttpClient httpClient)
        {
            _context = context;
            _logger = logger;
            _httpClient = httpClient;
        }


        public async Task<List<Category>> GetCategoriesAsync()
        {
            // Get ALL Categories
            if (_context.Database.CanConnect())
            {
                var categories = await _context.Category.ToListAsync();
                return categories;
            }
            else
            {
                // Handle case where database connection is not available
                throw new Exception("Database connection is not available.");
            }
        }

        public async Task<List<Category>> GetCategoriesFromApiAsync()
        {
            List<Category> Data = new();
            var apiUrl = "https://fishingforumapi.azurewebsites.net/api/Category";
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<List<Category>>(json);
            }

            return Data;
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            // Get ONE category
            if (_context.Database.CanConnect())
            {
                var category = await _context.Category.SingleOrDefaultAsync(c => c.Id == id);
                return category;
            }
            else
            {
                // Handle case where database connection is not available
                throw new Exception("Database connection is not available.");
            }
        }


        public async Task<List<SubCategory>> GetSubCategoriesAsync(int categoryId)
        {
            // Get ALL subcategories from one category
            if (_context.Database.CanConnect())
            {
                var subCategories = await _context.SubCategory.Where(s => s.CategoryId == categoryId).ToListAsync();
                return subCategories;
            }
            else
            {
                // Handle case where database connection is not available
                throw new Exception("Database connection is not available.");
            }
        }

        public async Task<SubCategory> GetSubCategoryAsync(int id)
        {
            // Get ONE subcategory
            return await _context.SubCategory.SingleOrDefaultAsync(s => s.Id == id);

        }


        public async Task<List<Models.Thread>> GetThreadsAsync(int subCategoryId)
        {
            // Get ALL threads from one subcategory
            return await _context.Thread.Where(t => t.SubCategoryId == subCategoryId).ToListAsync();

        }

        public async Task CreateThreadAsync(Models.Thread thread)
        {
            _context.Thread.Add(thread);
            await _context.SaveChangesAsync();
        }

        public async Task<Models.Thread> GetThreadAsync(string title, int subcategoryId)
        {
            // Get thread from title and subcategory
            return await _context.Thread.SingleOrDefaultAsync(t => t.Title == title && t.SubCategoryId == subcategoryId);
        }
        public async Task<Models.Thread> GetThreadAsync(Guid threadId)
        {
            // Get thread from id
            return await _context.Thread.SingleOrDefaultAsync(t => t.Id == threadId);
        }

        public async Task CreatePostAsync(Post post)
        {
            List<string> badWords = new List<string>()
            {
                "anal",
                "anus",
                "arse",
                "ass",
                "ballsack",
                "balls",
                "bastard",
                "bitch",
                "biatch",
                "bloody",
                "blowjob",
                "blow job",
                "bollock",
                "bollok",
                "boner",
                "boob",
                "bugger",
                "bum",
                "butt",
                "buttplug",
                "clitoris",
                "cock",
                "coon",
                "crap",
                "cunt",
                "damn",
                "dick",
                "dildo",
                "dyke",
                "fag",
                "feck",
                "fellate",
                "fellatio",
                "felching",
                "fuck",
                "f u c k",
                "fudgepacker",
                "fudge packer",
                "flange",
                "Goddamn",
                "God damn",
                "hell",
                "homo",
                "jerk",
                "jizz",
                "knobend",
                "knob end",
                "labia",
                "lmao",
                "lmfao",
                "muff",
                "nigger",
                "nigga",
                "omg",
                "penis",
                "piss",
                "poop",
                "prick",
                "pube",
                "pussy",
                "queer",
                "scrotum",
                "sex",
                "shit",
                "s hit",
                "sh1t",
                "slut",
                "smegma",
                "spunk",
                "tit",
                "tosser",
                "turd",
                "twat",
                "vagina",
                "wank",
                "whore",
                "wtf",
                "hora",
                "fitta"
            };

            // Create a regular expression pattern to match any of the bad words
            string pattern = @"\b(" + string.Join("|", badWords.Select(Regex.Escape).ToArray()) + @")\b";

            var input = post.Text;

            // Replace bad words with asterisks
            post.Text = Regex.Replace(input, pattern, match => new string('*', match.Length), RegexOptions.IgnoreCase);

            _context.Post.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Models.Post>> GetPostsAsync(Guid threadId)
        {
            return await _context.Post.Where(p => p.ThreadId == threadId).ToListAsync();
        }


        public async Task<Post> GetPostAsync(Guid postId)
        {
            return await _context.Post.SingleOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<List<AnonymizedUser>> GetAnonymizedUsersAsync(List<string> userIdList)
        {
            List<AnonymizedUser> users = new();

            foreach (var userId in userIdList)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);


                if (user != null)
                {

                    var profilePicture = await _context.ProfilePicture.SingleOrDefaultAsync(u => u.UserId == userId);
                    string profilePicureFilepath = string.Empty;

                    if (profilePicture != null)
                    {

                        profilePicureFilepath = profilePicture.FilePath;

                    }

                    users.Add(new AnonymizedUser { Id = user.Id, Alias = user.Alias, ProfilePicture = profilePicureFilepath });
                }
            }

            return users;
        }

        public async Task<AnonymizedUser> GetAnonymizedUserAsync(string userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

            var profilePicture = await _context.ProfilePicture.SingleOrDefaultAsync(u => u.UserId == userId);
            string profilePicureFilepath = string.Empty;

            if (profilePicture != null)
            {

                profilePicureFilepath = profilePicture.FilePath;

            }

            return new AnonymizedUser { Id = user.Id, Alias = user.Alias, ProfilePicture = profilePicureFilepath };

        }

        public async Task<List<AnonymizedUser>> GetAnonymizedUsersAsync()
        {
            var userList = await _context.Users.ToListAsync();

            List<AnonymizedUser> anonUsers = new();

            foreach (var user in userList)
            {
                anonUsers.Add(new AnonymizedUser { Id = user.Id, Alias = user.Alias });
            }

            return anonUsers;
        }

        public async Task ReportPostAsync(Post post)
        {
            post.Reported = true;
            await _context.SaveChangesAsync();
        }

        public async Task SendMessageAsync(Message message)
        {
            if (message != null)
            {
                _context.Message.Add(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Message>> GetMessagesAsync(string userId)
        {
            return await _context.Message.Where(m => m.RecieverId == userId).ToListAsync();
        }

        public async Task DeleteMessage(Guid messageId)
        {
            var message = await _context.Message.SingleOrDefaultAsync(m => m.Id == messageId);
            if (message != null)
            {
                _context.Message.Remove(message);
                _context.SaveChanges();
            }
        }

        public async Task UploadProfilePictureAsync(ProfilePicture profilePicture, string userId, string uploadsFolder)
        {
            var existingProfilePicture = await _context.ProfilePicture.FirstOrDefaultAsync(u => u.UserId == userId);

            if (existingProfilePicture != null)
            {
                // If a profile picture already exists
                var filePath = Path.Combine(uploadsFolder, existingProfilePicture.FileName);

                _context.Remove(existingProfilePicture);
                if (File.Exists(filePath))
                {
                    // Delete the file
                    File.Delete(filePath);
                }
            }

            _context.ProfilePicture.Add(profilePicture);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetProfilePicturePathAsync(string userId)
        {
            var profilePicture = await _context.ProfilePicture.FirstOrDefaultAsync(u => u.UserId == userId);

            if (profilePicture == null || string.IsNullOrEmpty(profilePicture.FilePath))
            {
                return null; // Handle the case where the profile picture is not found
            }

            return profilePicture.FilePath;
        }
    }
}
