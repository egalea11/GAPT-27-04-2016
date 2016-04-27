using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAPT.Models
{
    public class Order
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TourDateTimeId { get; set; }
        public int AdultQuantity { get; set; }
        public int ChildQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }

    public class Review
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public int? RatingId { get; set; }
        public string Comment { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }

    public class WishList
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public bool Expired { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }

    public class TourpageModel
    {
        public Tour Tour { get; set; }
        public string TourCategory { get; set; }
        public List<Image> Images {get; set;}
        public List<TourDate> TourDates { get; set; }
        public List<TourTime> TourTimes { get; set; }
        public List<TourDateTime> TourDateTimes { get; set; }
        public List<TourTimeTable> TourTimeTables { get; set; }
        public List<Location> TourLocations { get; set; }
        public List<WishList> Wishlists { get; set; }
    }
}