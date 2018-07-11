using System;
using System.Collections.Generic;

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
