using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DirectoryTool.Services
{
    public class SavingService
    {
        public void Save(string folderPath)
        {
            Folder folder;
            FileStream fileStream = new FileStream("datafile.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                folder = new Folder
                {
                    Name = folderPath.Split(Path.DirectorySeparatorChar).Last() ?? folderPath,
                    SubFolders = GetSubFolders(folderPath),
                    Files = GetFiles(folderPath)
                };
                formatter.Serialize(fileStream, folder);
            }
            catch(DirectoryNotFoundException e)
            {
                InfoService.ShowErrorMessage(e.Message);
            }
            catch(SerializationException e)
            {
                InfoService.ShowErrorMessage("Failed to save. Reason: " + e.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }
        private List<Folder> GetSubFolders(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException("Directory not found.");
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
                    SubFolders = GetSubFolders(subFolderPath),
                    Files = GetFiles(subFolderPath)
                });
            }
            return folders;
        }
        private List<File> GetFiles(string folderPath)
        {
            string[] filesInDirectory = Directory.GetFiles(folderPath);
            if (filesInDirectory.Length == 0)
            {
                return null;
            }
            List<File> files = new List<File>();
            foreach(string filePath in filesInDirectory)
            {
                files.Add(new File() {
                    Name = filePath.Split(Path.DirectorySeparatorChar).Last(),
                    Content = System.IO.File.ReadAllBytes(filePath)
                });
            }
            return files;
        }
    }
}