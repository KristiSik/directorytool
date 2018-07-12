using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryTool.Services
{
    public static class EstimatingService
    {
        public static long NumberOfBytesToProcess
        {
            get
            {
                return _numberOfBytesToProcess;
            }
            private set
            {
                _numberOfBytesToProcess = value;
            }
        }
        private static long _numberOfBytesToProcess;
        public static long ProcessedBytes { get; set; } = 0;

        public static void EstimateWorkToSave(string folderPath)
        {
            folderPath = Path.GetFullPath(folderPath);
            ScanSubFolders(folderPath);
        }
        public static void EstimateWorkToUnpack(string filePath)
        {
            NumberOfBytesToProcess = new FileInfo(filePath).Length;
        }
        private static void ScanSubFolders(string folderPath)
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
        private static void ScanFiles(string folderPath)
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
