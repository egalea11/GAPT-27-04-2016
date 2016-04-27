using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAPT.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TownId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
    }

    public class LocationAttraction
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int AttractionTypeId { get; set; }
    }
}