namespace LockExample
{
    internal sealed class Program
    {
        private static async Task Main()
        {
            var tasks = new Task[4];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(StartFile);
            }
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        private static void StartFile()
        {
            string[] fileNames = { "first.txt", "second.txt" };
            foreach (var item in fileNames)
            {
                FileService.UpdateFile(item);
            }
        }
    }
}
