using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProgrammeringsTest.DataModels;
using ProgrammeringsTest.Models;
using RestSharp;

namespace ProgrammeringsTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DestinationSearch(string searchTerm)
        {
            if (ModelState.IsValid)
            {
                var destinationList = new List<StopLocation>();

                var restClient = new RestClient();
                restClient.BaseUrl = new Uri("https://api.resrobot.se/v2/");

                var destinationRequest = new RestRequest("location.name?key=0e8eb279-dd0e-4b30-8f5d-ca0e244f9b6b&input=" +
                                                         searchTerm + "&maxNo=1&format=json");

                var destinationResponse = restClient.Execute<RootObject>(destinationRequest);

                var destinationData = destinationResponse.Data.StopLocation;

                if (destinationResponse.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var location in destinationData)
                    {
                        var item = new StopLocation
                        {
                            id = location.id,
                            name = location.name,
                            lat = location.lat,
                            lon = location.lon
                        };
                        destinationList.Add(item);
                    }
                    var stopLocation = destinationList[0].id;
                    TripSearch(stopLocation);
                }
                return View("Result");
            }
            return View("Index");
        }


        public ActionResult TripSearch(string id)
        {
            var tripList = new List<Trip>();
            var model = new ResultViewModel();

            var restClient = new RestClient {BaseUrl = new Uri("https://api.resrobot.se/v2/")};

            var tripRequest = new RestRequest("trip?key=0e8eb279-dd0e-4b30-8f5d-ca0e244f9b6b&input&originId=740000133&destId=" + id + "&format=json&numF=1&numB=0");

            var tripResponse = restClient.Execute<TripObject>(tripRequest);

            var tripData = tripResponse.Data.Trip;

            if (tripResponse.StatusCode == HttpStatusCode.OK)
            {
                foreach (var trip in tripData)
                {
                    var item = new Trip
                    {
                        duration = trip.duration,
                        LegList = trip.LegList,
                        ServiceDays = trip.ServiceDays,
                        tripId = trip.tripId,
                        ctxRecon = trip.ctxRecon,
                        idx = trip.idx
                    };
                    tripList.Add(item);
                }
            }

            var firstTrip = tripList.FirstOrDefault(x => x.idx == 0);
            if (firstTrip != null)
            {
                var departureTime = firstTrip.LegList.Leg[0].Origin.time;
                var arrivalTime = firstTrip.LegList.Leg.Last().Destination.time;
                var arrivalDate = firstTrip.LegList.Leg.Last().Destination.date;
                var duration = firstTrip.duration.Substring(2);
                var originName = firstTrip.LegList.Leg[0].Origin.name;
                var arrivalName = firstTrip.LegList.Leg.Last().Destination.name;

                model.DepartureTime = departureTime;
                model.Duration = duration;
                model.ArrivalTime = arrivalTime;
                model.ArrivalDate = arrivalDate;
                model.OriginName = originName.ToUpper();
                model.ArrivalName = arrivalName.ToUpper();

                var destLat = firstTrip.LegList.Leg.Last().Destination.lat.ToString();
                var destLng = firstTrip.LegList.Leg.Last().Destination.lon.ToString();
                var arrTimeCombined = arrivalDate + "T" + arrivalTime;
                WeatherForecast(destLat, destLng, arrTimeCombined, model);
            }


            return View("Result");
        }

        public ActionResult WeatherForecast(string lat, string lon, string arrTime, ResultViewModel model)
        {
            var forecastList = new List<TimeSery>();
            DateTime dt = Convert.ToDateTime(arrTime);

            //Rounding up the arrivalTime to an even hour for forecast API call (if the time of arrival is 16:24 for example, it gets rounded to 16:00).
            long ticks = dt.Ticks + 18000000000;    //one tick = 100 nanoseconds (10,000,000th of a second.
            var roundedTime = new DateTime(ticks - ticks % 36000000000, dt.Kind);

            //arrivalTime gets converted back to string to be able to compare it with the API response values.
            string dtStr = roundedTime.ToString().Insert(10, "T").Replace(" ", "");

            var restClient = new RestClient
            {
                BaseUrl = new Uri(
                    "https://opendata-download-metfcst.smhi.se/api/category/pmp3g/version/2/geotype/point/")
            };

            var longitude = lon.Replace(",", ".");
            var latitude = lat.Replace(",", ".");

            var weatherRequest = new RestRequest("lon/" + longitude + "/lat/" + latitude + "/data.json");

            var weatherResponse = restClient.Execute<WeatherObject>(weatherRequest);

            var weatherData = weatherResponse.Data;

            if (weatherResponse.StatusCode == HttpStatusCode.OK)
            {
                foreach (var weatherDataTimeSeries in weatherData.timeSeries)
                {
                    var item = new TimeSery
                    {
                        parameters = weatherDataTimeSeries.parameters,
                        validTime = weatherDataTimeSeries.validTime
                    };
                    forecastList.Add(item);
                }

                var arrForecast = forecastList.FirstOrDefault(x => x.validTime.Contains(dtStr));

                if (arrForecast != null)
                {
                    double temperature = arrForecast.parameters.Where(x => x.name == "t").Select(x => x.values[0]).Single();
                    double windSpeed = arrForecast.parameters.Where(x => x.name == "ws").Select(x => x.values[0]).Single();
                    double downFall = arrForecast.parameters.Where(x => x.name == "pmax").Select(x => x.values[0]).Single();
                    double downFallCategory = arrForecast.parameters.Where(x => x.name == "pcat").Select(x => x.values[0]).Single();

                    model.Temperature = temperature;
                    model.WindSpeed = windSpeed;
                    model.DownFall = downFall;

                    string categoryString = "";

                    //Switch to display the actual category name of the downfall instead of the category id.
                    switch (downFallCategory.ToString())
                    {
                        case "0":
                            categoryString = "INGEN NEDERBÖRD";
                            break;
                        case "1":
                            categoryString = "SNÖ";
                            break;
                        case "2":
                            categoryString = "SNÖBLANDAT REGN";
                            break;
                        case "3":
                            categoryString = "REGN";
                            break;
                        case "4":
                            categoryString = "DUGGREGN";
                            break;
                        case "5":
                            categoryString = "UNDERKYLT REGN";
                            break;
                        case "6":
                            categoryString = "UNDERKYLT DUGGREGN";
                            break;
                    }

                    model.DownFallCategory = downFallCategory;
                    model.DownFallString = categoryString;


                }
            }
            return View("Result", model);
        }
    }
}