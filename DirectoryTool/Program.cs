using DirectoryTool.Exceptions;
using DirectoryTool.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    throw new WrongCommandLineArguments("Arguments are not specified");
                switch (args[0])
                {
                    case "-s":
                        Console.WriteLine("Saving {0}", args[1]);
                        break;
                    case "-u":
                        Console.WriteLine("Unpacking {1}", args[1]);
                        break;
                    default:
                        throw new WrongCommandLineArguments("Arguments are not recognized");
                }
            }
            catch (WrongCommandLineArguments) {
                InfoService.ShowHelpMessage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            SavingService.Save(@"D:\prog\MyTestFolder");
            Console.ReadKey();
        }
    }
}
