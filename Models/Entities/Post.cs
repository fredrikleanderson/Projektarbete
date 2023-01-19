namespace Models.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual IEnumerable<Like> Likes { get; set; } = null!;

    }
}
