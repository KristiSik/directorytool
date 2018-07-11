using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DirectoryTool.Services
{
    public class UnpackingService
    {
        public void Unpack(string filePath)
        {
            Folder folder = null;
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                folder = (Folder)formatter.Deserialize(fileStream);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            }
            finally
            {
                fileStream.Close();
            }
        }
    }
}
