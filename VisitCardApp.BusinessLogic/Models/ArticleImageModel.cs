namespace VisitCardApp.BusinessLogic.Models
{
    using VisitCardApp.DataAccess.Entities;
    
    public class ArticleImageModel : IEntityModel<ArticleImage>
    {
        public ArticleImageModel() { }

        public ArticleImageModel(ArticleImage entity) 
        {
            this.SetFieldsFromEntity(entity);
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
                ArticleId = this.ArticleId,
                FileName = this.FileName
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
                this.ArticleId = entity.ArticleId;
                this.FileName = entity.FileName;
            }
        }
    }
}
