namespace VisitCardApp.DataAccess.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string UserName { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }
    }
}
