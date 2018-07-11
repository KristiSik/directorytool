using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryTool.Services
{
    public static class InfoService
    {
        private static Dictionary<string, string> Commands { get; set; }
        static InfoService()
        {
            Commands = new Dictionary<string, string>
            {
                { "-s <folder path>", "saving folder (also files and subfolders) into file" },
                { "-u <file path> <directory path>", "unpacking folder from file into selected directory" }
            };
        }
        public static void ShowHelpMessage()
        {
            Console.WriteLine("Directory Tool usage:");
            foreach(var command in Commands)
            {
                Console.WriteLine(" {0} — {1}", command.Key, command.Value);
            }
        }
        public static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
