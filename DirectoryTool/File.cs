using System;

namespace DirectoryTool
{
    [Serializable]
    public class File
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}
