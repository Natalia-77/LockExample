namespace LockExample
{
    internal static class FileResultCopyInfo
    {
        public static ResultInfo GetResultInfo(string sourceFolder, string fileName, TimeSpan time)
        {
            var sourcePath = Path.Combine(sourceFolder, fileName);
            var fileInfo = new FileInfo(sourcePath);
            var fileLength = fileInfo.Length;
            var result = new ResultInfo()
            {
                SourceFileName = fileName,
                DestinationFileName = fileName,
                FileSize = SizeFormatter.FileSizeFormat(fileLength),
                DateCopeFile = DateTime.UtcNow,
                TimeToCopy = time
            };
            return result;
        }
    }
}
