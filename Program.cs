using System;
using System.IO;
using System.Security.AccessControl;

namespace GambiarraVictor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Entre com a origem:");
            string source = Console.ReadLine();
            Console.WriteLine("Entre com o destino:");
            string destination = Console.ReadLine();

            if (!Directory.Exists(source))
            {
                Console.WriteLine($"Origem: [{source}] não existe, ou você não tem acesso.");
                return;
            }

            if (!Directory.Exists(destination))
            {
                Console.WriteLine($"Destino: [{destination}] não existe, criando o diretório.");
                try
                {
                    Directory.CreateDirectory(destination);
                    DirectorySecurity security = new DirectoryInfo(source).GetAccessControl(AccessControlSections.All);
                    new DirectoryInfo(destination).SetAccessControl(security);
                } catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
            }

            new CopyAll(source, destination);
            Console.ReadLine();
        }
    }
}
