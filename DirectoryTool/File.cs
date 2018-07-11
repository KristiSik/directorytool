using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryTool
{
    [Serializable]
    public class File
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}
