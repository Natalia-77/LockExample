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
            _resultFilePath = Path.Combine(cacheFilePath, ResultFolder, ResultFile);
        }
        public async void Write(ResultInfo result)
        {
            var dir = Path.GetDirectoryName(_resultFilePath) ?? throw new ArgumentException("Error get directory");
            DirectoryUtil.EnsureDirectory(dir);
            var textToFile = $"{result.SourceFileName},{result.DateCopeFile},{result.DestinationFileName},{result.FileSize},{Math.Round(result.TimeToCopy.TotalSeconds * 1000, 2)}";
            if (File.Exists(_resultFilePath))
            {
                using StreamWriter sw = File.AppendText(_resultFilePath);
                await sw.WriteLineAsync(textToFile).ConfigureAwait(false);
            }
            else
            {
                await File.WriteAllTextAsync(_resultFilePath, textToFile).ConfigureAwait(false);
            }
        }
        public bool isNotEmptyReadFile()
        {
            if (!File.Exists(_resultFilePath))
            {
                return false;
            }
            var lines = File.ReadAllLines(_resultFilePath);
            return lines.Any();
        }
        public void ClearTextFile()
        {
            File.WriteAllText(_resultFilePath, string.Empty);
        }
    }
}
