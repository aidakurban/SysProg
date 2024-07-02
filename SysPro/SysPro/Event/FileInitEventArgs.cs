using System;

namespace SysPro
{
    public class FileInitEventArgs
    {
        public string FilePath { get; set; }
        public FileInitEventArgs(string filePath)
        {
            FilePath = filePath;
        }

    }
}
