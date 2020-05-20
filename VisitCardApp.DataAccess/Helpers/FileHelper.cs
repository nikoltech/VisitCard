namespace VisitCardApp.DataAccess.Helpers
{
    public class FileHelper
    {
        public FileHelper()
        { }

        public FileHelper(string fileName, byte[] file)
        {
            this.FileName = fileName;
            this.File = file;
        }

        public string FileName { get; set; }

        public byte[] File { get; set; }
    }
}
