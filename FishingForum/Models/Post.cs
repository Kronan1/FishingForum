using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishingForum.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }

        
        [ForeignKey("Thread")]
        public Guid ThreadId { get; set; }
        public virtual Thread Thread { get; set; }


        [ForeignKey("FishingForumUser")]
        public int UserId { get; set; }
        public virtual Areas.Identity.Data.FishingForumUser CreatedBy { get; set; }


        public List<PostUserPicture> PostUserPictures { get; } = [];

    }
}
