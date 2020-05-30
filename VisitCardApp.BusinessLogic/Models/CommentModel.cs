namespace VisitCardApp.BusinessLogic.Models
{
    using System.ComponentModel.DataAnnotations;
    using VisitCardApp.DataAccess.Entities;

    public class CommentModel : IEntityModel<Comment>
    {
        public CommentModel() { }

        public CommentModel(Comment entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.Text = entity.Text;
                this.UserId = entity.UserId;
                this.ArticleId = entity.ArticleId;

                ArticleModel article = new ArticleModel();
                article.ToModel(entity.Article);
                this.Article = article;
            }
        }

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string UserName { get; set; }

        [Required]
        public int ArticleId { get; set; }

        public ArticleModel Article { get; set; }

        public string UserId { get; set; }


        public Comment ToEntity()
        {
            return new Comment
            {
                Id = this.Id,
                Text = this.Text,
                UserId = this.UserId,
                ArticleId = this.ArticleId,
                UserName = this.UserName,
                Article = this.Article?.ToEntity(),
            };
        }

        public void ToModel(Comment entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.Text = entity.Text;
                this.UserId = entity.UserId;
                this.ArticleId = entity.ArticleId;
                this.UserName = entity.UserName;

                if (Article == null)
                {
                    ArticleModel article = new ArticleModel();
                    article.ToModel(entity.Article);
                    this.Article = article;
                }
            }
        }
    }
}
