using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCEmail.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GAPT.Models;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web.Security;
using System.Text;

namespace GAPT.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext appdb = new ApplicationDbContext();
        private ToursDbContext db = new ToursDbContext();
        List<ViewModelTour> ViewAllTours = new List<ViewModelTour>();
        public List<Tour> AllTours = new List<Tour>();

        public JsonResult GetTours(string term)
        {
            List<string> tours;
            tours = db.Tour.Where(t => t.Name.Contains(term)).Select(x => x.Name).ToList();
            return Json(tours, JsonRequestBehavior.AllowGet);
        }

        public void GetCategAttrTours()
        {
            try
            {
                AllTours = db.Tour.ToList();
                var categories = db.Category.ToList();
                var attrtypes = db.AttractionType.ToList();
                var alltours = db.Tour.ToList();
                var tourTimes = db.TourTime.ToList();
                var thumbImages = db.Image.Where(m => m.Link.Contains("rsz")).ToList();
                var islands = db.Island.ToList();
                var locations = db.Location.ToList();
                var locationattractions = db.LocationAttraction.ToList();
                var towns = db.Town.ToList();
                var timetables = db.TourTimeTable.ToList();
                var datetimes = db.TourDateTime.ToList();
                var times = db.TourTime.ToList();
                List<int> searchtexttourids = new List<int>();

                if (User.Identity.IsAuthenticated)
                {
                    var currentusername = User.Identity.Name;
                    var userid = appdb.Users.Where(u => u.UserName == currentusername).FirstOrDefault().Id;
                    //var user = HttpContext.User.Identity.
                    //var currentuserid = User.Identity.
                    //var currentuserid = Convert.ToInt32(Membership.GetUser().ProviderUserKey);
                    var curruserwishlist = db.WishList.Where(w => w.UserId == userid && w.Expired == false).ToList();
                    GlobalData.wishlist = curruserwishlist;
                }
                else
                {
                    GlobalData.wishlist = null;
                }

                foreach (Tour t in alltours)
                {
                    ViewModelTour currtour = new ViewModelTour()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        ShortDescription = t.ShortDescription,
                        LongDescription = t.LongDescription,
                        AdultPrice = t.AdultPrice,
                        ChildPrice = t.ChildPrice,
                        CategoryId = t.CategoryId,
                        AverageRatingId = t.AverageRatingId,
                        DateTimeCreated = t.DateTimeCreated,
                        MaxGroupSize = t.MaxGroupSize,
                        ThumbImage = thumbImages.FirstOrDefault(ti => ti.TourId == t.Id).Link
                    };

                    string st = tourTimes.FirstOrDefault(tt => tt.TourId == t.Id).StartTime;
                    string et = tourTimes.FirstOrDefault(tt => tt.TourId == t.Id).EndTime;

                    DateTime dt1 = DateTime.ParseExact(st, "HH:mm", new DateTimeFormatInfo());
                    DateTime dt2 = DateTime.ParseExact(et, "HH:mm", new DateTimeFormatInfo());
                    TimeSpan ts = dt2.Subtract(dt1);

                    currtour.Duration = ts.Hours.ToString() + "hrs";
                    currtour.Duration = ts.Minutes == 0 ? currtour.Duration : currtour.Duration + " " + ts.Minutes.ToString() + "mins";
                    ViewAllTours.Add(currtour);
                }

                GlobalData.priceslist = new List<decimal[]>();

                decimal[] price1 = new decimal[2];
                decimal[] price2 = new decimal[2];
                decimal[] price3 = new decimal[2];

                price1[0] = 0;
                price1[1] = 10;
                price2[0] = 10;
                price2[1] = 20;
                price3[0] = 20;
                price3[1] = 30;

                GlobalData.priceslist.Add(price1);
                GlobalData.priceslist.Add(price2);
                GlobalData.priceslist.Add(price3);

                if (GlobalData.selectedcategories != null)
                    GlobalData.tours = ViewAllTours.Where(t => GlobalData.selectedcategories.Contains(t.CategoryId)).ToList();
                else if (GlobalData.searchtext != null)
                {
                    //GlobalData.selectedcategories = null;
                    string[] words = GlobalData.searchtext.Split(' ');
                    foreach (string word in words)
                    {
                        //var searchtourids = alltours.Where(t => t.Name.Contains(word) || t.ShortDescription.Contains(word) || t.LongDescription.Contains(word)).ToList().Select(tt => tt.Id).ToArray();
                        var categoryids = categories.Where(t => t.Name.ToLower().Contains(word)).ToList().Select(c => c.Id).ToArray();
                        //var categtourids = alltours.Where(t => categoryids.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                        var islandids = islands.Where(i => i.Name.ToLower().Contains(word)).ToList().Select(t => t.Id).ToArray();
                        var attractionids = attrtypes.Where(a => a.Name.ToLower().Contains(word)).ToList().Select(aa => aa.Id).ToArray();
                        var locattrids = locationattractions.Where(la => attractionids.Contains(la.AttractionTypeId)).ToList().Select(l => l.LocationId).ToArray();
                        var townids = towns.Where(t => islandids.Contains(t.IslandId) || t.Name.ToLower().Contains(word)).ToList().Select(tt => tt.Id).ToArray();
                        var locationids = locations.Where(l => locattrids.Contains(l.Id) || townids.Contains(l.TownId)).ToList().Select(ll => ll.Id).ToArray();

                        var timeids = timetables.Where(t => locationids.Contains(t.LocationId)).ToList().Select(tt => tt.TourTimeId).ToArray();
                        var timetourids = times.Where(t => timeids.Contains(t.TourId)).ToList().Select(tt => tt.TourId).ToArray();

                        var searchtourids = alltours.Where(t => t.Name.ToLower().Contains(word) || t.ShortDescription.ToLower().Contains(word) || t.LongDescription.ToLower().Contains(word) || categoryids.Contains(t.CategoryId) || timetourids.Contains(t.Id)).ToList().Select(tt => tt.Id).ToArray();

                        foreach (var id in searchtourids)
                        {
                            if (!searchtexttourids.Contains(id))
                                searchtexttourids.Add(id);
                        }
                    }
                    //GlobalData.searchtext = null;
                    GlobalData.tours = ViewAllTours.Where(t => searchtexttourids.Contains(t.Id)).ToList();
                }
                else
                    GlobalData.tours = ViewAllTours;

                GlobalData.attractionTypes = attrtypes;
                GlobalData.categories = categories;
                //GlobalData.tours = ViewAllTours;
                GlobalData.alltours = ViewAllTours;
                GlobalData.islands = islands;

                ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, selectedcategory = GlobalData.selectedcategories, wishlist = GlobalData.wishlist };

                //return model;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message.ToString());
            }
            //finally
            //{
            //    GlobalData.selectedcategories = null;
            //    GlobalData.searchtext = null;
            //}
            //return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("egalea.11@gmail.com"));  // replace with valid value 
                message.From = new MailAddress("toursmaltin@gmail.com");  // replace with valid value
                message.Subject = "Your email subject";
                message.Body = string.Format(body, model.FromName, model.FromEmail, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "toursmaltin@gmail.com",  // replace with valid value
                        Password = "cis2104.groupAPT"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Sent");
                }
            }
            return View(model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchSortby(string sortby)
        {
            switch (sortby)
            {
                case "bestmatch":
                    break;
                case "name":
                    var toursbyname = GlobalData.tours.OrderBy(t => t.Name).ToList();
                    GlobalData.tours = toursbyname;
                    break;
                case "popular":
                    var toursbypopular = GlobalData.tours.OrderBy(t => t.AverageRatingId).ToList();
                    GlobalData.tours = toursbypopular;
                    break;
                case "pricelow":
                    var toursbypricelow = GlobalData.tours.OrderBy(t => t.AdultPrice).ToList();
                    GlobalData.tours = toursbypricelow;
                    break;
                case "pricehigh":
                    var toursbypricehigh = GlobalData.tours.OrderByDescending(t => t.AdultPrice).ToList();
                    GlobalData.tours = toursbypricehigh;
                    break;
            }

            ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, wishlist = GlobalData.wishlist };

            return PartialView("SearchTours", model);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CookiePolicy()
        {
            return View();
        }

        public ActionResult RemoveFromWishlist(IEnumerable<int> id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // check is already done on client side but should be added here as well
                Response.StatusCode = 401;
                Response.End();
            }

            if (id == null)
                return Json(false);


            var wishlists = db.WishList.ToList();
            var currentusername = User.Identity.Name;
            var userid = appdb.Users.Where(u => u.UserName == currentusername).FirstOrDefault().Id;
            var curruserwishlist = wishlists.Where(w => w.UserId == userid && w.TourId == id.First() && w.Expired == false).ToList();

            if (curruserwishlist.Count != 0)
            {
                db.WishList.Remove(curruserwishlist.FirstOrDefault());
                db.SaveChangesAsync();
                GlobalData.wishlist = wishlists.Where(w => w.UserId == userid).ToList();
                return Json(true);
            }
            return Json(false);
        }
        public ActionResult AddToWishlist(IEnumerable<int> id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // check is already done on client side but should be added here as well
                Response.StatusCode = 401;
                Response.End();
            }

            if(id == null)
                return Json(false);

            var wishlists = db.WishList.Where(w => w.Expired != true).ToList();
            var currentusername = User.Identity.Name;
            var userid = appdb.Users.Where(u => u.UserName == currentusername).FirstOrDefault().Id;
            var curruserwishlist = wishlists.Where(w => w.UserId == userid && w.TourId == id.First()).ToList();

            if (curruserwishlist.Count == 0)
            {
                GAPT.Models.WishList wishlist = new WishList
                {
                    TourId = id.First(),
                    DateTimeCreated = DateTime.Now,
                    UserId = userid,
                    Expired = false
                };
                //string query = "INSERT INTO [WishList] (TourId, DateTimeCreated, UserId) VALUES (\"" 
                //                + wishlist.TourId + "\",\"" + wishlist.DateTimeCreated + "\",\"" + wishlist.UserId + "\");";
                db.WishList.Add(wishlist);
                db.SaveChangesAsync();

                GlobalData.wishlist = wishlists.Where(w => w.UserId == userid).ToList();
                return Json(true);
            }
            return Json(false);         
        }

        public ActionResult OrderConfirmation(CustomerInfoModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // check is already done on client side but should be added here as well
                Response.StatusCode = 401;
                Response.End();
            }

            CustomerInfoModel orderModel = (CustomerInfoModel)Session["PaymentModel"];
            var currUserName = User.Identity.Name;
            var userId = appdb.Users.Where(u => u.UserName == currUserName).FirstOrDefault().Id;
            
            Order order = new Order() 
            { 
                AdultQuantity = orderModel.AdultAmount,
                ChildQuantity = orderModel.ChildAmount,
                DateTimeCreated = DateTime.Now,
                TotalPrice = orderModel.TotalPrice,
                UserId = userId,
                TourDateTimeId = orderModel.TourDateTimeId
            };

            db.Order.Add(order);
            db.SaveChanges();
            var orderId = order.Id;
            //var orderId = db.Order.Where(o => o.UserId == userId && o.DateTimeCreated == order.DateTimeCreated).FirstOrDefault().Id;

            foreach (var att in orderModel.AdultDetails)
            {
                TourAttendees attendeeAdult = new TourAttendees() 
                { 
                    Name = att.FirstName,
                    Surname = att.LastName,
                    Title = att.Title,
                    OrderId = (int)orderId,
                    DateTimeCreated = DateTime.Now,
                    IsAdult = true,
                };

                db.TourAttendees.Add(attendeeAdult);
                db.SaveChanges();
            }
            foreach (var att in orderModel.ChildDetails)
            {
                int month = DateTime.ParseExact(att.BirthMonth.ToString(), "MMMM", CultureInfo.InvariantCulture).Month;

                TourAttendees attendeeChild = new TourAttendees()
                {
                    Name = att.FirstName,
                    Surname = att.LastName,
                    Title = att.Title,
                    OrderId = (int)orderId,
                    DateTimeCreated = DateTime.Now,
                    IsAdult = false,
                    DateOfBirth = (DateTime)new DateTime(att.BirthYear, month, att.BirthDay)
                };

                db.TourAttendees.Add(attendeeChild);
                db.SaveChanges();
            }

            Session["PaymentModel"] = null;
            Session["Tourpage"] = null;
            return View(orderModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(CustomerInfoModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // check is already done on client side but should be added here as well
                Response.StatusCode = 401;
                Response.End();
            }

            if (model.BackToTour == true)
                return RedirectToAction("Tourpage");

            CustomerInfoModel paymentModel = new CustomerInfoModel()
            {
                Tour = db.Tour.Where(t => t.Id == model.TourId).FirstOrDefault(),
                TourId = model.TourId,
                TourDateId = model.TourDateId,
                TourDate = db.TourDate.Where(t => t.Id == model.TourDateId).FirstOrDefault(),
                TourTimeId = model.TourTimeId,
                TourTime = db.TourTime.Where(t => t.Id == model.TourTimeId).FirstOrDefault(),
                TourDateTimeId = model.TourDateTimeId,
                TourDateTime = db.TourDateTime.Where(t => t.Id == model.TourDateTimeId).FirstOrDefault(),
                TourStartingLocation = model.TourStartingLocation,
                AdultAmount = model.AdultAmount,
                ChildAmount = model.ChildAmount,
                ChildTotalPrice = model.ChildTotalPrice,
                AdultTotalPrice = model.AdultTotalPrice,
                TotalPrice = model.TotalPrice,
                ChildDetails = model.ChildDetails,
                AdultDetails = model.AdultDetails
            };

            paymentModel.StringTourTime = paymentModel.TourTime.StartTime + "-" + paymentModel.TourTime.EndTime;

            Session["PaymentModel"] = paymentModel;

            return View(paymentModel);
        }

        [HttpPost]
        public JsonResult GetJsonDates(IEnumerable<int> tourId)
        {
            if (tourId == null || tourId.Count() == 0)
            {
                // Argument not passed
                // Reponse 400 Bad Request
                Response.StatusCode = 400;
                Response.End();
            }
            var tourDates = db.TourDate.Where(d => d.TourId == tourId.FirstOrDefault() && d.DateOfTour > DateTime.Now).Select(d => d.DateOfTour).ToArray();

            if (tourDates == null || tourDates.Count() == 0)
                return null;

            var jsonString = new StringBuilder();
            List<string> dates = new List<string>();

            for (int i = 0; i < tourDates.Count(); i++)
            {
                var tempDate = tourDates[i];
                DateTime temp = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day);
                string str = temp.ToString("yyyy-MM-dd");
                dates.Add(str);
            }

            return this.Json(dates, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetJsonLocations()
        {
            int? tourId = (int?)Session["TourId"];

            if (tourId == null || tourId == 0)
            {
                // Argument not passed
                // Reponse 400 Bad Request
                return null;
                //Response.StatusCode = 400;
                //Response.End();
            }

            var tourTimes = db.TourTime.Where(t => t.TourId == tourId).ToList();
            var tourTimeIds = tourTimes.Select(t => t.Id).ToArray();
            var tourDates = db.TourDate.Where(t => t.TourId == tourId && t.DateOfTour > DateTime.Now).ToList();
            var tourDateIds = tourDates.Select(t => t.Id).ToArray();
            var tourDateTimes = db.TourDateTime.Where(t => tourDateIds.Contains(t.Id)).ToList();
            var tourTimeTables = db.TourTimeTable.Where(t => tourTimeIds.Contains(t.TourTimeId)).ToList();
            var tourLocationIds = tourTimeTables.Select(t => t.LocationId).ToArray();
            var tourLocations = db.Location.Where(l => tourLocationIds.Contains(l.Id)).ToList();

            var jsonString = new StringBuilder();

            //List<string> dates = new List<string>();

            //for (int i = 0; i < tourDates.Count(); i++)
            //{
            //    var tempDate = tourDates[i];
            //    DateTime temp = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day);
            //    string str = temp.ToString("yyyy-MM-dd");
            //    dates.Add(str);
            //}
            //var jsonS = Json(tourLocations).ToString();
            //Trace.TraceInformation(Json(tourLocations).ToString());
            return this.Json(tourLocations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CustomerInfo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // check is already done on client side but should be added here as well
                Response.StatusCode = 401;
                Response.End();
            }

            CustomerInfoModel orderModel = (CustomerInfoModel)Session["PaymentModel"];
            if (orderModel != null)
                return View(orderModel);

            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerInfo(TourpageModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // check is already done on client side but should be added here as well
                Response.StatusCode = 401;
                Response.End();
            }

            CustomerInfoModel orderModel = (CustomerInfoModel)Session["PaymentModel"];
            if (orderModel != null)
                return View(orderModel);

            var tourDetails = db.Tour.Where(t => t.Id == model.Tour.Id).FirstOrDefault();
            decimal totalAdultPrice = model.AdultAmount * tourDetails.AdultPrice;
            decimal totalChildPrice = model.ChildAmount * tourDetails.ChildPrice;
            decimal totalPrice = totalAdultPrice + totalChildPrice;

            string fromFormat = "MM/dd/yyyy";
            DateTime DateOfTour = DateTime.ParseExact(model.StringTourDate, fromFormat, CultureInfo.InvariantCulture);

            var tourDate = db.TourDate.Where(t => t.TourId == model.Tour.Id && t.DateOfTour == DateOfTour).FirstOrDefault();
            
            string[] times = model.TourTime.Split('-');
            string startTime = times[0];
            string endTime = times[1];
            var tourTime = db.TourTime.Where(t => t.TourId == model.Tour.Id && t.StartTime == startTime && t.EndTime == endTime).FirstOrDefault();
            var tourDateTime = db.TourDateTime.Where(t => t.TourDateId == tourDate.Id && t.TourTimeId == tourTime.Id).FirstOrDefault();
            var tourLocationId = db.TourTimeTable.Where(t => t.TourTimeId == tourTime.Id && t.StartTime == tourTime.StartTime).FirstOrDefault().LocationId;
            var tourLocationName = db.Location.Where(l => l.Id == tourLocationId).FirstOrDefault().Name;

            CustomerInfoModel customerModel = new CustomerInfoModel()
            {
                AdultAmount = model.AdultAmount,
                ChildAmount = model.ChildAmount,
                AdultTotalPrice = totalAdultPrice,
                ChildTotalPrice = totalChildPrice,
                TotalPrice = totalPrice,
                Tour = tourDetails,
                TourDate = tourDate,
                TourTime = tourTime,
                TourDateTime = tourDateTime,
                TourStartingLocation = tourLocationName,
                TourDateId = tourDate.Id,
                TourTimeId = tourTime.Id,
                TourDateTimeId = tourDateTime.Id
            };

            customerModel.AdultDetails = new List<AdultDetails>();
            for (int i = 0; i < model.AdultAmount; i++)
            {
                AdultDetails adult = new AdultDetails() 
                {
                    Id = i + 1
                };
                customerModel.AdultDetails.Add(adult);
            }

            if (model.ChildAmount == 0)
                return View(customerModel);

            customerModel.ChildDetails = new List<ChildDetails>();
            for (int i = 0; i < model.ChildAmount; i++)
            {
                ChildDetails child = new ChildDetails()
                {
                    Id = i + 1
                };
                customerModel.ChildDetails.Add(child);
            }

            var category = db.Category.Where(c => c.Id == customerModel.Tour.CategoryId).FirstOrDefault().Name;
            var tourTimes = db.TourTime.Where(t => t.TourId == customerModel.Tour.Id).ToList();
            var tourTimeIds = tourTimes.Select(t => t.Id).ToArray();
            var tourDates = db.TourDate.Where(t => t.TourId == customerModel.Tour.Id && t.DateOfTour > DateTime.Now).ToList();
            var tourDateIds = tourDates.Select(t => t.Id).ToArray();
            var tourDateTimes = db.TourDateTime.Where(t => tourDateIds.Contains(t.Id)).ToList();
            var tourTimeTables = db.TourTimeTable.Where(t => tourTimeIds.Contains(t.TourTimeId)).ToList();
            var tourLocationIds = tourTimeTables.Select(t => t.LocationId).ToArray();
            var tourLocations = db.Location.Where(l => tourLocationIds.Contains(l.Id)).ToList();
            var tourImages = db.Image.Where(i => i.TourId == customerModel.Tour.Id && !i.Link.Contains("rsz")).ToList();

            TourpageModel tourpageModel = new TourpageModel() 
            {
                Tour = customerModel.Tour,
                AdultAmount = model.AdultAmount,
                ChildAmount = model.ChildAmount,
                TourDate = tourDate.DateOfTour,
                TourTime = tourTime.StartTime + "-" + tourTime.EndTime,
                TourDateId = tourDate.Id,
                TourTimeId = tourTime.Id,
                TourCategory = category,
                TourTimes = tourTimes,
                TourDates = tourDates,
                TourDateTimes = tourDateTimes,
                TourTimeTables = tourTimeTables,
                TourLocations = tourLocations,
                Images = tourImages,
            };

            if (User.Identity.IsAuthenticated)
            {
                var currentUserName = User.Identity.Name;
                var userId = appdb.Users.Where(u => u.UserName == currentUserName).FirstOrDefault().Id;
                var currUserWishlist = db.WishList.Where(w => w.UserId == userId && w.Expired == false).ToList();
                tourpageModel.Wishlists = currUserWishlist;
            }

            Session["Tourpage"] = tourpageModel;

            return View(customerModel);
        }

        [HttpPost]
        public string GetDateOfTour(IEnumerable<int> tourId) //, IEnumerable<int> tourId    dataType: "json"
        {
            if (tourId == null || tourId.Count() == 0)
            {
                // Argument not passed
                // Reponse 400 Bad Request
                Response.StatusCode = 400;
                Response.End();
            }
            var tourDate = db.TourDate.Where(d => d.TourId == tourId.FirstOrDefault() && d.DateOfTour > DateTime.Now).FirstOrDefault().DateOfTour;
            DateTime temp = new DateTime(tourDate.Year, tourDate.Month, tourDate.Day);
            string str = temp.ToString("MM/dd/yyyy");

            return str;
        }

        [HttpPost]
        public string GetTourTime(IEnumerable<string> tourDate)
        {
            if (tourDate == null || tourDate.Count() == 0)
            {
                // Argument not passed
                // Reponse 400 Bad Request
                Response.StatusCode = 400;
                Response.End();
            }

            string[] TourDateAndId = tourDate.FirstOrDefault().Split(':');
            int tourId = Convert.ToInt32(TourDateAndId[1]);

            //DateTime temp = Convert.ToDateTime(TourDateAndId[0]);
            //string Temp = temp.ToShortDateString();
            string fromFormat = "MM/dd/yyyy";
            DateTime DateOfTour = DateTime.ParseExact(TourDateAndId[0], fromFormat, CultureInfo.InvariantCulture);

            var tourDateId = db.TourDate.Where(d => d.TourId == tourId && d.DateOfTour == DateOfTour).FirstOrDefault();

            if (tourDateId == null)
                return "false";

            var getDateId = tourDateId.Id;

            var tourTimesIds = db.TourDateTime.Where(dt => dt.TourDateId == getDateId).ToList().Select(t => t.TourTimeId).ToArray();
            var tourTimes = db.TourTime.Where(t => tourTimesIds.Contains(t.Id)).ToList();

            string timeOptions = "";

            for (int k = 0; k < tourTimes.Count; k++)
            {
                string time = tourTimes.ElementAt(k).StartTime + "-" + tourTimes.ElementAt(k).EndTime;
                if (k == 0)
                    timeOptions = "<option value=\"" + time + "\" selected=\"selected\">" + time + "</option>";
                else
                    timeOptions = timeOptions + "<option value=\"" + time + "\" selected=\"selected\">" + time + "</option>";
            }
            return timeOptions;
        }

        [HttpPost]
        public int GetPlacesAvailableCustomerInfo(IEnumerable<string> tourDateTime)
        {
            if (tourDateTime == null || tourDateTime.Count() == 0)
            {
                // Argument not passed
                // Reponse 400 Bad Request
                Response.StatusCode = 400;
                Response.End();
            }

            string[] TourDateAndTimeAndId = tourDateTime.FirstOrDefault().Split(';');
            int tourId = Convert.ToInt32(TourDateAndTimeAndId[2]);

            //DateTime temp = Convert.ToDateTime(TourDateAndTimeAndId[0]);
            //string Temp = temp.ToShortDateString();
            //string fromFormat = "MM/dd/yyyy";
            string fromFormat = "dd/MM/yyyy";
            DateTime DateOfTour = DateTime.ParseExact(TourDateAndTimeAndId[0], fromFormat, CultureInfo.InvariantCulture);

            string[] stringTime = TourDateAndTimeAndId[1].Split('-');
            string startTime = stringTime[0];
            string endTime = stringTime[1];
            int maxTourGroupSize = db.Tour.Where(t => t.Id == tourId).FirstOrDefault().MaxGroupSize;

            var tourDateId = db.TourDate.Where(d => d.TourId == tourId && d.DateOfTour == DateOfTour).FirstOrDefault().Id;
            var tourTimeIds = db.TourTime.Where(t => t.TourId == tourId && t.StartTime == startTime && t.EndTime == endTime).ToList().Select(t => t.Id).ToArray();
            var tourDateTimeIds = db.TourDateTime.Where(dt => dt.TourDateId == tourDateId && tourTimeIds.Contains(dt.TourTimeId)).ToList().Select(t => t.Id).ToArray();
            var orders = db.Order.Where(o => tourDateTimeIds.Contains(o.TourDateTimeId)).ToList();

            int totalAdultAmount = 0;
            int totalChildAmount = 0;
            foreach (var order in orders)
            {
                totalAdultAmount = totalAdultAmount + order.AdultQuantity;
                totalChildAmount = totalChildAmount + order.ChildQuantity;
            }

            int totalGroupSize = totalAdultAmount + totalChildAmount;
            int placesLeft = maxTourGroupSize - totalGroupSize;

            return placesLeft;
        }

        [HttpPost]
        public int GetPlacesAvailable(IEnumerable<string> tourDateTime)
        {
            if (tourDateTime == null || tourDateTime.Count() == 0)
            {
                // Argument not passed
                // Reponse 400 Bad Request
                Response.StatusCode = 400;
                Response.End();
            }

            string[] TourDateAndTimeAndId = tourDateTime.FirstOrDefault().Split(';');
            int tourId = Convert.ToInt32(TourDateAndTimeAndId[2]);

            //DateTime temp = Convert.ToDateTime(TourDateAndTimeAndId[0]);
            //string Temp = temp.ToShortDateString();
            string fromFormat = "MM/dd/yyyy";
            DateTime DateOfTour = DateTime.ParseExact(TourDateAndTimeAndId[0], fromFormat, CultureInfo.InvariantCulture);

            string[] stringTime = TourDateAndTimeAndId[1].Split('-');
            string startTime = stringTime[0];
            string endTime = stringTime[1];
            int maxTourGroupSize = db.Tour.Where(t => t.Id == tourId).FirstOrDefault().MaxGroupSize;

            var tourDateId = db.TourDate.Where(d => d.TourId == tourId && d.DateOfTour == DateOfTour).FirstOrDefault().Id;
            var tourTimeIds = db.TourTime.Where(t => t.TourId == tourId && t.StartTime == startTime && t.EndTime == endTime).ToList().Select(t => t.Id).ToArray();
            var tourDateTimeIds = db.TourDateTime.Where(dt => dt.TourDateId == tourDateId && tourTimeIds.Contains(dt.TourTimeId)).ToList().Select(t => t.Id).ToArray();
            var orders = db.Order.Where(o => tourDateTimeIds.Contains(o.TourDateTimeId)).ToList();

            int totalAdultAmount = 0;
            int totalChildAmount = 0;
            foreach (var order in orders)
            {
                totalAdultAmount = totalAdultAmount + order.AdultQuantity;
                totalChildAmount = totalChildAmount + order.ChildQuantity;
            }

            int totalGroupSize = totalAdultAmount + totalChildAmount;
            int placesLeft = maxTourGroupSize - totalGroupSize;

            return placesLeft;
        }

        public ActionResult PasswordReminder()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public PartialViewResult Reviews(int id)
        {
            var reviews = db.Review.Where(r => r.TourId == id);
            var userIds = reviews.Select(r => r.UserId).ToArray();
            var reviewUsers = appdb.Users.Where(u => userIds.Contains(u.Id)).ToList();
            ReviewViewModel model = new ReviewViewModel();
            model.Reviews = new List<ReviewModel>();

            foreach (var r in reviews)
            {
                ReviewModel reviewModel = new ReviewModel()
                {
                    Id = r.Id,
                    RatingId = r.RatingId,
                    Comment = r.Comment,
                    DateTimeCreated = r.DateTimeCreated,
                    TourId = r.TourId,
                    Username = reviewUsers.Where(u => u.Id == r.UserId).FirstOrDefault().UserName
                };

                model.Reviews.Add(reviewModel);
            }

            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Reviews()
        {
            TourpageModel sessionModel = (TourpageModel)Session["Tourpage"];
            if (sessionModel == null)
                return RedirectToAction("Error", "Home");

            var reviews = db.Review.Where(r => r.TourId == sessionModel.Tour.Id);
            var userIds = reviews.Select(r => r.UserId).ToArray();
            var reviewUsers = appdb.Users.Where(u => userIds.Contains(u.Id)).ToList();
            ReviewViewModel model = new ReviewViewModel();
            model.Reviews = new List<ReviewModel>();

            foreach (var r in reviews)
            {
                ReviewModel reviewModel = new ReviewModel()
                {
                    Id = r.Id,
                    RatingId = r.RatingId,
                    Comment = r.Comment,
                    DateTimeCreated = r.DateTimeCreated,
                    TourId = r.TourId,
                    Username = reviewUsers.Where(u => u.Id == r.UserId).FirstOrDefault().UserName
                };

                model.Reviews.Add(reviewModel);
            }

            return PartialView(model);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ReviewTour(int id)
        {
            Review model = new Review() 
            {
                TourId = id
            };

            return PartialView("ReviewTour", model);
        }

        [HttpGet]
        public ActionResult ReviewTour()
        {
            TourpageModel sessionModel = (TourpageModel)Session["Tourpage"];
            if (sessionModel == null)
                return RedirectToAction("Error", "Home");

            Review model = new Review()
            {
                TourId = sessionModel.Tour.Id
            };

            return PartialView("ReviewTour", model);
        }
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RecalculateAverageRating(IEnumerable<int> tourId) 
        {
            var ratings = db.Review.Where(r => r.TourId == tourId.FirstOrDefault() && r.RatingId != 6).ToList().Select(r => r.RatingId).ToArray();
            int sumRating = 0;

            foreach (var r in ratings)
                sumRating += Convert.ToInt32(r);

            float averageRating;
            if (ratings.Count() == 0)
            {
               averageRating = ratings.Count();
            }
            averageRating = sumRating / ratings.Count();

            int tourRating = (int)averageRating;

            try 
            {
                db.Database.ExecuteSqlCommand("update [Tour] set [AverageRatingId] = @p1 where [Id] = @p2",
                new System.Data.SqlClient.SqlParameter("p1", tourRating),
                new System.Data.SqlClient.SqlParameter("p2", tourId.FirstOrDefault()));
            }
            catch(Exception e) 
            {
                Trace.TraceError(e.Message);
            }

            var AverageRatingId = db.Tour.Where(t => t.Id == tourId.FirstOrDefault()).FirstOrDefault().AverageRatingId;
            
            AverageRatingModel model = new AverageRatingModel() 
            {
                AverageRating = AverageRatingId
            };
            return PartialView("TourpageAverageRating", model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> InsertReview(IEnumerable<string> review) //async Task<ActionResult>
        {
            string[] result = review.FirstOrDefault().Split(';');
            int tourId = Convert.ToInt32(result[0]);
            var comment = result[1];
            int? ratingId = null;

            if(result[2] != null && result[2] != "")
                ratingId = Convert.ToInt32(result[2]);

            if (review != null)
            {
                var currentusername = User.Identity.Name;
                var userid = appdb.Users.Where(u => u.UserName == currentusername).FirstOrDefault().Id;

                Review reviewToPost = new Review() 
                {
                    TourId = tourId,
                    Comment = comment,
                    RatingId = ratingId == null ? 6 : ratingId,
                    DateTimeCreated = DateTime.Now,
                    UserId = userid
                };

                db.Review.Add(reviewToPost);
                //db.SaveChanges();
                await db.SaveChangesAsync();
            }

            var reviews = db.Review.Where(r => r.TourId == tourId);
            var userIds = reviews.Select(r => r.UserId).ToArray();
            var reviewUsers = appdb.Users.Where(u => userIds.Contains(u.Id)).ToList();
            ReviewViewModel model = new ReviewViewModel();
            model.Reviews = new List<ReviewModel>();

            foreach (var r in reviews)
            {
                ReviewModel reviewModel = new ReviewModel()
                {
                    Id = r.Id,
                    RatingId = r.RatingId,
                    Comment = r.Comment,
                    DateTimeCreated = r.DateTimeCreated,
                    TourId = r.TourId,
                    Username = reviewUsers.Where(u => u.Id == r.UserId).FirstOrDefault().UserName
                };

                model.Reviews.Add(reviewModel);
            }

            return PartialView("Reviews", model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TourpageAverageRating(int tourId)
        {
            var ratingId = db.Tour.Where(t => t.Id == tourId).FirstOrDefault().AverageRatingId;

            AverageRatingModel model = new AverageRatingModel() 
            { 
                AverageRating = ratingId
            };
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult TourpageAverageRating()
        {
            TourpageModel sessionModel = (TourpageModel)Session["Tourpage"];
            if (sessionModel == null)
                return RedirectToAction("Error", "Home");

            var ratingId = db.Tour.Where(t => t.Id == sessionModel.Tour.Id).FirstOrDefault().AverageRatingId;

            AverageRatingModel model = new AverageRatingModel()
            {
                AverageRating = ratingId
            };
            return PartialView(model);
            
        }

        [AllowAnonymous]
        public ActionResult SearchToursAverageRating(int tourId)
        {
            var ratingId = db.Tour.Where(t => t.Id == tourId).FirstOrDefault().AverageRatingId;

            AverageRatingModel model = new AverageRatingModel()
            {
                AverageRating = ratingId
            };
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult Tourpage()
        {
            TourpageModel sessionModel = (TourpageModel)Session["Tourpage"];
            if (sessionModel != null)
            {
                Session["TourId"] = sessionModel.Tour.Id;
                return View(sessionModel);
            }
            return RedirectToAction("Error","Home");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tourpage(int id)
        {
            Session["TourId"] = id;

            TourpageModel sessionModel = (TourpageModel)Session["Tourpage"];
            if (sessionModel != null)
                return View(sessionModel);

            //var id = 1;
            var tour = db.Tour.Where(t => t.Id == id).FirstOrDefault();

            if (tour == null)
                return RedirectToAction("Error", "Home");

            var category = db.Category.Where(c => c.Id == tour.CategoryId).FirstOrDefault().Name;
            var tourTimes = db.TourTime.Where(t => t.TourId == id).ToList();
            var tourTimeIds = tourTimes.Select(t => t.Id).ToArray();
            var tourDates = db.TourDate.Where(t => t.TourId == id && t.DateOfTour > DateTime.Now).ToList();
            var tourDateIds = tourDates.Select(t => t.Id).ToArray();
            var tourDateTimes = db.TourDateTime.Where(t => tourDateIds.Contains(t.Id)).ToList();
            var tourTimeTables = db.TourTimeTable.Where(t => tourTimeIds.Contains(t.TourTimeId)).ToList();
            var tourLocationIds = tourTimeTables.Select(t => t.LocationId).ToArray();
            var tourLocations = db.Location.Where(l => tourLocationIds.Contains(l.Id)).ToList();
            var tourImages = db.Image.Where(i => i.TourId == id && !i.Link.Contains("rsz")).ToList();

            TourpageModel model = new TourpageModel
            {
                Tour = tour,
                TourCategory = category,
                TourTimes = tourTimes,
                TourDates = tourDates,
                TourDateTimes = tourDateTimes,
                TourTimeTables = tourTimeTables,
                TourLocations = tourLocations,
                Images = tourImages,
            };

            if (User.Identity.IsAuthenticated)
            {
                var currentUserName = User.Identity.Name;
                var userId = appdb.Users.Where(u => u.UserName == currentUserName).FirstOrDefault().Id;
                var currUserWishlist = db.WishList.Where(w => w.UserId == userId && w.Expired == false).ToList();
                model.Wishlists = currUserWishlist;
            }

            return View("Tourpage", model);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Search()
        {
            try
            {
                GetCategAttrTours();
                //GlobalData.selectedcategories = null;
                ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, selectedcategory = GlobalData.selectedcategories, wishlist = GlobalData.wishlist };
                ViewData["CategAttrTours"] = model;
                //GlobalData.searchtext = null;
                return View(model);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }
            finally
            {
                GlobalData.selectedcategories = null;
                GlobalData.selectedattr = null;
                GlobalData.searchtext = null;
                GlobalData.selectedislands = null;
                GlobalData.selectedmonths = null;
                GlobalData.toprice = null;
                GlobalData.fromprice = null;
            }
            return View();
        }

        public PartialViewResult HomeCategDropDown()
        {
            ViewModelCategory model = new ViewModelCategory();
            model.categories = db.Category.ToList();
            return PartialView(model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilter(string searchtext)
        {
            //GetCategAttrTours();
            GlobalData.selectedcategories = null;
            GlobalData.searchtext = searchtext.ToLower();
            //ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, selectedcategory = GlobalData.selectedcategories };
            //return View("Search", model);
            return RedirectToAction("Search");
            //Search();
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchCategory(IEnumerable<int> id)
        {
            GlobalData.selectedcategories = id;
            return RedirectToAction("Search");
            //return View();
        }

        public ActionResult ShoppingCart()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Sent()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterIslands(IEnumerable<int> selectedislands)
        {
            GlobalData.selectedislands = null;
            var alltowns = db.Town.ToList();
            var alllocations = db.Location.ToList();
            var alltimetables = db.TourTimeTable.ToList();
            var alldatetimes = db.TourDateTime.ToList();
            var alltimes = db.TourTime.ToList();
            var alldates = db.TourDate.ToList();
            GlobalData.tours = GlobalData.alltours;
            IEnumerable<int> toursbysearch = null;

            if (selectedislands == null)
            {
                GlobalData.selectedislands = null;
            }
            else
            {
                GlobalData.selectedislands = selectedislands;
                var townsbyislandids = alltowns.Where(t => GlobalData.selectedislands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsbytownids = alllocations.Where(l => townsbyislandids.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timebyislandids = alltimetables.Where(t => locationsbytownids.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursbyislandids = alltimes.Where(t => timebyislandids.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                toursbysearch = toursbyislandids;
            }
            if (GlobalData.selectedcategories != null)
            {
                var toursbycategids = GlobalData.alltours.Where(t => GlobalData.selectedcategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                if (toursbysearch == null)
                    toursbysearch = toursbycategids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbycategids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedattr != null)
            {
                var locationattrs = db.LocationAttraction.Where(l => GlobalData.selectedattr.Contains(l.AttractionTypeId)).ToList();
                var locationbyattrsids = db.LocationAttraction.Where(l => GlobalData.selectedattr.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timebyattrids = alltimetables.Where(t => locationbyattrsids.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourbyattrids = alltimes.Where(t => timebyattrids.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                if (toursbysearch == null)
                    toursbysearch = tourbyattrids;
                else
                {
                    var searchres = toursbysearch.Where(t => tourbyattrids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }

            if (GlobalData.selectedmonths != null)
            {
                var toursbymonthids = alldates.Where(d => GlobalData.selectedmonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                if (toursbysearch == null)
                    toursbysearch = toursbymonthids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbymonthids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.fromprice != null && GlobalData.toprice != null)
            {
                var toursbypriceids = GlobalData.tours.Where(t => t.AdultPrice <= GlobalData.toprice && t.AdultPrice >= GlobalData.fromprice).ToList().Select(tt => tt.Id).ToArray();
                if (toursbysearch == null)
                    toursbysearch = toursbypriceids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbypriceids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }

            if (toursbysearch != null)
                GlobalData.tours = GlobalData.alltours.Where(t => toursbysearch.Contains(t.Id)).ToList();

            ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, wishlist = GlobalData.wishlist };

            return PartialView("SearchTours", model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterCategories(IEnumerable<int> selectedcategories)
        {
            GlobalData.selectedcategories = null;
            var alltowns = db.Town.ToList();
            var alllocations = db.Location.ToList();
            var alltimetables = db.TourTimeTable.ToList();
            var alldatetimes = db.TourDateTime.ToList();
            var alltimes = db.TourTime.ToList();
            var alldates = db.TourDate.ToList();
            GlobalData.tours = GlobalData.alltours;
            IEnumerable<int> toursbysearch = null;

            if (selectedcategories == null)
            {
                GlobalData.selectedcategories = null;
            }
            else
            {
                GlobalData.selectedcategories = selectedcategories;
                var toursbycategids = GlobalData.alltours.Where(t => GlobalData.selectedcategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                toursbysearch = toursbycategids;
            }
            if (GlobalData.selectedattr != null)
            {
                var locationattrs = db.LocationAttraction.Where(l => GlobalData.selectedattr.Contains(l.AttractionTypeId)).ToList();
                var locationbyattrsids = db.LocationAttraction.Where(l => GlobalData.selectedattr.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timebyattrids = alltimetables.Where(t => locationbyattrsids.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourbyattrids = alltimes.Where(t => timebyattrids.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                if (toursbysearch == null)
                    toursbysearch = tourbyattrids;
                else
                {
                    var searchres = toursbysearch.Where(t => tourbyattrids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedislands != null)
            {
                var townsbyislandids = alltowns.Where(t => GlobalData.selectedislands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsbytownids = alllocations.Where(l => townsbyislandids.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timebyislandids = alltimetables.Where(t => locationsbytownids.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursbyislandids = alltimes.Where(t => timebyislandids.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                if (toursbysearch == null)
                    toursbysearch = toursbyislandids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbyislandids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedmonths != null)
            {
                var toursbymonthids = alldates.Where(d => GlobalData.selectedmonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                if (toursbysearch == null)
                    toursbysearch = toursbymonthids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbymonthids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.fromprice != null && GlobalData.toprice != null)
            {
                var toursbypriceids = GlobalData.tours.Where(t => t.AdultPrice <= GlobalData.toprice && t.AdultPrice >= GlobalData.fromprice).ToList().Select(tt => tt.Id).ToArray();
                if (toursbysearch == null)
                    toursbysearch = toursbypriceids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbypriceids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }

            if (toursbysearch != null)
                GlobalData.tours = GlobalData.alltours.Where(t => toursbysearch.Contains(t.Id)).ToList();

            ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, wishlist = GlobalData.wishlist };

            return PartialView("SearchTours", model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterAttr(IEnumerable<int> selectedattr)
        {
            GlobalData.selectedattr = null;
            var alltowns = db.Town.ToList();
            var alllocations = db.Location.ToList();
            var alltimetables = db.TourTimeTable.ToList();
            var alldatetimes = db.TourDateTime.ToList();
            var alltimes = db.TourTime.ToList();
            var alldates = db.TourDate.ToList();
            GlobalData.tours = GlobalData.alltours;
            IEnumerable<int> toursbysearch = null;

            if (selectedattr == null)
            {
                GlobalData.selectedattr = null;
            }
            else
            {
                GlobalData.selectedattr = selectedattr;

                var locationattrs = db.LocationAttraction.Where(l => selectedattr.Contains(l.AttractionTypeId)).ToList();
                var locationbyattrsids = db.LocationAttraction.Where(l => selectedattr.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timebyattrids = alltimetables.Where(t => locationbyattrsids.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourbyattrids = alltimes.Where(t => timebyattrids.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                toursbysearch = tourbyattrids;
            }

            if (GlobalData.selectedislands != null)
            {
                var townsbyislandids = alltowns.Where(t => GlobalData.selectedislands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsbytownids = alllocations.Where(l => townsbyislandids.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timebyislandids = alltimetables.Where(t => locationsbytownids.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursbyislandids = alltimes.Where(t => timebyislandids.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                if (toursbysearch == null)
                    toursbysearch = toursbyislandids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbyislandids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedcategories != null)
            {
                var toursbycategids = GlobalData.alltours.Where(t => GlobalData.selectedcategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                if (toursbysearch == null)
                    toursbysearch = toursbycategids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbycategids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedmonths != null)
            {
                var toursbymonthids = alldates.Where(d => GlobalData.selectedmonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                if (toursbysearch == null)
                    toursbysearch = toursbymonthids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbymonthids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.fromprice != null && GlobalData.toprice != null)
            {
                var toursbypriceids = GlobalData.tours.Where(t => t.AdultPrice <= GlobalData.toprice && t.AdultPrice >= GlobalData.fromprice).ToList().Select(tt => tt.Id).ToArray();
                if (toursbysearch == null)
                    toursbysearch = toursbypriceids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbypriceids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }

            if (toursbysearch != null)
                GlobalData.tours = GlobalData.alltours.Where(t => toursbysearch.Contains(t.Id)).ToList();

            ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, wishlist = GlobalData.wishlist };

            return PartialView("SearchTours", model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterMonths(IEnumerable<int> selectedmonths)
        {
            GlobalData.selectedmonths = null;
            var alltowns = db.Town.ToList();
            var alllocations = db.Location.ToList();
            var alltimetables = db.TourTimeTable.ToList();
            var alldatetimes = db.TourDateTime.ToList();
            var alltimes = db.TourTime.ToList();
            var alldates = db.TourDate.ToList();
            GlobalData.tours = GlobalData.alltours;
            IEnumerable<int> toursbysearch = null;

            if (selectedmonths == null)
            {
                GlobalData.selectedmonths = null;
            }
            else
            {
                GlobalData.selectedmonths = selectedmonths;

                var toursbymonthids = alldates.Where(d => selectedmonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                toursbysearch = toursbymonthids;
            }

            if (GlobalData.selectedislands != null)
            {
                var townsbyislandids = alltowns.Where(t => GlobalData.selectedislands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsbytownids = alllocations.Where(l => townsbyislandids.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timebyislandids = alltimetables.Where(t => locationsbytownids.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursbyislandids = alltimes.Where(t => timebyislandids.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                if (toursbysearch == null)
                    toursbysearch = toursbyislandids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbyislandids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedcategories != null)
            {
                var toursbycategids = GlobalData.alltours.Where(t => GlobalData.selectedcategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                if (toursbysearch == null)
                    toursbysearch = toursbycategids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbycategids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedattr != null)
            {
                var locationattrs = db.LocationAttraction.Where(l => GlobalData.selectedattr.Contains(l.AttractionTypeId)).ToList();
                var locationbyattrsids = db.LocationAttraction.Where(l => GlobalData.selectedattr.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timebyattrids = alltimetables.Where(t => locationbyattrsids.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourbyattrids = alltimes.Where(t => timebyattrids.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                if (toursbysearch == null)
                    toursbysearch = tourbyattrids;
                else
                {
                    var searchres = toursbysearch.Where(t => tourbyattrids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }

            if (GlobalData.fromprice != null && GlobalData.toprice != null)
            {
                var toursbypriceids = GlobalData.tours.Where(t => t.AdultPrice <= GlobalData.toprice && t.AdultPrice >= GlobalData.fromprice).ToList().Select(tt => tt.Id).ToArray();
                if (toursbysearch == null)
                    toursbysearch = toursbypriceids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbypriceids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }

            if (toursbysearch != null)
                GlobalData.tours = GlobalData.alltours.Where(t => toursbysearch.Contains(t.Id)).ToList();

            ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, wishlist = GlobalData.wishlist };

            return PartialView("SearchTours", model);
        }

        [HttpPost]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterPrice(IEnumerable<int> selectedprice)
        {
            GlobalData.fromprice = null;
            GlobalData.toprice = null;
            var alltowns = db.Town.ToList();
            var alllocations = db.Location.ToList();
            var alltimetables = db.TourTimeTable.ToList();
            var alldatetimes = db.TourDateTime.ToList();
            var alltimes = db.TourTime.ToList();
            var alldates = db.TourDate.ToList();
            GlobalData.tours = GlobalData.alltours;
            IEnumerable<int> toursbysearch = null;

            if (selectedprice == null || selectedprice.ElementAt(0) == 4)
            {
                GlobalData.fromprice = null;
                GlobalData.toprice = null;
            }
            else
            {
                GlobalData.fromprice = GlobalData.priceslist.ElementAt(selectedprice.ElementAt(0))[0];
                GlobalData.toprice = GlobalData.priceslist.ElementAt(selectedprice.ElementAt(0))[1];
                //GlobalData.fromprice = selectedprice.ElementAt(0);
                //GlobalData.toprice = selectedprice.ElementAt(1);

                var toursbypriceids = GlobalData.tours.Where(t => t.AdultPrice <= GlobalData.toprice && t.AdultPrice >= GlobalData.fromprice).ToList().Select(tt => tt.Id).ToArray();
                toursbysearch = toursbypriceids;
            }
            if (GlobalData.selectedmonths != null)
            {
                var toursbymonthids = alldates.Where(d => GlobalData.selectedmonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                if (toursbysearch == null)
                    toursbysearch = toursbymonthids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbymonthids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedislands != null)
            {
                var townsbyislandids = alltowns.Where(t => GlobalData.selectedislands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsbytownids = alllocations.Where(l => townsbyislandids.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timebyislandids = alltimetables.Where(t => locationsbytownids.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursbyislandids = alltimes.Where(t => timebyislandids.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                if (toursbysearch == null)
                    toursbysearch = toursbyislandids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbyislandids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedcategories != null)
            {
                var toursbycategids = GlobalData.alltours.Where(t => GlobalData.selectedcategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                if (toursbysearch == null)
                    toursbysearch = toursbycategids;
                else
                {
                    var searchres = toursbysearch.Where(t => toursbycategids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }
            if (GlobalData.selectedattr != null)
            {
                var locationattrs = db.LocationAttraction.Where(l => GlobalData.selectedattr.Contains(l.AttractionTypeId)).ToList();
                var locationbyattrsids = db.LocationAttraction.Where(l => GlobalData.selectedattr.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timebyattrids = alltimetables.Where(t => locationbyattrsids.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourbyattrids = alltimes.Where(t => timebyattrids.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                if (toursbysearch == null)
                    toursbysearch = tourbyattrids;
                else
                {
                    var searchres = toursbysearch.Where(t => tourbyattrids.Contains(t)).ToArray();
                    toursbysearch = searchres;
                }
            }

            if (toursbysearch != null)
                GlobalData.tours = GlobalData.alltours.Where(t => toursbysearch.Contains(t.Id)).ToList();

            ViewModelLookUp model = new ViewModelLookUp { categories = GlobalData.categories, attractionTypes = GlobalData.attractionTypes, tours = GlobalData.tours, islands = GlobalData.islands, wishlist = GlobalData.wishlist };

            return PartialView("SearchTours", model);
        }
    }

    public static class GlobalData
    {
        // read-write variable
        public static List<decimal[]> priceslist
        {
            get
            {
                return HttpContext.Current.Application["priceslist"] as List<decimal[]>;
            }
            set
            {
                HttpContext.Current.Application["priceslist"] = value;
            }

        }

        public static List<WishList> wishlist
        {
            get
            {
                return HttpContext.Current.Application["wishlist"] as List<WishList>;
            }
            set
            {
                HttpContext.Current.Application["wishlist"] = value;
            }

        }

        public static List<ViewModelTour> alltours
        {
            get
            {
                return HttpContext.Current.Application["alltours"] as List<ViewModelTour>;
            }
            set
            {
                HttpContext.Current.Application["alltours"] = value;
            }
        }
        public static List<ViewModelTour> tours
        {
            get
            {
                return HttpContext.Current.Application["tours"] as List<ViewModelTour>;
            }
            set
            {
                HttpContext.Current.Application["tours"] = value;
            }
        }

        public static List<Category> categories
        {
            get
            {
                return HttpContext.Current.Application["categories"] as List<Category>;
            }
            set
            {
                HttpContext.Current.Application["categories"] = value;
            }
        }

        public static List<AttractionType> attractionTypes
        {
            get
            {
                return HttpContext.Current.Application["attractionTypes"] as List<AttractionType>;
            }
            set
            {
                HttpContext.Current.Application["attractionTypes"] = value;
            }
        }

        public static List<Island> islands
        {
            get
            {
                return HttpContext.Current.Application["islands"] as List<Island>;
            }
            set
            {
                HttpContext.Current.Application["islands"] = value;
            }
        }

        public static IEnumerable<int> selectedcategories
        {
            get
            {
                return HttpContext.Current.Application["selectedcategories"] as IEnumerable<int>;
            }
            set
            {
                HttpContext.Current.Application["selectedcategories"] = value;
            }
        }

        public static IEnumerable<int> selectedattr
        {
            get
            {
                return HttpContext.Current.Application["selectedattr"] as IEnumerable<int>;
            }
            set
            {
                HttpContext.Current.Application["selectedattr"] = value;
            }
        }

        public static IEnumerable<int> selectedislands
        {
            get
            {
                return HttpContext.Current.Application["selectedislands"] as IEnumerable<int>;
            }
            set
            {
                HttpContext.Current.Application["selectedislands"] = value;
            }
        }

        public static IEnumerable<int> selectedmonths
        {
            get
            {
                return HttpContext.Current.Application["selectemonths"] as IEnumerable<int>;
            }
            set
            {
                HttpContext.Current.Application["selectemonths"] = value;
            }
        }

        public static decimal? fromprice
        {
            get
            {
                return HttpContext.Current.Application["fromprice"] as decimal?;
            }
            set
            {
                HttpContext.Current.Application["fromprice"] = value;
            }
        }

        public static decimal? toprice
        {
            get
            {
                return HttpContext.Current.Application["toprice"] as decimal?;
            }
            set
            {
                HttpContext.Current.Application["toprice"] = value;
            }
        }
        public static string searchtext
        {
            get
            {
                return HttpContext.Current.Application["searchtext"] as string;
            }
            set
            {
                HttpContext.Current.Application["searchtext"] = value;
            }
        }
    }
}