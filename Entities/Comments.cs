namespace CRUDNewsApi.Entities
{
    public class Comments
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CommentedAt { get; set; } = DateTime.Now;
        public EStatus Status { get; set; }
        public Posts Post { get; set; }
        public User CommentedBy { get; set; }
    }
}
