namespace VisitCardApp.BusinessLogic.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using VisitCardApp.DataAccess.Entities;

    public class ArticleModel : IEntityModel<Article>
    {
        public ArticleModel()
        {
            this.ArticleImages = new List<ArticleImageModel>();
            this.Comments = new List<CommentModel>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Topic { get; set; }

        public string Text { get; set; }

        public List<ArticleImageModel> ArticleImages { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public List<CommentModel> Comments { get; set; }

        public string UserId { get; set; }

        public Article ToEntity()
        {
            List<ArticleImage> articleImages = new List<ArticleImage>();
            foreach (ArticleImageModel imModel in this.ArticleImages)
            {
                articleImages.Add(imModel.ToEntity());
            }

            List<Comment> comments = new List<Comment>();
            foreach (CommentModel comModel in this.Comments)
            {
                comments.Add(comModel.ToEntity());
            }

            return new Article
            {
                Id = this.Id,
                Topic = this.Topic,
                Text = this.Text,
                ArticleImages = articleImages,
                CategoryId = this.CategoryId,
                Category = this.Category,
                UserId = this.UserId,
                Comments = comments
            };
        }

        public void ToModel(Article entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.Topic = entity.Topic;
                this.Text = entity.Text;
                this.CategoryId = entity.CategoryId;
                this.Category = entity.Category;

                List<ArticleImageModel> articleImages = new List<ArticleImageModel>();
                if (entity.ArticleImages != null)
                {
                    foreach (ArticleImage im in entity.ArticleImages)
                    {
                        articleImages.Add(new ArticleImageModel(im));
                    }
                }

                this.ArticleImages = articleImages;

                List<CommentModel> comments = new List<CommentModel>();
                if (entity.Comments != null)
                {
                    foreach (Comment c in entity.Comments)
                    {
                        CommentModel commentModel = new CommentModel { Article = this };
                        commentModel.ToModel(c);
                        comments.Add(commentModel);
                    }
                }

                this.Comments = comments;
            }
        }
    }
}
