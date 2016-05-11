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



            var dates = db.TourDate.Where(d => d.DateOfTour == tomorrow).ToList().Select(d => d.Id).ToArray(); //gets all tours which are tomorrow
            var dateTimeIds = db.TourDateTime.Where(dt => dates.Contains(dt.TourDateId)).ToList().Select(d => d.Id).ToArray(); //gets their tour date time Ids when compared to previous
            var getUserIds = db.Order.Where(o => dateTimeIds.Contains(o.TourDateTimeId)).ToList().Select(o => o.UserId).ToArray(); //gets User IDs when compared to previous
            var getUserPhoneNumbers = adc.Users.Where(u => getUserIds.Contains(u.Id)).ToList().Select(u => u.PhoneNumber).ToArray(); //gets phone numbers when compared to previous
            var getUserName = adc.Users.Where(u => getUserIds.Contains(u.Id)).ToList().Select(u => u.Name).ToArray(); //gets the name of the usre that booked the tour
            var getUserEmail = adc.Users.Where(u => getUserIds.Contains(u.Id)).ToList().Select(u => u.Email).ToArray(); //gets the email of the usre that booked the tour

            var getTourDateTimeID = db.Order.Where(o => getUserIds.Contains(o.UserId)).ToList().Select(o => o.TourDateTimeId).ToArray(); //gets the tour date time id
            var getTourID = db.TourDate.Where(td => getTourDateTimeID.Contains(td.Id)).ToList().Select(td => td.TourId).ToArray(); //gets the tourID
            var getTourName = db.Tour.Where(t => getTourID.Contains(t.Id)).ToList().Select(t => t.Name).ToArray(); //gets the Tour name



            int i = 0;
            foreach (string num in getUserPhoneNumbers)
            {

                long number = Convert.ToInt64(Regex.Replace(num, "[^0-9]", ""));
                string finalNum = Convert.ToString(number);
                finalNum = finalNum.Insert(0, "+");
                //uncommenting the following code will enable reminders via sms and emails
                /*
               var message = new IdentityMessage
                {
                    //Destination is hardcoded for testing purposes, the phone numbers should be retreived from the data base.
                    Destination = finalNum,
                    Body = "REMINDER: Dear " + getUserName[i] +", your booked tour: " + getTourName[i] + " is tomorrow! - Tours Maltin"
                };
                sms.SendAsync(message);
                
               ac.SendEmail(getUserEmail[i], "FULL INFO", finalNum + " REMINDER: Dear " + getUserName[i] + ", your booked tour: " + getTourName[i] + " is tomorrow! - Tours Maltin");
                i++;
                */
  }
  


        }
    }
}