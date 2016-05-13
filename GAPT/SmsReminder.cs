using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using GAPT.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;

namespace GAPT
{
    public class SmsReminder
    {
        private Controllers.AccountController ac = new Controllers.AccountController();
        private ToursDbContext db = new ToursDbContext();
        private ApplicationDbContext adc = new ApplicationDbContext();
        SmsService sms = new SmsService();

        //In order for this to work, users must place orders and their tours must be tomorrow (from any 'today')
        public void Reminder()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1.0);
            var tourDates = db.TourDate.Where(td => td.DateOfTour == tomorrow).ToList();
                var tourDateIds = tourDates.Select(t => t.Id).ToArray();
                var tourDateTimes = db.TourDateTime.Where(t => tourDateIds.Contains(t.TourDateId)).ToList();
                var tourDateTimeIds = tourDateTimes.Select(t => t.Id).ToArray();
                var orders = db.Order.Where(o => tourDateTimeIds.Contains(o.TourDateTimeId)).ToList();
                var userIds = orders.Select(o => o.UserId).ToArray();
                var users = adc.Users.Where(u => userIds.Contains(u.Id)).ToArray();

                var tourDateTimeByOrderIds = orders.Select(o => o.TourDateTimeId).ToArray();
                var tourDateByOrderIds = tourDateTimes.Where(t => tourDateTimeByOrderIds.Contains(t.Id)).ToList().Select(t => t.TourDateId).ToArray();
                var tourIds = tourDates.Where(t => tourDateByOrderIds.Contains(t.Id)).ToList().Select(t => t.TourId).ToArray();
                var tours = db.Tour.Where(t => tourIds.Contains(t.Id)).ToList();

                foreach (var tour in tours)
                {
                    var currTourDateIds = tourDates.Where(t => tourDateByOrderIds.Contains(t.Id) && t.TourId == tour.Id).ToList().Select(t => t.Id).ToArray();
                    var currTourDateTimeIds = tourDateTimes.Where(t => currTourDateIds.Contains(t.TourDateId)).ToList().Select(t => t.Id).ToArray();
                    var currTourUserIds = orders.Where(o => currTourDateTimeIds.Contains(o.TourDateTimeId)).ToList().Select(o => o.UserId).ToArray();
                    var currUsers = users.Where(u => currTourUserIds.Contains(u.Id)).ToList();

                    foreach (var user in currUsers)
                    {
                        long number = Convert.ToInt64(Regex.Replace(user.PhoneNumber, "[^0-9]", ""));
                        string finalNum = Convert.ToString(number);
                        finalNum = finalNum.Insert(0, "+");

                        //uncommenting the following code will enable reminders via sms and emails
                        /*
                        var message = new IdentityMessage
                        {
                            //Destination is hardcoded for testing purposes, the phone numbers should be retreived from the data base.
                            Destination = user.PhoneNumber,
                            Body = "REMINDER: Dear " + user.Name + ", your booked tour: " + tour.Name + " is tomorrow! - Tours Maltin"
                        };
                        sms.SendAsync(message);
                        */
                        ac.SendEmail(user.Email, "FULL INFO", " REMINDER: Dear " + user.Name + ", your booked tour: " + tour.Name + " is tomorrow! - Tours Maltin");
                    }

                }

            }
  


        }
    }
