using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishingForum.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public string Text { get; set; }

        
        [ForeignKey("Thread")]
        public Guid ThreadId { get; set; }
        public virtual Thread Thread { get; set; }


        [ForeignKey("FishingForumUser")]
        public string UserId { get; set; }
        public virtual Areas.Identity.Data.FishingForumUser FishingForumUser { get; set; }


        public List<PostUserPicture> PostUserPictures { get; }

        public bool Reported { get; set; } = false;

    }
}
