using System.ComponentModel.DataAnnotations.Schema;

namespace FishingForum.Models
{
    public class PostInteraction
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }

        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
