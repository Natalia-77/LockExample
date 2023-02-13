using LockExample.Utils;
namespace LockExample
{
    internal sealed class WriteResult
    {
        private const string ResultFolder = "Result";
        private const string ResultFile = "result.txt";
        private readonly string _resultFilePath;
        public WriteResult(string cacheFilePath)
        {
            _resultFilePath = cacheFilePath;
        }
        public async Task Write(ResultInfo result)
        {
            var pathToWrite = Path.Combine(_resultFilePath, ResultFolder, ResultFile);
            var dir = Path.GetDirectoryName(pathToWrite) ?? throw new ArgumentException("Error get directory");
            DirectoryUtil.EnsureDirectory(dir);
            using StreamWriter sw = File.AppendText(pathToWrite);
            await sw.WriteLineAsync($"{result.SourceFileName} copy {result.DateCopeFile.ToShortDateString()}" +
                $" into {result.DestinationFileName} with size {result.FileSize} total time :" +
                $" {Math.Round(result.TimeToCopy.TotalSeconds * 1000, 2)}").ConfigureAwait(false);
        }
    }
}
