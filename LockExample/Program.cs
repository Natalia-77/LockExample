namespace LockExample
{
    internal sealed class Program
    {
        private static async Task Main()
        {
            var dirApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var destinationFolderName = "Destination";
            var sourceFolderName = "LockStatementApp";
            var fileService = new FileService();
            await fileService.StartFileCopy(dirApp, destinationFolderName, sourceFolderName).ConfigureAwait(false);
        }
    }
}
