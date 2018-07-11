using DirectoryTool.Exceptions;
using DirectoryTool.Services;
using System;
using System.Globalization;
using System.Threading;

namespace DirectoryTool
{
    class Program
    {
        private static SavingService SavingService { get; set; } = new SavingService();
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    throw new WrongCommandLineArguments("Arguments are not specified.");
                switch (args[0])
                {
                    case "-s":
                        if (args.Length < 2)
                            throw new WrongCommandLineArguments("Folder path is not specified.");
                        //SavingService.Save(args[1]);
                        break;
                    case "-u":
                        if (args.Length < 2)
                        {
                            throw new WrongCommandLineArguments("File path is not specified.");
                        }
                        else if (args.Length < 3)
                        {
                            throw new WrongCommandLineArguments("Directory path is not specified.");
                        }
                        Console.WriteLine("Unpacking {1}", args[1]);
                        break;
                    default:
                        throw new WrongCommandLineArguments("Arguments are not recognized.");
                }
            }
            catch (WrongCommandLineArguments e) {
                InfoService.ShowErrorMessage(e.Message);
                InfoService.ShowHelpMessage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}