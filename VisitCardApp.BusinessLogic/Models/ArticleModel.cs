namespace VisitCardApp.BusinessLogic.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using VisitCardApp.DataAccess.Entities;
    using VisitCardApp.DataAccess.Helpers;

    public class ArticleModel : IEntityModel<Article>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Topic { get; set; }

        public string Text { get; set; }

        /// <summary>
        /// filename, image file byte array
        /// </summary>
        public List<FileHelper> ArticleImages { get; set; }

        public Article ToEntity()
        {
            return new Article
            {
                Id = this.Id,
                Topic = this.Topic,
                Text = this.Text,
                ArticleImages = this.ArticleImages
            };
        }

        public void ToModel(Article entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.Topic = entity.Topic;
                this.Text = entity.Text;
                this.ArticleImages = entity.ArticleImages;
            }
        }
    }
}
