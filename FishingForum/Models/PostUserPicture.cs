namespace FishingForum.Models
{
    public class PostUserPicture
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }



        public Guid UserPictureId { get; set; }
        public UserPicture UserPicture { get; set; }
    }
}
