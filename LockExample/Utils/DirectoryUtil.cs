namespace LockExample.Utils
{
    internal static class DirectoryUtil
    {
        public static void EnsureDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                _ = Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
