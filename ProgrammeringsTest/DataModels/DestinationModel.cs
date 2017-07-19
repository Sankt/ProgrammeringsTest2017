using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgrammeringsTest.DataModels
{
    public class DestinationModel
    {
        
    }
    public class StopLocation
    {
        public string id { get; set; }
        public string extId { get; set; }
        public string name { get; set; }
        public double lon { get; set; }
        public double lat { get; set; }
        public int weight { get; set; }
        public int products { get; set; }
    }

    public class RootObject
    {
        public List<StopLocation> StopLocation { get; set; }
        public List<Trip> Trip { get; set; }
        public string scrB { get; set; }
        public string scrF { get; set; }
    }
}