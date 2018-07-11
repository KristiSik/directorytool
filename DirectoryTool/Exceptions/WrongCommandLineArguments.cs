using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryTool.Exceptions
{
    class WrongCommandLineArguments : Exception
    {
        public WrongCommandLineArguments(string message)
            :base(message)
        { }
    }
}
