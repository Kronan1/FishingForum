using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FishingForum.Models
{
    public class ProfilePicture
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        [StringLength(40)]
        public string FileName { get; set; }


        [StringLength(10)]
        public string FileType { get; set; }


        [ForeignKey("FishingForumUser")]
        public string UserId { get; set; }
        public virtual Areas.Identity.Data.FishingForumUser FishingForumUser { get; set; }
    }
}
