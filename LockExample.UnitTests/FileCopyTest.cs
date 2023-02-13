namespace LockExample.UnitTests
{
    public class FileCopyTest
    {
        private const string TempFolderName = "LockTempFilesFolder";
        private const string DestinationTempFolderName = "Destination";

        [Fact]
        public async void Copied_Files_To_Destination_Not_Null()
        {           
            var tempFolder = Path.GetTempPath();            
            var path = Path.Combine(tempFolder, TempFolderName);
            var destPath = Path.Combine(tempFolder, DestinationTempFolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                WriteTestFiles(path);
            }
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }
            var fileService = new FileService(tempFolder);
            await fileService.StartFileCopy(tempFolder, DestinationTempFolderName, TempFolderName);
            var files = Directory.GetFiles(destPath);

            Assert.NotNull(files);
        }
        private static void WriteTestFiles(string sourceDir)
        {
            var extention = ".txt";
            for (int i = 0; i < 3; i++)
            {
                var filePath = Path.Combine(sourceDir, Path.GetRandomFileName() + extention);
                Random rnd = new();
                var sizeInKb = rnd.Next(1, 25);
                byte[] data = new byte[sizeInKb * 1024];
                rnd.NextBytes(data);
                string secureRandomString = Convert.ToBase64String(data);
                File.WriteAllText(filePath, secureRandomString);
            }             
        }
    }
}