using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace DirectoryTool.Services
{
    public class SavingService
    {
        public void Save(string folderPath, string fileName)
        {
            InfoService.InitializeProgressMessageThread();
            folderPath = Path.GetFullPath(folderPath);
            string rootFolderName = folderPath.Split(Path.DirectorySeparatorChar).Last();
            if (String.IsNullOrEmpty(fileName))
            {
                fileName = rootFolderName;
            }
            Folder folder;
            FileStream fileStream = new FileStream(fileName + "." + ConfigurationManager.AppSettings["SavedFileFormat"], FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                InfoService.ShowStatusMessage("Reading...");
                folder = new Folder
                {
                    Name = rootFolderName,
                    SubFolders = GetSubFolders(folderPath),
                    Files = GetFiles(folderPath)
                };
                InfoService.ProcessFinished = true;
                InfoService.ShowStatusMessage("Saving...");
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
                InfoService.ShowStatusMessage("Done.");
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
            try
            {
                var exceptions = new ConcurrentQueue<Exception>();
                Parallel.ForEach(
                    filesInDirectory,
                    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                    (filePath) =>
                    {
                        try
                        {
                            InfoService.FileInProcess = filePath;
                            lock (files)
                            {
                                files.Add(new File()
                                {
                                    Name = filePath.Split(Path.DirectorySeparatorChar).Last(),
                                    Content = System.IO.File.ReadAllBytes(filePath)
                                });
                            }
                        }
                        catch (Exception e)
                        {
                            exceptions.Enqueue(e);
                        }
                        finally
                        {
                            EstimatingService.ProcessedBytes += new FileInfo(filePath).Length;
                        }
                    });
                if (exceptions.Count != 0)
                    throw new AggregateException(exceptions);
            }
            catch(AggregateException e)
            {
                foreach(var exception in e.Flatten().InnerExceptions)
                {
                    InfoService.ShowErrorMessage(exception.Message);
                }
            }
            return files;
        }
    }
}