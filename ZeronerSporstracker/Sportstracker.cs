using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ZeronerSporstracker
{
    public class Sportstracker
    {
        public gpx gpx;
    }

    public class gpx
    {
        public Metadata metadata;
        public Track trk;
    }

    public class Metadata
    {
        public string name;
        public string desc;
        public Author author;
    }

    public class Author
    {
        public string name;
    }

    public class Track
    {
        [XmlArrayItem("trkpt")]
        public TrackPoint[] trkseg;
    }

    public class TrackSegment
    {
        [XmlArray("TeamMembers")]
        public TrackPoint[] trkpt;
    }

    public class TrackPoint
    {
        [XmlAttribute]
        public double lat;
        [XmlAttribute]
        public double lon;
        public double ele;
        public DateTime time;
    }
}
