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

            if (!WriteOutput(outputFile, recs))
            {
                Console.WriteLine($"Error in writing output file...");
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

        static bool WriteOutput(string path, ZeronerHealthPro[] recs)
        {
            using (Stream file = File.OpenWrite(path))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(gpx));
                var st = new Sportstracker()
                {
                    gpx = new gpx()
                };

                st.gpx.metadata = new Metadata()
                {
                    name = "Test 1",
                    desc = "Desc of Test 1",
                    author = new Author()
                    {
                        name = "Martin Slezak"
                    }
                };

                //st.gpx.trk = new Track()
                //{
                //    trkseg = new TrackSegment()
                //};

                //st.gpx.trk.trkseg.trkpt = new TrackPoint[recs.Length];
                //st.gpx.trk.trkseg = new TrackPoint[recs.Length];
                st.gpx.trk = new Track()
                {
                    trkseg = new TrackPoint[recs.Length]
                };

                uint idx = 0;
                foreach (var r in recs)
                {
                    var trkpt = new TrackPoint()
                    {
                        lat = r.Y,
                        lon = r.X,
                        ele = r.V,
                        time = UnixTimeStampToDateTime(r.T)
                    };

                    st.gpx.trk.trkseg[idx++] = trkpt;
                }

                serializer.Serialize(file, st.gpx);
            }
            return true;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
