using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryTool.Services
{
    public static class InfoService
    {
        public static string FileInProcess { get; set; }
        private static readonly int lengthOfProgressBar = 40;
        private static int CursorTopPosition;
        private static Dictionary<string, string> Commands { get; set; }
        public static bool ProcessFinished { get; set; } = false;
        static InfoService()
        {
            Commands = new Dictionary<string, string>
            {
                { "-s <folder path> <file name>", "saving folder (including files and subfolders) into file" },
                { "-u <file path> <directory path>", "unpacking folder from file into selected directory" }
            };
            CursorTopPosition = Console.CursorTop;
        }

        public static void ShowHelpMessage()
        {
            Console.WriteLine("Directory Tool usage:");
            foreach(var command in Commands)
            {
                Console.WriteLine("  {0} — {1}", command.Key, command.Value);
            }
        }
        public static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void InitializeProgressMessageThread()
        {
            Thread progressBarThread = new Thread(ShowProgress);
            progressBarThread.Start();
        }
        public static void ShowProgress()
        {
            do
            {
                int i = 0;
                StringBuilder progressBar = new StringBuilder("[");
                long lengthOfFilledPart = (ProcessFinished) ?lengthOfProgressBar: (long) (lengthOfProgressBar * ((EstimatingService.NumberOfBytesToProcess == 0)?0:((double) EstimatingService.ProcessedBytes / EstimatingService.NumberOfBytesToProcess)));
                int percents = Math.Min((EstimatingService.ProcessedBytes == 0) ? 0 : ((int)(100 * (double)EstimatingService.ProcessedBytes / EstimatingService.NumberOfBytesToProcess)), 100);
                while (EstimatingService.ProcessedBytes > 0 && i < lengthOfFilledPart)
                {
                    progressBar.Append('#');
                    i++;
                }
                while (i < lengthOfProgressBar)
                {
                    progressBar.Append('.');
                    i++;
                }
                if (ProcessFinished)
                {
                    FileInProcess = "";
                }
                progressBar.Append($"] {percents}% {Path.GetFileName(FileInProcess)}");
                ClearConsoleLine(CursorTopPosition);
                Console.Write(progressBar);
                Thread.Sleep(100);
                if (ProcessFinished && lengthOfFilledPart == lengthOfProgressBar)
                {
                    break;
                }
            } while (true);
        }
        public static void ShowStatusMessage(string message)
        {
            ClearConsoleLine(CursorTopPosition + 1);
            Console.Write(message);
        }
        private static void ClearConsoleLine(int linePosition)
        {
            Console.SetCursorPosition(0, linePosition);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, linePosition);
        }
    }
}