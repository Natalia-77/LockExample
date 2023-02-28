namespace LockExample
{
    internal class ResultInfo
    {
        public string? SourceFileName { get; init; }
        public string? DestinationFileName { get; init; }
        public string? FileSize { get; init; }
        public DateTime DateCopeFile { get; init; }
        public TimeSpan TimeToCopy { get; init; }
    }
}
