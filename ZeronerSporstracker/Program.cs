using Newtonsoft.Json;
using System;
using System.IO;

namespace ZeronerSporstracker
{
    class Program
    {
        enum ExitCodes
        {
            Ok = 0,

            NoFilesToProceed
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintHelp();
                return;
            }

            var inputFile = args[0];
            var outputFile = args[1];

            using (StreamReader file = File.OpenText(inputFile))
            {
                //TextReader
                var content = file.ReadToEnd();
                JsonSerializer serializer = new JsonSerializer();
                var movie2 = serializer.Deserialize(file.ReadToEnd(), typeof(ZeronerHealthPro));
            }

            Console.WriteLine("Hello World!");
        }

        static void PrintHelp()
        {
            Console.WriteLine("Zeroner  GPS -> Sportstracker GPX converter");
            Console.WriteLine("Usage:");
            Console.WriteLine("program <inputFile> <outputFile>");
        }
    }
}
