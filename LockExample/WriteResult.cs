using LockExample.Utils;

namespace LockExample
{
    internal sealed class WriteResult
    {
        private const string ResultFolder = "Result";
        private const string ResultFile = "result.txt";
        private readonly string _resultFilePath;
        public WriteResult(string? cacheFilePath = null)
        {
            _resultFilePath = cacheFilePath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ResultFolder, ResultFile);
        }
        public async Task Write(ResultInfo result)
        {
            var dir = Path.GetDirectoryName(_resultFilePath) ?? throw new ArgumentException("Error get directory");
            DirectoryUtil.EnsureDirectory(dir);
            using StreamWriter sw = File.AppendText(_resultFilePath);
            await sw.WriteLineAsync($"{result.SourceFileName} copy {result.DateCopeFile.ToShortDateString()}" +
                $" into {result.DestinationFileName} with size {result.FileSize} total time :" +
                $" {Math.Round(result.TimeToCopy.TotalSeconds * 1000, 2)}").ConfigureAwait(false);
        }
    }
}
