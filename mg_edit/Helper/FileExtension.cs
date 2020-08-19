using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace mg_edit.Helper
{
    class FileExtension
    {
        // Helper that creates folders recursivly
        public static void AddNewFolder(string current, Queue<string> remaining)
        {
            string wd = System.IO.Directory.GetCurrentDirectory();
            string first = remaining.Dequeue();
            current = current + "\\\\" + first;
            if (!System.IO.Directory.Exists(wd + current))
            {
                System.IO.Directory.CreateDirectory(wd + current);
            }

            if (remaining.Count > 0)
            {
                AddNewFolder(current, remaining);
            }
        }


        // Creates the given folder path
        public static void CreateFolderPath(string path)
        {
            if (path[path.Length - 1] == '/') {
                path = path.Substring(0, path.Length - 1);
            }
            Queue<string> folders = new Queue<string>(path.Split('/'));

            AddNewFolder("", folders);
        }
    }
}
