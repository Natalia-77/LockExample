using System.Diagnostics;
using System.Runtime.CompilerServices;
using LockExample.Utils;
[assembly: InternalsVisibleTo("LockExample.UnitTests")]
namespace LockExample
{
    internal sealed class FileService
    {
        //private const string FolderName = "LockStatementApp";
        //private const string Destination = "Destination";
        private readonly string _sourcePath;
        public FileService(string? sourcePath = null)
        {
            _sourcePath = sourcePath ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
        public async Task StartFileCopy(string destinationFilePath, string destinationFolderName, string sourceFolderName)
        {
            var destinationPath = Path.Combine(destinationFilePath, destinationFolderName);
            var source = Path.Combine(_sourcePath, sourceFolderName);
            DirectoryUtil.EnsureDirectory(destinationPath);
            await CopyFileAsync(source, destinationPath).ConfigureAwait(false);
        }
        public async Task CopyFileAsync(string source, string destination)
        {
            var fileEntries = Directory.GetFiles(source);
            var tasks = new List<Task<ResultInfo>>();
            foreach (var item in fileEntries)
            {
                var task = Task.Run(() => CopyToDestination(source, destination, item));
                tasks.Add(task);
                ResultInfo resultItem = await task.ConfigureAwait(false);
                var writeresult = new WriteResult(_sourcePath);
                await writeresult.Write(resultItem).ConfigureAwait(false);
            }
            _ = await Task.WhenAll(tasks).ConfigureAwait(true);
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
