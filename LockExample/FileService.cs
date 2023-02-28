using System.Diagnostics;
using System.Runtime.CompilerServices;
using LockExample.Utils;
[assembly: InternalsVisibleTo("LockExample.UnitTests")]
namespace LockExample
{
    internal static class FileService
    {
        private static readonly object _lock = new();
        public static async Task StartFileCopy(string sourcePath, string destinationPath)
        {
            //var dir = Path.GetFullPath(sourcePath) ?? throw new ArgumentException("Error get directory");//...Local/LocalStatementApp
            var dirApp = Path.GetFileName(sourcePath);
            var destApp = Path.GetFileName(destinationPath);
            var res = Path.GetDirectoryName(sourcePath) ?? throw new ArgumentException("Error get directory");
            if (string.IsNullOrWhiteSpace(dirApp) || string.IsNullOrWhiteSpace(destApp))
            {
                throw new DirectoryNotFoundException("No such a directory");
            }
            else
            {
                DirectoryUtil.EnsureDirectory(dirApp);
            }
            var result = new WriteResult(res);
            if (result.isNotEmptyReadFile())
            {
                result.ClearTextFile();
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
        }
        public static Task CopyToDestination(string sourcePathFolder, string destinationPathFolder, string namefile)
        {
            var stopWatch = new Stopwatch();
            var currentFileName = Path.GetFileName(namefile);
            var sourcePath = Path.Combine(sourcePathFolder, currentFileName);
            var destinationPath = Path.Combine(destinationPathFolder, currentFileName);
            var directoryName = Path.GetDirectoryName(sourcePathFolder) ?? throw new ArgumentException("Error get directory");
            //lock (_lock)
            //{
            stopWatch.Start();
            File.Copy(sourcePath, destinationPath, true);
            stopWatch.Stop();
            TimeSpan timeCopyFile = stopWatch.Elapsed;
            lock (_lock)
            {
                ResultInfo fileInfo = FileResultCopyInfo.GetResultInfo(sourcePathFolder, currentFileName, timeCopyFile);
                var writeresult = new WriteResult(directoryName);
                writeresult.Write(fileInfo);
            }
            return Task.FromResult(0);
        }
    }
}
