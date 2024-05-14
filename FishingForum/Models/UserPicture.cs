using Azure;
using System.ComponentModel.DataAnnotations;

namespace FishingForum.Models
{
    public class UserPicture
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        [StringLength(100)]
        public string Name { get; set; }


        [StringLength(100)]
        public string Description { get; set; }


        [StringLength(36)]
        public string FileName { get; set; }


        [StringLength(10)]
        public string FileType { get; set; }


        public List<PostUserPicture> PostUserPictures { get; } = [];

    }
}
