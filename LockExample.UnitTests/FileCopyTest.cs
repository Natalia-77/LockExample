namespace LockExample.UnitTests
{
    public class FileCopyTest
    {       
        [Fact]
        public void Test1()
        {           
            var res = Path.GetTempPath();//C:...//AppData/Local/Temp
            Console.WriteLine(res);
            var path = Path.Combine(res, "LockTempFilesFolder");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                WriteTestFiles(path);
            }
            //Console.WriteLine(path);
        }
        private void WriteTestFiles(string sourceDir)
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