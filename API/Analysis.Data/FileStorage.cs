using Analysis.Data.Interface;
using System;
using System.IO;

namespace Analysis.Data
{
    public class FileStorage : IStorage
    {
        private string fileName = string.Empty;

        public FileStorage(string fileName)
        {
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public string Load()
        {
            return ReadFile(this.fileName);
        }

        public void Save(string data)
        {
            SaveFile(this.fileName, data);
        }

        private void SaveFile(string fileName, string str)
        {
            File.WriteAllText(fileName, str);
        }

        private string ReadFile(string fileName)
        {
            if (File.Exists(fileName))
                return File.ReadAllText(fileName);
            else
                return string.Empty;
        }
    }
}
