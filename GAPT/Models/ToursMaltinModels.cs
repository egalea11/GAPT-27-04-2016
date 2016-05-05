using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class TempOrder
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TourDateTimeId { get; set; }
        public int AdultQuantity { get; set; }
        public int ChildQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public bool Expired {get; set;}
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

    public class ReviewModel
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public int TourId { get; set; }
        public int? RatingId { get; set; }
        public string Comment { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }

    public class ReviewViewModel
    {
        public List<ReviewModel> Reviews { get; set; }
    }

    public class WishList
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public bool Expired { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }

    public class CustomerInfoModel
    {
        public int AdultAmount { get; set; }
        public int ChildAmount { get; set; }
        public decimal AdultTotalPrice { get; set; }
        public decimal ChildTotalPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public Tour Tour { get; set; }
        public TourDate TourDate { get; set; }
        public TourTime TourTime { get; set; }
        public TourDateTime TourDateTime { get; set; }
        public IList<AdultDetails> AdultDetails { get; set; }
        public IList<ChildDetails> ChildDetails { get; set; }
        public string TourStartingLocation { get; set; }
        public DateTime DateTimeTourDate { get; set; }
        public string StringTourTime { get; set; }
        public int TourTimeId { get; set; }
        public int TourDateId { get; set; }
        public int TourDateTimeId { get; set; }
        public int TourId { get; set; }
        public bool BackToTour { get; set; }
    }

    public class AdultDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ChildDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int BirthDay { get; set; }
        public string BirthMonth { get; set; }
        public int BirthYear { get; set; }
    }

    public class TourpageModel
    {
        public Tour Tour { get; set; }
        public string TourCategory { get; set; }
        public List<Image> Images { get; set; }
        public List<TourDate> TourDates { get; set; }
        public List<TourTime> TourTimes { get; set; }
        public List<TourDateTime> TourDateTimes { get; set; }
        public List<TourTimeTable> TourTimeTables { get; set; }
        public List<Location> TourLocations { get; set; }
        public List<WishList> Wishlists { get; set; }
        public int AdultAmount { get; set; }
        public int ChildAmount { get; set; }
        public int TourDateId { get; set; }
        public int TourTimeId { get; set; }
        public DateTime TourDate { get; set; }
        public string StringTourDate { get; set; }
        [Required]
        public string TourTime { get; set; }
    }

    public class AverageRatingModel
    {
        public int AverageRating { get; set; }
    }

    public class OrderDetails
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int TourDateTimeId { get; set; }
        public int AdultQuantity { get; set; }
        public int ChildQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public decimal TotalAdultPrice { get; set; }
        public decimal TotalChildPrice { get; set; }
        public string TourName { get; set; }
        public string StringTourTime { get; set; }
        public DateTime DateTimeTourDate { get; set; }
        public string StartingLocation { get; set; }
        public int TourId { get; set; }
    }

    public class OrderHistoryModel
    {
        public List<OrderDetails> Orders { get; set; }
    }
}