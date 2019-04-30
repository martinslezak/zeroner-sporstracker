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
            ZeronerHealthPro[] recs;
            if (!LoadFile(inputFile, out recs))
            {
                Console.WriteLine($"Error in loading input file...");
                return;
            }


            Console.WriteLine("DONE");
        }

        static void PrintHelp()
        {
            Console.WriteLine("Zeroner  GPS -> Sportstracker GPX converter");
            Console.WriteLine("Usage:");
            Console.WriteLine("program <inputFile> <outputFile>");
        }

        static bool LoadFile(string path, out ZeronerHealthPro[] recs)
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    recs = (ZeronerHealthPro[]) serializer.Deserialize(file, typeof(ZeronerHealthPro[]));
                    Console.WriteLine($"Records: {recs.Length}");
                }
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine($"Can not open file {path}");
                recs = null;
                return false;
            }
            catch (Newtonsoft.Json.JsonSerializationException e)
            {
                Console.WriteLine($"Error in deserilization file {path}");
                recs = null;
                return false;
            }

            return true;
        }
    }
}
