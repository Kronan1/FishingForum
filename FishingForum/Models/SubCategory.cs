using System.ComponentModel.DataAnnotations.Schema;

namespace FishingForum.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = new();
    }
}
