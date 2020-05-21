namespace VisitCardApp.DataAccess.Helpers
{
    public class FileHelper
    {
        public FileHelper()
        { }

        public FileHelper(string fileName, byte[] file, string imageMimeType)
        {
            this.FileName = fileName;
            this.File = file;
            this.ImageMimeType = imageMimeType;
        }

        public string FileName { get; set; }

        public string ImageMimeType { get; set; }

        public byte[] File { get; set; }
    }
}
