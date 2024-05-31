using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace FishingForum.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public string Text { get; set; }

        public string SenderId { get; set; }

        public string RecieverId { get; set; }

    }
}
