﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DirectoryTool.Services
{
    public class UnpackingService
    {
        public void Unpack(string filePath, string directoryPath)
        {
            InfoService.InitializeProgressMessageThread();
            if (String.IsNullOrEmpty(directoryPath))
            {
                directoryPath = Directory.GetCurrentDirectory();
            }
            Folder folder = null;
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            try
            {
                InfoService.ShowStatusMessage("Reading...");
                BinaryFormatter formatter = new BinaryFormatter();
                folder = (Folder)formatter.Deserialize(fileStream);
            }
            catch (SerializationException e)
            {
                InfoService.ShowErrorMessage("Failed to unpack. Reason: " + e.Message);
            }
            catch (FileNotFoundException e)
            {
                InfoService.ShowErrorMessage(e.Message);
            }
            finally
            {
                fileStream.Close();
            }
            InfoService.ShowStatusMessage("Unpacking...");
            CreateFoldersAndFilesFromInstance(folder, directoryPath);
            InfoService.ProcessFinished = true;
            EstimatingService.ProcessedBytes = EstimatingService.NumberOfBytesToProcess;
            InfoService.ShowStatusMessage("Done.");
        }
        private void CreateFoldersAndFilesFromInstance(Folder rootFolder, string directoryPath)
        {
            string newPath = string.Format("{0}{1}{2}", directoryPath, Path.DirectorySeparatorChar, rootFolder.Name);
            Directory.CreateDirectory(newPath);
            if (rootFolder.SubFolders != null)
            {
                foreach (Folder folder in rootFolder.SubFolders)
                {
                    CreateFoldersAndFilesFromInstance(folder, newPath);
                }
            }
            if (rootFolder.Files != null)
            {
                foreach (File file in rootFolder.Files)
                {
                    string fileName = $"{newPath}{Path.DirectorySeparatorChar}{file.Name}";
                    System.IO.File.WriteAllBytes(fileName, file.Content);
                    EstimatingService.ProcessedBytes += new FileInfo(fileName).Length;
                }
            }
        }
    }
}