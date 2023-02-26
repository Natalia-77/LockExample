namespace LockExample
{
    internal sealed class Program
    {
        private static async Task Main()
        {
            var dirApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var destinationFolderName = "Destination";
            var sourceFolderName = "LockStatementApp";
            var source = Path.Combine(dirApp, sourceFolderName);
            var destination = Path.Combine(dirApp, destinationFolderName);
            await FileService.StartFileCopy(source, destination).ConfigureAwait(false);
        }
    }
}
