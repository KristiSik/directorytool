using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryTool.Services
{
    public class SavingService
    {
        public void Save(string folderPath)
        {
            Folder folder = new Folder
            {
                Name = "12345",
                SubFolders = GetSubFolders(folderPath),
                Files = new List<File>()
                {
                    new File()
                    {
                        Name = "file.png",
                        Content = System.IO.File.ReadAllBytes(@"D:\prog\DirectoryTool\DirectoryTool\bin\Debug\2.jpg")
                    }
                }
            };
            FileStream fileStream = new FileStream("datafile.dat", FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fileStream, folder);
            }
            catch(SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }
        private List<Folder> GetSubFolders(string folderPath)
        {
            string[] subDirectories = Directory.GetDirectories(@folderPath);
            if (subDirectories.Length == 0)
            {
                return null;
            }
            List<Folder> folders = new List<Folder>();
            foreach(string subFolderPath in subDirectories)
            {
                folders.Add(new Folder()
                {
                    Name = subFolderPath.Split(Path.DirectorySeparatorChar).Last(),
                    SubFolders = GetSubFolders(subFolderPath)
                });
            }
            return folders;
        }
    }
}