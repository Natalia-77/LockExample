namespace LockExample
{
    internal sealed class Program
    {
        private static async Task Main()
        {
            await FileService.StartFileCopy().ConfigureAwait(false);
        }
    }
}
