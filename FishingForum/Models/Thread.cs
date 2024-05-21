using System.ComponentModel.DataAnnotations.Schema;

namespace FishingForum.Models
{
    public class Thread
    {
        public Guid Id { get; set; }
        public string Title { get; set; }


        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; } = new();


        [ForeignKey("FishingForumUser")]
        public int UserId { get; set; }
        public virtual Areas.Identity.Data.FishingForumUser CreatedBy { get; set; } = new();

        public DateTime DateCreated { get; set; }
    }
}
