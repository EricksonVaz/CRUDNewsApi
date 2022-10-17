namespace CRUDNewsApi.Entities
{
    public class Likes
    {
        public int Id { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.Now;
        public Posts Post { get; set; }
        public User LikedBy { get; set; }

    }
}
