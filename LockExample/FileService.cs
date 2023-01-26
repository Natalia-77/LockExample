namespace LockExample
{
    internal static class FileService
    {
        private const string FolderName = "LockStatementApp";
        private const string Destination = "Destination";
        private static readonly object fileLock = new();

        public static void StartFileCopy()
        {
            var dirApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var destinationPath = Path.Combine(dirApp, Destination);
            var sourcePath = Path.Combine(dirApp, FolderName);
            if (!Directory.Exists(destinationPath))
            {
                _ = Directory.CreateDirectory(destinationPath);
            }
            _ = CopyFileAsync(sourcePath, destinationPath);
        }
        public static async Task CopyFileAsync(string source, string destination)
        {
            var fileEntries = Directory.GetFiles(source);
            var len = fileEntries.Length;
            var tasks = new Task[len - 1];
            for (var i = 0; i < len; i++)
            {
                Console.WriteLine(Path.GetFileName(fileEntries[i]));
                tasks[i] = Task.Run(() => CopyToDestination(source, destination, fileEntries[i]));
            }
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        public static void CopyToDestination(string source, string destination, string namefile)
        {
            lock (fileLock)
            {
                var currentFileName = Path.GetFileName(namefile);
                File.Copy(Path.Combine(source, currentFileName), Path.Combine(destination, currentFileName), true);
            }
        }
    }
}
