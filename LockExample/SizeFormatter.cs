namespace LockExample
{
    public static class SizeFormatter
    {
        private static readonly string[] sizes = { "byte", "KB", "MB", "GB" };

        public static string FileSizeFormat(long fileSizeInBytes)
        {
            var tempNumber = (decimal)fileSizeInBytes;
            var count = 0;
            while (Math.Round(tempNumber / 1024) >= 1)
            {
                tempNumber /= 1024;
                count++;
            }
            return FormattableString.Invariant($"{tempNumber:N1} {sizes[count]}");
        }
    }
}
