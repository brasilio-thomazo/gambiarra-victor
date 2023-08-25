using System;
using System.IO;
using System.Security.AccessControl;

namespace GambiarraVictor
{

    internal class CopyAll
    {

        public CopyAll(string source, string destination)
        {
            if (Directory.Exists(source))
            {
                DirectoryInfo srcInfo = new DirectoryInfo(source);
                DirectoryInfo dstInfo = new DirectoryInfo(destination);
                this.ReadSource(srcInfo.FullName, dstInfo.FullName);
            } else
            Console.WriteLine("Source directory {} not exists.");

        }

        private void PrintSuccess()
        {
            string text = "[SUCCESS]";
            int winWidth = Console.WindowWidth;
            int textStart = winWidth - text.Length;
            if (textStart >= 0) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(textStart, Console.CursorTop);
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        private void ReadSource(string source, string destination)
        {
            if (!Directory.Exists(source))
            {
                Console.WriteLine($"Source directory {source} not exists.");
                return;
            }

            if (!Directory.Exists(destination))
            {
                Console.Write($"Creating destination directory {destination}");
                Directory.CreateDirectory(destination);
                PrintSuccess();
                DirectorySecurity security = new DirectoryInfo(source).GetAccessControl(AccessControlSections.All);
                new DirectoryInfo(destination).SetAccessControl(security);
            }

            DirectoryInfo info = new DirectoryInfo(destination);

            string[] files = Directory.GetFiles(source);
            CopyFiles(files, info.FullName);
            foreach (string dir in Directory.GetDirectories(source))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                Console.WriteLine($"Read subdirectory {dir}");
                ReadSource(dir, string.Concat(destination, "\\", directoryInfo.Name));
            }
        }

        private void CopyFiles(string[] files, string destination)
        {
            foreach (string file in files) {
                FileInfo info = new FileInfo(file);
                Copyfile(info, string.Concat(destination, "\\", info.Name));
            }
        }

        private void Copyfile(FileInfo source, string destination)
        {
            Console.WriteLine($"copy {source} to {destination}");
            StreamCopy(source, destination);
        }

        private void StreamCopy(FileInfo source, string destination)
        {
            byte[] buffer = new byte[4096];
            using (FileStream fs = new FileStream(source.FullName, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            using (FileStream ws = new FileStream(destination, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(ws))
            {
                int read;
                long write = 0;
                Console.WriteLine($"Copy {source.Name}...\n");
                while ((read = br.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Console.CursorLeft = 0;
                    write += read;
                    bw.Write(buffer, 0, read);
                    Console.Write($"{write}/{source.Length}");
                }
                Console.WriteLine("\n");

            }
            SetPermissions(source, destination);
        }

        private void SetPermissions(FileInfo source, string destination) {
            try
            {
                FileInfo dst = new FileInfo(destination);
                FileSecurity security = source.GetAccessControl(AccessControlSections.All);
                Console.Write($"Apply permission on {destination}");
                dst.SetAccessControl(security);
                PrintSuccess();
            }
            catch(Exception ex)
            {
                Console.WriteLine (ex.ToString());
            }

        }
    }
}
