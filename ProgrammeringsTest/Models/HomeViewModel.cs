using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProgrammeringsTest.DataModels;

namespace ProgrammeringsTest.Models
{
    public class HomeViewModel
    {
        [Required(ErrorMessage = "Stad måste anges.")]
        public string SearchTerm { get; set; }

        public List<Trip> Trips { get; set; }
    }
}