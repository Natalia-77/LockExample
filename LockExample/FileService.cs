using System.Diagnostics;
using LockExample.Utils;

namespace LockExample
{
    internal static class FileService
    {
        private const string FolderName = "LockStatementApp";
        private const string Destination = "Destination";
        public static async Task StartFileCopy()
        {
            var dirApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var destinationPath = Path.Combine(dirApp, Destination);
            var sourcePath = Path.Combine(dirApp, FolderName);
            DirectoryUtil.EnsureDirectory(destinationPath);
            await CopyFileAsync(sourcePath, destinationPath).ConfigureAwait(false);
        }
        public static async Task CopyFileAsync(string source, string destination)
        {
            var fileEntries = Directory.GetFiles(source);
            var tasks = new List<Task<ResultInfo>>();
            foreach (var item in fileEntries)
            {
                var task = Task.Run(() => CopyToDestination(source, destination, item));
                tasks.Add(task);
            }
            _ = await Task.WhenAll(tasks).ConfigureAwait(true);
            foreach (Task<ResultInfo> item in tasks)
            {
                ResultInfo resultItem = item.GetAwaiter().GetResult();
                var writeresult = new WriteResult();
                await writeresult.Write(resultItem).ConfigureAwait(false);
            }
        }
        public static Task<ResultInfo> CopyToDestination(string source, string destination, string namefile)
        {
            var stopWatch = new Stopwatch();
            var currentFileName = Path.GetFileName(namefile);
            var sourcePath = Path.Combine(source, currentFileName);
            var destinationPath = Path.Combine(destination, currentFileName);
            stopWatch.Start();
            File.Copy(sourcePath, destinationPath, true);
            stopWatch.Stop();
            TimeSpan timeCopyFile = stopWatch.Elapsed;
            //Console.WriteLine($"File {currentFileName} RunTime {Math.Round(timeCopyFile.TotalSeconds * 1000, 2)}");
            ResultInfo fileInfo = FileResultCopyInfo.GetResultInfo(source, currentFileName, timeCopyFile);
            return Task.FromResult(fileInfo);
        }
    }
}
