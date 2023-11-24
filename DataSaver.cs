using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    
    internal static class DataSaver
    {
        static string mainDir = FileSystem.Current.AppDataDirectory;
        public static string ReadTextFile(string filePath)
        {
            try
            {
                if (!File.Exists(mainDir + "/" + filePath))
                {
                    File.Create(mainDir + "/" + filePath);
                    Debug.WriteLine(mainDir + "/" + filePath);
                }
                var file = mainDir + "\\" + filePath;
                using Stream fileStream = File.OpenRead(file);
                using StreamReader reader = new StreamReader(fileStream);
                string res = reader.ReadToEnd();
                reader.Close();
                fileStream.Close();
                return res;
            }
            catch (Exception ex)
            {
                EventBus.codeError(ex);
                return "";
            }
        }
        public static void WriteTextFile(string file,string content)
        {
            using Stream fileStream = File.OpenWrite(mainDir + "/" + file);
            using StreamWriter writer = new StreamWriter(fileStream);
            writer.Write(content);
            writer.Close();
            fileStream.Close();
        }
    }
}
