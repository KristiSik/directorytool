using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryTool
{
    [Serializable]
    public class Folder
    {
        public string Name { get; set; }
        public List<Folder> SubFolders { get; set; }
        public List<File> Files { get; set; }
    }
}
