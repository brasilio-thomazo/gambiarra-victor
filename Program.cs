using System;
using System.IO;

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
