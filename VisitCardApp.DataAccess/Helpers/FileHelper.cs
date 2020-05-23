namespace VisitCardApp.DataAccess.Helpers
{
    public class FileHelper
    {
        public FileHelper()
        { }

        public FileHelper(string fileName, byte[] file, string imageMimeType, string urlPath)
        {
            this.FileName = fileName;
            this.File = file;
            this.ImageMimeType = imageMimeType;
            this.UrlPath = urlPath;
        }

        public string FileName { get; set; }

        public string ImageMimeType { get; set; }

        public string UrlPath { get; set; }

        public byte[] File { get; set; }
    }
}
