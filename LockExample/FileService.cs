using System.Diagnostics;
namespace LockExample
{
    internal static class FileService
    {
        private const string FolderName = "LockStatementApp";
        private const string Destination = "Destination";
        // private static readonly object fileLock = new();

        public static async Task StartFileCopy()
        {
            var dirApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var destinationPath = Path.Combine(dirApp, Destination);
            var sourcePath = Path.Combine(dirApp, FolderName);
            if (!Directory.Exists(destinationPath))
            {
                _ = Directory.CreateDirectory(destinationPath);
            }
            await CopyFileAsync(sourcePath, destinationPath).ConfigureAwait(false);
        }
        public static async Task CopyFileAsync(string source, string destination)
        {
            var fileEntries = Directory.GetFiles(source);
            var tasks = new List<Task>();
            foreach (var item in fileEntries)
            {
                var task = Task.Run(() => CopyToDestination(source, destination, item));
                tasks.Add(task);
            }
            await Task.WhenAll(tasks).ConfigureAwait(true);
            //foreach (Task item in tasks)
            //{
            //    Console.WriteLine(item.Status);
            //}
        }
        public static void CopyToDestination(string source, string destination, string namefile)
        {
            var stopWatch = new Stopwatch();
            var currentFileName = Path.GetFileName(namefile);
            stopWatch.Start();
            File.Copy(Path.Combine(source, currentFileName), Path.Combine(destination, currentFileName), true);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine($"File {currentFileName} nRunTime {Math.Round(ts.TotalSeconds * 1000, 2)}");
        }
    }
}
