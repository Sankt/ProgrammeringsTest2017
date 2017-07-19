using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgrammeringsTest.DataModels
{
    public class ForecastModel
    {
    }
    public class Geometry
    {
        public string type { get; set; }
        public List<List<double>> coordinates { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public string levelType { get; set; }
        public int level { get; set; }
        public string unit { get; set; }
        public List<double> values { get; set; }
    }

    public class TimeSery
    {
        public string validTime { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class WeatherObject
    {
        public string approvedTime { get; set; }
        public string referenceTime { get; set; }
        public Geometry geometry { get; set; }
        public List<TimeSery> timeSeries { get; set; }
    }
}