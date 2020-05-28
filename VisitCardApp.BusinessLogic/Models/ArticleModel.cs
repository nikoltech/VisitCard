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
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Topic { get; set; }

        public string Text { get; set; }

        public List<ArticleImageModel> ArticleImages { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Article ToEntity()
        {
            List<ArticleImage> articleImages = new List<ArticleImage>();
            foreach (ArticleImageModel imModel in ArticleImages)
            {
                articleImages.Add(imModel.ToEntity());
            }

            return new Article
            {
                Id = this.Id,
                Topic = this.Topic,
                Text = this.Text,
                ArticleImages = articleImages,
                CategoryId = this.CategoryId,
                Category = this.Category
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
            }
        }
    }
}
