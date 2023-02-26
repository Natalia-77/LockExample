namespace LockExample.UnitTests
{
    public class FileCopyTest
    {
        [Theory]
        [InlineData(" ", "DestinationTempFilesFolder")]
        [InlineData("", "DestinationTempFilesFolder")]
        public async void Empty_Or_WhiteSpace_Source_Temp_Folder(string souseTempFolderName, string destinationTempFolderName)
        {
            var tempFolder = Path.GetTempPath();
            var path = Path.Combine(tempFolder, souseTempFolderName);
            var destPath = Path.Combine(tempFolder, destinationTempFolderName);
            var expectedErrorMessage = "No such a directory";
            DirectoryNotFoundException ex = await Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await FileService.StartFileCopy(path, destPath));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Theory]
        [InlineData("LockTempFilesFolder", " ")]
        [InlineData("LockTempFilesFolder", "")]
        public async void Empty_Or_WhiteSpace_Destination_Temp_Folder(string souseTempFolderName, string destinationTempFolderName)
        {
            var tempFolder = Path.GetTempPath();
            var path = Path.Combine(tempFolder, souseTempFolderName);
            var destPath = Path.Combine(tempFolder, destinationTempFolderName);
            var expectedErrorMessage = "No such a directory";
            DirectoryNotFoundException ex = await Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await FileService.StartFileCopy(path, destPath));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Theory]
        [InlineData(" ", " ")]
        [InlineData("", "")]
        public async void Empty_Or_WhiteSpace_Source_And_Destination_Temp_Folder(string souseTempFolderName, string destinationTempFolderName)
        {
            var tempFolder = Path.GetTempPath();
            var path = Path.Combine(tempFolder, souseTempFolderName);
            var destPath = Path.Combine(tempFolder, destinationTempFolderName);
            var expectedErrorMessage = "No such a directory";
            DirectoryNotFoundException ex = await Assert.ThrowsAsync<DirectoryNotFoundException>(async () => await FileService.StartFileCopy(path, destPath));
            Assert.Equal(expectedErrorMessage, ex.Message);
        }

        [Theory]
        [InlineData("LockTempFilesFolder", "DestinationTempFilesFolder")]
        public void Copied_Files_To_Destination_Compare_Count_Files(string sourceTempFolder, string destinationTempFolder)
        {
            //Arrange
            var tempFolder = Path.GetTempPath();
            var sourcePath = Path.Combine(tempFolder, sourceTempFolder);
            var destPath = Path.Combine(tempFolder, destinationTempFolder);
            EnsureSourceDirectory(sourcePath);
            EnsureDestinationDirectory(destPath);
            //Act
            _ = FileService.StartFileCopy(sourcePath, destPath);
            var filesDestination = Directory.GetFiles(destPath);
            var filesSource = Directory.GetFiles(sourcePath);
            var destinationLen = filesDestination.Length;
            var sourceLen = filesSource.Length;
            //Assert
            Assert.Equal(destinationLen, sourceLen);
        }
        [Theory]
        [InlineData("Result", "LockTempFilesFolder")]
        public void Compare_File_Names_Source_And_Result_File(string resultFolderName, string sourceFolderName)
        {
            //Arrange
            var dirTemp = Path.GetTempPath();
            var sourcePath = Path.Combine(dirTemp, sourceFolderName);
            var pathToResultFile = Path.Combine(dirTemp, resultFolderName, "result.txt");
            var filesSource = Directory.GetFiles(sourcePath).Select(Path.GetFileName);
            var infoReadFromResultFile = GetResultFileInfos(pathToResultFile);           
            var listInfo = infoReadFromResultFile.OrderBy(item => item.SourceFileName).Select(item => item.SourceFileName);
            //Act
            var differenceQuery = listInfo.Except(filesSource);
            //Assert
            Assert.Empty(differenceQuery);            
        }       

        public List<ResultFileInfo> GetResultFileInfos(string pathFileToRead)
        {
            var listOfFileItems = new List<ResultFileInfo>();

            using StreamReader reader = new(pathFileToRead);
            string? line;
            int counter = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string[] itemsInFile = line.Split(",");
                counter++;
                listOfFileItems.Add(new ResultFileInfo()
                {
                    SourceFileName = itemsInFile[0],
                    DateCopyFile = itemsInFile[1],
                    DestinationFileName = itemsInFile[2],
                    FileSize = itemsInFile[3],
                    TimeToCopy = TimeSpan.Parse(itemsInFile[4])
                });
            }
            reader.Close();
            return listOfFileItems;
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
        private static void EnsureSourceDirectory(string pathToSourceCheckExist)
        {
            if (!Directory.Exists(pathToSourceCheckExist))
            {
                Directory.CreateDirectory(pathToSourceCheckExist);
                WriteTestFiles(pathToSourceCheckExist);
            }
        }
        private static void EnsureDestinationDirectory(string pathToDestinationCheckExist)
        {
            if (!Directory.Exists(pathToDestinationCheckExist))
            {
                Directory.CreateDirectory(pathToDestinationCheckExist);                
            }
        }
    }
}