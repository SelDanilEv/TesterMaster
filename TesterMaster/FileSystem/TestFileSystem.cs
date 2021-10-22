using System;
using System.IO;

namespace TesterMaster.FileSystem
{
    class TestFileSystem
    {
        public void Start()
        {
            Console.WriteLine("Input directory path");
            var path = Console.ReadLine();

            var homeDirectory = new DirectoryInfo(path);

            PrintLastAccessTime(homeDirectory);
        }

        public void PrintLastAccessTime(DirectoryInfo source)
        {
            source.Refresh();

            foreach (FileInfo fi in source.GetFiles())
            {
                //Console.WriteLine(fi.FullName + " (LWT)-> " + fi.LastAccessTime.Date + "\n");
                //Console.WriteLine(fi.FullName + " (LAT)-> " + fi.LastAccessTime.Date +"\n");
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                PrintLastAccessTime(diSourceSubDir);

                Console.WriteLine(diSourceSubDir.FullName + " (CT)-> " + diSourceSubDir.CreationTime + "\n");
            }
        }
    }
}