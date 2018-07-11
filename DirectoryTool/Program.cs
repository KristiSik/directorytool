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
        private static UnpackingService UnpackingService { get; set; } = new UnpackingService();
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                    throw new WrongCommandLineArguments("Arguments are not specified.");
                switch (args[0].Substring(1))
                {
                    case "s":
                        if (args.Length < 2)
                            throw new WrongCommandLineArguments("Folder path is not specified.");
                        if (args.Length < 3)
                        {
                            SavingService.Save(@args[1], null);
                        } else
                        {
                            SavingService.Save(@args[1], @args[2]);
                        }
                        break;
                    case "u":
                        if (args.Length < 2)
                        {
                            throw new WrongCommandLineArguments("File path is not specified.");
                        }
                        if (args.Length < 3)
                        {
                            UnpackingService.Unpack(@args[1], null);
                        } else
                        {
                            UnpackingService.Unpack(@args[1], @args[2]);
                        }
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
                InfoService.ShowErrorMessage(e.Message);
            }
        }
    }
}