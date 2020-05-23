namespace VisitCardApp.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using VisitCardApp.BusinessLogic.Models;

    // TODO: work with files. Remove reading bytes!!!
    public class ArticleViewModel
    {
        public ArticleViewModel() { }

        public ArticleViewModel(ArticleModel articleModel)
        {
            this.Id = articleModel.Id;
            this.Topic = articleModel.Topic;
            this.Text = articleModel.Text;
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage = "Не более 200 символов")]
        public string Topic { get; set; }

        [Required]
        public string Text { get; set; }

        /// <summary>
        /// filename, image file byte array
        /// </summary>
        public FormFileCollection Files { get; set; }

        public ArticleModel ToAppModel()
        {
            return new ArticleModel
            {
                Id = this.Id,
                Topic = this.Topic,
                Text = this.Text
            };
        }
    }
}
