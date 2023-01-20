namespace LockExample
{
    internal static class FileService
    {
        private const string FileName = "first.txt";
        private const string FolderName = "LockStatementApp";

        public static string GetPathDirectory()
        {
            var dirApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var locationFile = Path.Combine(dirApp, FolderName, FileName);
            var dir = Path.GetDirectoryName(locationFile);
            if (!Directory.Exists(dir))
            {
                _ = Directory.CreateDirectory(locationFile);
            }
            return locationFile;
        }
    }
}
