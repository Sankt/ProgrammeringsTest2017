using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgrammeringsTest.DataModels
{
    public class TripModel
    {
    }
    public class ServiceDay
    {
        public string planningPeriodBegin { get; set; }
        public string planningPeriodEnd { get; set; }
        public string sDaysR { get; set; }
        public string sDaysI { get; set; }
        public string sDaysB { get; set; }
    }

    public class Origin
    {
        public string name { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string extId { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public string time { get; set; }
        public string date { get; set; }
    }

    public class Destination
    {
        public string name { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string extId { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public string time { get; set; }
        public string date { get; set; }
    }

    public class Note
    {
        public string value { get; set; }
        public string key { get; set; }
        public string type { get; set; }
        public int priority { get; set; }
        public int routeIdxFrom { get; set; }
        public int routeIdxTo { get; set; }
    }

    public class Notes
    {
        public List<Note> Note { get; set; }
    }

    public class Product
    {
        public string name { get; set; }
        public string num { get; set; }
        public string catCode { get; set; }
        public string catOutS { get; set; }
        public string catOutL { get; set; }
        public string operatorCode { get; set; }
        public string @operator { get; set; }
        public string operatorUrl { get; set; }
    }

    public class Stop
    {
        public string name { get; set; }
        public string id { get; set; }
        public string extId { get; set; }
        public int routeIdx { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public string depTime { get; set; }
        public string depDate { get; set; }
        public string arrTime { get; set; }
        public string arrDate { get; set; }
    }

    public class Stops
    {
        public List<Stop> Stop { get; set; }
    }

    public class Leg
    {
        public Origin Origin { get; set; }
        public Destination Destination { get; set; }
        public Notes Notes { get; set; }
        public Product Product { get; set; }
        public Stops Stops { get; set; }
        public string idx { get; set; }
        public string name { get; set; }
        public string transportNumber { get; set; }
        public string transportCategory { get; set; }
        public string type { get; set; }
        public bool reachable { get; set; }
        public string direction { get; set; }
        public string duration { get; set; }
        public int? dist { get; set; }
    }

    public class LegList
    {
        public List<Leg> Leg { get; set; }
    }

    public class Trip
    {
        public List<ServiceDay> ServiceDays { get; set; }
        public LegList LegList { get; set; }
        public int idx { get; set; }
        public string tripId { get; set; }
        public string ctxRecon { get; set; }
        public string duration { get; set; }
    }

    public class TripObject
    {
        public List<Trip> Trip { get; set; }
        public string scrB { get; set; }
        public string scrF { get; set; }
    }
}