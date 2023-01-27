﻿using System.Diagnostics;

namespace LockExample
{
    internal static class FileService
    {
        private const string FolderName = "LockStatementApp";
        private const string Destination = "Destination";
        private static readonly object fileLock = new();

        public static void StartFileCopy()
        {
            var dirApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var destinationPath = Path.Combine(dirApp, Destination);
            var sourcePath = Path.Combine(dirApp, FolderName);
            if (!Directory.Exists(destinationPath))
            {
                _ = Directory.CreateDirectory(destinationPath);
            }
            _ = CopyFileAsync(sourcePath, destinationPath);
        }
        public static async Task CopyFileAsync(string source, string destination)
        {
            var stopWatch = new Stopwatch();
            var fileEntries = Directory.GetFiles(source);
            var len = fileEntries.Length;
            var tasks = new Task[len];
            for (var i = 0; i < len; i++)
            {
                lock (fileLock)
                {
                    stopWatch.Start();
                    Console.WriteLine(Path.GetFileName(fileEntries[i]));
                    tasks[i] = Task.Run(() => CopyToDestination(source, destination, fileEntries[i]));
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    Console.WriteLine($"RunTime {ts.TotalSeconds * 1000}");
                }
                Thread.Sleep(2000);

            }
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public static void CopyToDestination(string source, string destination, string namefile)
        {
            var currentFileName = Path.GetFileName(namefile);
            File.Copy(Path.Combine(source, currentFileName), Path.Combine(destination, currentFileName), true);

        }
    }
}
