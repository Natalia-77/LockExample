using System.Text;
namespace LockExample
{
    internal static class FileService
    {
        private const string ResultFileName = "result.txt";
        private const string FolderName = "LockStatementApp";
        private static readonly object fileLock = new();

        public static string GetPathDirectory(string dirFilename)
        {
            var dirApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var locationFile = Path.Combine(dirApp, FolderName, dirFilename);
            var dir = Path.GetDirectoryName(locationFile);
            if (!Directory.Exists(dir))
            {
                if (dir is not null)
                {
                    _ = Directory.CreateDirectory(dir);
                }
            }
            return dir ?? "";
        }
        public static void UpdateFile(string fileName)
        {
            var path = GetPathDirectory(fileName);
            var fullPath = Path.Combine(path, fileName);
            var pathToFileResult = Path.Combine(path, ResultFileName);
            string? textResultRead;
            lock (fileLock)
            {
                if (!File.Exists(fullPath))
                {
                    try
                    {
                        var textWrite = $"This is some text in the {fileName} file.";
                        CreateAndWriteToFile(fullPath, textWrite);
                        textResultRead = ReadFromFile(fullPath);
                        EnsureFile(pathToFileResult, textResultRead);
                    }
                    catch
                    {
                        throw new ArgumentException(nameof(path));
                    }
                }
                else
                {
                    textResultRead = ReadFromFile(fullPath);
                    EnsureFile(pathToFileResult, textResultRead);
                }
            }
        }
        public static void CreateAndWriteToFile(string pathToWrite, string textToWrite)
        {
            using FileStream fileStream = File.Create(pathToWrite);
            var info = new UTF8Encoding(true).GetBytes(textToWrite);
            fileStream.Write(info, 0, info.Length);
        }
        public static string ReadFromFile(string pathToRead)
        {
            var readTextFromFile = File.ReadAllText(pathToRead);
            return readTextFromFile;
        }
        public static void EnsureFile(string filepath, string textToFile)
        {
            if (!File.Exists(filepath))
            {
                CreateAndWriteToFile(filepath, textToFile);
            }
            else
            {
                using StreamWriter sw = File.AppendText(filepath);
                sw.WriteLine(textToFile);
            }
        }
    }
}
