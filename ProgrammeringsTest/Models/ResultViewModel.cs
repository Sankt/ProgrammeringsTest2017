namespace ProgrammeringsTest.Models
{
    public class ResultViewModel
    {
        public string Duration { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string ArrivalDate { get; set; }
        public string OriginName { get; set; }
        public string ArrivalName { get; set; }

        public double Temperature { get; set; }
        public double WindSpeed { get; set; }
        public double DownFall { get; set; }
        public double DownFallCategory { get; set; }
        public string DownFallString { get; set; }
    }
}