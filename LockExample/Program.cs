namespace LockExample
{
    internal sealed class Program
    {
        private static void Main()
        {
            var res = FileService.GetPathDirectory();
            Console.WriteLine(res);
        }
    }
}
