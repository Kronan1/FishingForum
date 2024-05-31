using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishingForum.Models
{
    public class Thread
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }


        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }


        [ForeignKey("FishingForumUser")]
        public string UserId { get; set; }
        public virtual Areas.Identity.Data.FishingForumUser FishingForumUser { get; set; }

        public DateTime DateCreated { get; set; }

    }
}
