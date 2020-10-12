namespace VisitCardApp.BusinessLogic.Models
{
    using VisitCardApp.DataAccess.Entities;
    
    public class ArticleImageModel : IEntityModel<ArticleImage>
    {
        public ArticleImageModel() { }

        public ArticleImageModel(ArticleImage entity) 
        {
            this.SetFieldsFromEntity(entity);

            ArticleModel articleModel = new ArticleModel();
            articleModel.ToModel(entity.Article);
        }

        public ArticleImageModel(ArticleImage entity, ArticleModel articleModel)
        {
            this.SetFieldsFromEntity(entity);

            if (articleModel != null && articleModel.Id == entity.Article?.Id)
            {
                this.Article = articleModel;
                this.ArticleId = articleModel.Id;
            }
            else
            {
                ArticleModel articleModelFromEntity = new ArticleModel();
                articleModelFromEntity.ToModel(entity.Article);
                this.ArticleId = articleModelFromEntity.Id;
            }
        }

        //public ArticleImageModel(string filename, byte[] file, string imageMimeType, string urlPath)
        //{
        //    this.FileName = filename;
        //    this.File = file;
        //    this.ImageMimeType = imageMimeType;
        //    this.UrlPath = urlPath;
        //}

        public int Id { get; set; }

        public string UrlPath { get; set; }

        public string ImageMimeType { get; set; }

        public string FileName { get; set; }

        public byte[] File { get; set; }

        public int ArticleId { get; set; }

        public ArticleModel Article { get; set; }

        public ArticleImage ToEntity()
        {
            return new ArticleImage
            {
                Id = this.Id,
                UrlPath = this.UrlPath,
                ImageMimeType = this.ImageMimeType,
                File = this.File,
                ArticleId = this.ArticleId
                // FileName = this.FileName // Security NOTE: Do not get from user
            };
        }

        public void ToModel(ArticleImage entity)
        {
            this.SetFieldsFromEntity(entity);
        }

        private void SetFieldsFromEntity(ArticleImage entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.UrlPath = entity.UrlPath;
                this.ImageMimeType = entity.ImageMimeType;
                // this.FileName = entity.FileName; // Security NOTE: Do not give to user
            }
        }
    }
}
