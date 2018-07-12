using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryTool.Services
{
    public class EstimatingService
    {
        public long NumberOfBytesToProcess { get => _numberOfBytesToProcess; private set => _numberOfBytesToProcess = value; }
        private long _numberOfBytesToProcess;

        public void EstimateWorkToBeDone(string folderPath)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            folderPath = Path.GetFullPath(folderPath);
            ScanSubFolders(folderPath);
            sw.Stop();
            System.Console.WriteLine("Parallel: {0} ({1} bytes)", sw.Elapsed, NumberOfBytesToProcess);
        }
        private void ScanSubFolders(string folderPath)
        {
            string[] subDirectories = Directory.GetDirectories(@folderPath);
            ScanFiles(folderPath);
            if (subDirectories.Length != 0)
            {
                foreach (string subFolderPath in subDirectories)
                {
                    ScanSubFolders(subFolderPath);
                }
            }
        }
        private void ScanFiles(string folderPath)
        {
            string[] filesInDirectory = Directory.GetFiles(folderPath);
            if (filesInDirectory.Length != 0)
            {
                Parallel.ForEach<string, long>(
                    filesInDirectory,
                    () => 0,
                    (filePath, loop, subNumberOfBytes) =>
                    {
                        return subNumberOfBytes += new FileInfo(filePath).Length;
                    },
                    (finalResult) => Interlocked.Add(ref _numberOfBytesToProcess, finalResult)
                );
            }
        }
    }
}
