namespace CRUDNewsApi.Entities
{
    public class Posts
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public string Tags { get; set; }
        public EStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public User PostedBy { get; set; }
        public List<Comments> Comments { get; set; }
        public List<Likes> Likes { get; set; }
    }
}
