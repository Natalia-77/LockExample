namespace LockExample.UnitTests
{
    public class ResultFileInfo
    {
        public string? SourceFileName { get; init; }
        public string? DateCopyFile { get; init; }
        public string? DestinationFileName { get; init; }
        public string? FileSize { get; init; }       
        public TimeSpan? TimeToCopy { get; init; }
    }
}
