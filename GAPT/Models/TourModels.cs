using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace GAPT.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildPrice { get; set; }
        public int MaxGroupSize { get; set; }
        public int AverageRatingId { get; set; }
        public int CategoryId { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }

    public class TourAttendees
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public bool IsAdult { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }

    public class TourDate
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public DateTime DateOfTour { get; set; }
    }

    public class TourDateTime
    {
        public int Id { get; set; }
        public int TourDateId { get; set; }
        public int TourTimeId { get; set; }
    }

    public class TourTime
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class TourTimeTable
    {
        public int Id { get; set; }
        public int TourTimeId { get; set; }
        public int LocationId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class Image
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string Link { get; set; }
    }
}