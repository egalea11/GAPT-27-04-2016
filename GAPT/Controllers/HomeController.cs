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
                    List<ViewModelTour> tourByName = (List<ViewModelTour>)Session["SearchTours"];
                    List<ViewModelTour> SortByName = tourByName.OrderBy(t => t.Name).ToList();
                    Session["SearchTours"] = SortByName;
                    break;
                case "popular":
                    List<ViewModelTour> tourByPopular = (List<ViewModelTour>)Session["SearchTours"];
                    List<ViewModelTour> SortByPopular = tourByPopular.OrderBy(t => t.AverageRatingId).ToList();
                    Session["SearchTours"] = SortByPopular;
                    break;
                case "pricelow":
                    List<ViewModelTour> tourByPriceLow = (List<ViewModelTour>)Session["SearchTours"];
                    List<ViewModelTour> SortByPriceLow = tourByPriceLow.OrderBy(t => t.AdultPrice).ToList();
                    Session["SearchTours"] = SortByPriceLow;
                    break;
                case "pricehigh":
                    List<ViewModelTour> tourByPriceHigh = (List<ViewModelTour>)Session["SearchTours"];
                    List<ViewModelTour> SortByPriceHigh = tourByPriceHigh.OrderByDescending(t => t.AdultPrice).ToList();
                    Session["SearchTours"] = SortByPriceHigh;
                    break;
            }

            ViewModelSearch model = new ViewModelSearch()
            {
                Tours = (List<ViewModelTour>)Session["SearchTours"],
                Wishlists = (List<WishList>)Session["Wishlists"]
            };

            return PartialView("SearchTours", model);
        }

        public ActionResult Index()
        {
            Session["SelectedIslands"] = null;
            Session["SelectedAttractions"] = null;
            Session["SelectedCategories"] = null;
            Session["FromPrice"] = null;
            Session["ToPrice"] = null;
            Session["SelectedMonths"] = null;
            Session["SearchText"] = null;
            Session["TourId"] = null;

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
                Session["Wishlists"] = wishlists.Where(w => w.UserId == userid).ToList();
                return Json(true);
            }
            return Json(false);
        }
        public ActionResult AddToWishlist(IEnumerable<int> id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // check is already done on client side but should be added here as well
                //Response.StatusCode = 401;
                //Response.End();
                return Json(false);
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

                Session["Wishlists"] = wishlists.Where(w => w.UserId == userid).ToList();
                return Json(true);
            }
            return Json(false);         
        }

        public async Task<ActionResult> OrderConfirmation(CustomerInfoModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // check is already done on client side but should be added here as well
                Response.StatusCode = 401;
                Response.End();
            }

            int? tempOrderId = (int?)Session["TempOrderId"];

            if (tempOrderId != null && tempOrderId != 0)
            {
                db.Database.ExecuteSqlCommand("update [TempOrder] set [Expired] = @p1 where [Id] = @p2",
                    new System.Data.SqlClient.SqlParameter("p1", 1),
                    new System.Data.SqlClient.SqlParameter("p2", tempOrderId));
            }

            CustomerInfoModel orderModel = (CustomerInfoModel)Session["PaymentModel"];
            var currUserName = User.Identity.Name;
            var user = appdb.Users.Where(u => u.UserName == currUserName).FirstOrDefault();

            Order order = new Order()
            {
                AdultQuantity = orderModel.AdultAmount,
                ChildQuantity = orderModel.ChildAmount,
                DateTimeCreated = DateTime.Now,
                TotalPrice = orderModel.TotalPrice,
                UserId = user.Id,
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
            if (orderModel.ChildAmount != 0)
            {
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
            }

            var tourDateTime = db.TourDateTime.Where(t => t.Id == order.TourDateTimeId).FirstOrDefault();
            var tourDate = db.TourDate.Where(d => d.Id == tourDateTime.TourDateId).FirstOrDefault().DateOfTour.ToShortDateString();
            var tourTime = db.TourTime.Where(t => t.Id == tourDateTime.TourTimeId).FirstOrDefault();
            var stringTourTime = tourTime.StartTime + "-" + tourTime.EndTime;
            var tour = db.Tour.Where(t => t.Id == tourTime.TourId).FirstOrDefault();
            var startingLocationId = db.TourTimeTable.Where(t => t.TourTimeId == tourTime.Id && t.StartTime == tourTime.StartTime).FirstOrDefault().LocationId;
            var startingLocation = db.Location.Where(l => l.Id == startingLocationId).FirstOrDefault();

            // user.Email
            // user.Name
            // user.Surname
            // order.AdultQuantity
            // order.ChildQuantity
            // order.TotalPrice
            // tourDate    ----> date of tour as string
            // stringTourTime    ----> time of tour as string
            // tour.Name
            // startingLocation.Name    ----> just in case trid tikteb starting location


            // --------- Send Email confirmation ----------
            string body;
            using (var sr = new StreamReader(Server.MapPath("\\App_Data\\") + "emailTemplate.txt"))
            {
                body = sr.ReadToEnd();
            }

            var message = new MailMessage();
            message.To.Add(new MailAddress(user.Email));
            message.From = new MailAddress("toursmaltin@gmail.com");
            message.Subject = "Order Receipt";
            message.Body = string.Format(body, tour.Name, tourDate, stringTourTime, user.Name, user.Surname, order.AdultQuantity, order.ChildQuantity, order.TotalPrice, order.Id, startingLocation.Name);
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
            }

            Session["PaymentModel"] = null;
            Session["Tourpage"] = null;
            return View(orderModel);
        }

        [HttpPost]
        public void PayPalPayment(CustomerInfoModel model)
        {
            int? tempOrderId = (int?)Session["TempOrderId"];

            if (tempOrderId != null && tempOrderId != 0)
            {
                db.Database.ExecuteSqlCommand("update [TempOrder] set [Expired] = @p1 where [Id] = @p2",
                    new System.Data.SqlClient.SqlParameter("p1", 1),
                    new System.Data.SqlClient.SqlParameter("p2", tempOrderId));
            }

            Session["TempOrderId"] = null;

            CustomerInfoModel orderModel = (CustomerInfoModel)Session["PaymentModel"];
            var currUserName = User.Identity.Name;
            var user = appdb.Users.Where(u => u.UserName == currUserName).FirstOrDefault();

            TempOrder tempOrder = new TempOrder()
            {
                AdultQuantity = orderModel.AdultAmount,
                ChildQuantity = orderModel.ChildAmount,
                DateTimeCreated = DateTime.Now,
                TotalPrice = orderModel.TotalPrice,
                UserId = user.Id,
                TourDateTimeId = orderModel.TourDateTimeId
            };

            db.TempOrder.Add(tempOrder);
            db.SaveChanges();
            //var tempOrderId = tempOrder.Id;
            Session["TempOrderId"] = tempOrder.Id;
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Payment()
        {
            CustomerInfoModel paymentModel = (CustomerInfoModel)Session["PaymentModel"];

            int? tempOrderId = (int?)Session["TempOrderId"];

            if (tempOrderId != null && tempOrderId != 0)
            {
                db.Database.ExecuteSqlCommand("update [TempOrder] set [Expired] = @p1 where [Id] = @p2",
                    new System.Data.SqlClient.SqlParameter("p1", 1),
                    new System.Data.SqlClient.SqlParameter("p2", tempOrderId));
            }

            Session["TempOrderId"] = null;

            return View(paymentModel);
        }

        [HttpPost]
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

            int? tempOrderId = (int?)Session["TempOrderId"];

            if (tempOrderId != null && tempOrderId != 0)
            {
                db.Database.ExecuteSqlCommand("update [TempOrder] set [Expired] = @p1 where [Id] = @p2",
                    new System.Data.SqlClient.SqlParameter("p1", 1),
                    new System.Data.SqlClient.SqlParameter("p2", tempOrderId));
            }

            Session["TempOrderId"] = null;
            return RedirectToAction("Payment");
            //return View(paymentModel);
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

            Session["PaymentModel"] = null;

            //CustomerInfoModel orderModel = (CustomerInfoModel)Session["PaymentModel"];
            //if (orderModel != null)
            //    return View(orderModel);

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
            var tempOrders = db.TempOrder.Where(to => tourDateTimeIds.Contains(to.TourDateTimeId) && to.Expired == false).ToList();

            int totalAdultAmount = 0;
            int totalChildAmount = 0;
            foreach (var order in orders)
            {
                totalAdultAmount = totalAdultAmount + order.AdultQuantity;
                totalChildAmount = totalChildAmount + order.ChildQuantity;
            }

            int totalTempAdultAmount = 0;
            int totalTempChildAmount = 0;
            foreach (var tempOrder in tempOrders)
            {
                totalTempAdultAmount = totalTempAdultAmount + tempOrder.AdultQuantity;
                totalTempChildAmount = totalTempChildAmount + tempOrder.ChildQuantity;
            }

            int totalGroupSize = totalAdultAmount + totalChildAmount + totalTempAdultAmount + totalTempChildAmount;
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
            var tempOrders = db.TempOrder.Where(to => tourDateTimeIds.Contains(to.TourDateTimeId) && to.Expired == false).ToList();

            int totalAdultAmount = 0;
            int totalChildAmount = 0;
            foreach (var order in orders)
            {
                totalAdultAmount = totalAdultAmount + order.AdultQuantity;
                totalChildAmount = totalChildAmount + order.ChildQuantity;
            }

            int totalTempAdultAmount = 0;
            int totalTempChildAmount = 0;
            foreach (var tempOrder in tempOrders)
            {
                totalTempAdultAmount = totalTempAdultAmount + tempOrder.AdultQuantity;
                totalTempChildAmount = totalTempChildAmount + tempOrder.ChildQuantity;
            }

            int totalGroupSize = totalAdultAmount + totalChildAmount + totalTempAdultAmount + totalTempChildAmount;
            //int totalGroupSize = totalAdultAmount + totalChildAmount;
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

            // if there is rating, calculates average
            float averageRating;
            if (sumRating > 0)
                averageRating = sumRating / ratings.Count();
            else
                averageRating = ratings.Count();
                
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
            Session["SelectedIslands"] = null;
            Session["SelectedAttractions"] = null;
            Session["SelectedCategories"] = null;
            Session["FromPrice"] = null;
            Session["ToPrice"] = null;
            Session["SelectedMonths"] = null;

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
        public ActionResult Tourpage(int id)
        {
            Session["SelectedIslands"] = null;
            Session["SelectedAttractions"] = null;
            Session["SelectedCategories"] = null;
            Session["FromPrice"] = null;
            Session["ToPrice"] = null;
            Session["SelectedMonths"] = null;

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

        [AllowAnonymous]
        public ActionResult SearchTours()
        {
            ViewModelSearch model = new ViewModelSearch()
            { 
                Tours = (List<ViewModelTour>)Session["SearchTours"],
                Wishlists = (List<WishList>)Session["Wishlists"]
            };

            return PartialView(model);
        }

        [AllowAnonymous]
        public ActionResult SearchIslands()
        {
            var AllIslands = db.Island.ToList();
            ViewModelIsland model = new ViewModelIsland();
            model.Islands = AllIslands;
            //model.Islands = new List<Island>();
            //model.Islands = (List<Island>)Session["AllIslands"];
            return PartialView(model);
        }

        [AllowAnonymous]
        public ActionResult SearchAttractionTypes()
        {
            ViewModelAttractionType model = new ViewModelAttractionType();
            model.AttractionTypes = new List<AttractionType>();
            //model.AttractionTypes = (List<AttractionType>)Session["AllAttractionTypes"];
            model.AttractionTypes = db.AttractionType.ToList();
            return PartialView(model);
        }

        [AllowAnonymous]
        public ActionResult SearchCategories()
        {
            //ViewModelCategory model = new ViewModelCategory() 
            //{
            //    Categories = (List<Category>)Session["AllCategories"],
            //    selectedcategory = (IEnumerable<int>)Session["SelectedCategories"]
            //};
            ViewModelCategory model = new ViewModelCategory()
            {
                Categories = db.Category.ToList(),
                selectedcategory = (IEnumerable<int>)Session["SelectedCategories"]
            };
            //model.Categories = new List<Category>();
            //model.Categories = (List<Category>)Session["AllCategories"];
            return PartialView(model);
        }

        [AllowAnonymous]
        [HttpGet]
        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Search()
        {
            try
            {
                #region Load all tours

                var AllTours = db.Tour.ToList();
                var AllTourTimes = db.TourTime.ToList();
                var ThumbImages = db.Image.Where(m => m.Link.Contains("rsz")).ToList();
                var AllTimeTables = db.TourTimeTable.ToList();
                var AllTourDateTimes = db.TourDateTime.ToList();

                Session["AllTourTimes"] = AllTourTimes;
                Session["ThumbImages"] = ThumbImages;
                Session["AllTimeTables"] = AllTimeTables;
                Session["AllTourDateTimes"] = AllTourDateTimes;

                if (User.Identity.IsAuthenticated)
                {
                    var currentUserName = User.Identity.Name;
                    var userId = appdb.Users.Where(u => u.UserName == currentUserName).FirstOrDefault().Id;
                    var currUserWishlist = db.WishList.Where(w => w.UserId == userId && w.Expired == false).ToList();
                    Session["Wishlists"] = currUserWishlist;
                }
                else
                    Session["Wishlists"] = null;

                foreach (Tour t in AllTours)
                {
                    ViewModelTour tour = new ViewModelTour()
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
                        ThumbImage = ThumbImages.FirstOrDefault(ti => ti.TourId == t.Id).Link
                    };

                    string st = AllTourTimes.FirstOrDefault(tt => tt.TourId == t.Id).StartTime;
                    string et = AllTourTimes.FirstOrDefault(tt => tt.TourId == t.Id).EndTime;

                    DateTime dt1 = DateTime.ParseExact(st, "HH:mm", new DateTimeFormatInfo());
                    DateTime dt2 = DateTime.ParseExact(et, "HH:mm", new DateTimeFormatInfo());
                    TimeSpan ts = dt2.Subtract(dt1);

                    tour.Duration = ts.Hours.ToString() + "hrs";
                    tour.Duration = ts.Minutes == 0 ? tour.Duration : tour.Duration + " " + ts.Minutes.ToString() + "mins";
                    ViewAllTours.Add(tour);
                }
                Session["SearchTours"] = ViewAllTours;
                Session["AllTours"] = ViewAllTours;

                List<decimal[]> priceList = new List<decimal[]>();

                decimal[] price1 = new decimal[2];
                decimal[] price2 = new decimal[2];
                decimal[] price3 = new decimal[2];

                price1[0] = 0;
                price1[1] = 10;
                price2[0] = 10;
                price2[1] = 20;
                price3[0] = 20;
                price3[1] = 30;

                priceList.Add(price1);
                priceList.Add(price2);
                priceList.Add(price3);

                Session["PriceList"] = priceList;

                #endregion

                IEnumerable<int> selectedCategories = (IEnumerable<int>)Session["SelectedCategories"];

                if (selectedCategories != null)
                    Session["SearchTours"] = ViewAllTours.Where(t => selectedCategories.Contains(t.CategoryId)).ToList();
                else
                    Session["SearchTours"] = ViewAllTours;

                return View();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message.ToString());
            } 
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Search(string searchText)
        {
            try
            {
                #region Load all tours

                var AllTours = db.Tour.ToList();
                var AllCategories = db.Category.ToList();
                var AllAttractionTypes = db.AttractionType.ToList();
                var AllTourTimes = db.TourTime.ToList();
                var ThumbImages = db.Image.Where(m => m.Link.Contains("rsz")).ToList();
                var AllIslands = db.Island.ToList();
                var AllLocations = db.Location.ToList();
                var AllLocationAttractions = db.LocationAttraction.ToList();
                var AllTowns = db.Town.ToList();
                var AllTimeTables = db.TourTimeTable.ToList();
                var AllTourDateTimes = db.TourDateTime.ToList();

                List<int> searchTextTourIds = new List<int>();

                if (User.Identity.IsAuthenticated)
                {
                    var currentUserName = User.Identity.Name;
                    var userId = appdb.Users.Where(u => u.UserName == currentUserName).FirstOrDefault().Id;
                    var currUserWishlist = db.WishList.Where(w => w.UserId == userId && w.Expired == false).ToList();
                    Session["Wishlists"] = currUserWishlist;
                }
                else
                    Session["Wishlists"] = null;

                foreach (Tour t in AllTours)
                {
                    ViewModelTour tour = new ViewModelTour()
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
                        ThumbImage = ThumbImages.FirstOrDefault(ti => ti.TourId == t.Id).Link
                    };

                    string st = AllTourTimes.FirstOrDefault(tt => tt.TourId == t.Id).StartTime;
                    string et = AllTourTimes.FirstOrDefault(tt => tt.TourId == t.Id).EndTime;

                    DateTime dt1 = DateTime.ParseExact(st, "HH:mm", new DateTimeFormatInfo());
                    DateTime dt2 = DateTime.ParseExact(et, "HH:mm", new DateTimeFormatInfo());
                    TimeSpan ts = dt2.Subtract(dt1);

                    tour.Duration = ts.Hours.ToString() + "hrs";
                    tour.Duration = ts.Minutes == 0 ? tour.Duration : tour.Duration + " " + ts.Minutes.ToString() + "mins";
                    ViewAllTours.Add(tour);
                }

                Session["AllTours"] = ViewAllTours;

                List<decimal[]> priceList = new List<decimal[]>();

                decimal[] price1 = new decimal[2];
                decimal[] price2 = new decimal[2];
                decimal[] price3 = new decimal[2];

                price1[0] = 0;
                price1[1] = 10;
                price2[0] = 10;
                price2[1] = 20;
                price3[0] = 20;
                price3[1] = 30;

                priceList.Add(price1);
                priceList.Add(price2);
                priceList.Add(price3);

                Session["PriceList"] = priceList;

                #endregion

                IEnumerable<int> selectedCategories = (IEnumerable<int>)Session["SelectedCategories"];

                if (searchText != null && searchText.Trim() != "")
                {
                    var tourByFullName = db.Tour.Where(t => t.Name.ToLower() == searchText.ToLower()).FirstOrDefault();
                    if (tourByFullName != null)
                        Session["SearchTours"] = ViewAllTours.Where(t => t.Id == tourByFullName.Id).ToList();
                    else
                    {
                        string[] words = searchText.ToLower().Trim().Split(' ');
                        foreach (string word in words)
                        {
                            var categoryIds = AllCategories.Where(t => t.Name.ToLower().Contains(word)).ToList().Select(c => c.Id).ToArray();
                            var islandIds = AllIslands.Where(i => i.Name.ToLower().Contains(word)).ToList().Select(t => t.Id).ToArray();
                            var attractionIds = AllAttractionTypes.Where(a => a.Name.ToLower().Contains(word)).ToList().Select(aa => aa.Id).ToArray();
                            var locAttrIds = AllLocationAttractions.Where(la => attractionIds.Contains(la.AttractionTypeId)).ToList().Select(l => l.LocationId).ToArray();
                            var townIds = AllTowns.Where(t => islandIds.Contains(t.IslandId) || t.Name.ToLower().Contains(word)).ToList().Select(tt => tt.Id).ToArray();
                            var locationIds = AllLocations.Where(l => locAttrIds.Contains(l.Id) || townIds.Contains(l.TownId)).ToList().Select(ll => ll.Id).ToArray();

                            var timeiIds = AllTimeTables.Where(t => locationIds.Contains(t.LocationId)).ToList().Select(tt => tt.TourTimeId).ToArray();
                            var timeTourIds = AllTourTimes.Where(t => timeiIds.Contains(t.TourId)).ToList().Select(tt => tt.TourId).ToArray();

                            var searchTourIds = AllTours.Where(t => t.Name.ToLower().Contains(word) || t.ShortDescription.ToLower().Contains(word) || t.LongDescription.ToLower().Contains(word) || categoryIds.Contains(t.CategoryId) || timeTourIds.Contains(t.Id)).ToList().Select(tt => tt.Id).ToArray();

                            foreach (var id in searchTourIds)
                            {
                                if (!searchTextTourIds.Contains(id))
                                    searchTextTourIds.Add(id);
                            }
                        }
                        if (selectedCategories != null)
                        {
                            var searchTourByCategIds = ViewAllTours.Where(t => selectedCategories.Contains(t.CategoryId)).ToList().Select(t => t.Id).ToArray();
                            var searchResultIds = searchTourByCategIds.Where(t => searchTextTourIds.Contains(t)).ToArray();
                            Session["SearchTours"] = ViewAllTours.Where(t => searchResultIds.Contains(t.Id)).ToList();
                        }
                        else
                            Session["SearchTours"] = ViewAllTours.Where(t => searchTextTourIds.Contains(t.Id)).ToList();
                    }
                }
                else
                    Session["SearchTours"] = ViewAllTours;

                return View();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message.ToString());
            }

            return View();
        }

        public PartialViewResult HomeCategDropDown()
        {
            ViewModelCategory model = new ViewModelCategory();
            model.Categories = db.Category.ToList();
            return PartialView(model);
        }

        [HttpPost]
        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchCategory(IEnumerable<int> id)
        {
            Session["SelectedCategories"] = id;
            return RedirectToAction("Search");
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
        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterIslands(IEnumerable<int> selectedislands)
        {
            var allTowns = db.Town.ToList();
            var allLocations = db.Location.ToList();
            var allTimeTables = db.TourTimeTable.ToList();
            var allDateTimes = db.TourDateTime.ToList();
            var allTimes = db.TourTime.ToList();
            var allDates = db.TourDate.ToList();
            List<ViewModelTour> allTours = (List<ViewModelTour>)Session["AllTours"];

            Session["SearchTours"] = allTours;
            IEnumerable<int> toursBySearch = null;
            IEnumerable<int> selectedAttractions = (IEnumerable<int>)Session["SelectedAttractions"];
            IEnumerable<int> selectedMonths = (IEnumerable<int>)Session["SelectedMonths"];
            IEnumerable<int> selectedCategories = (IEnumerable<int>)Session["SelectedCategories"];
            decimal? fromPrice = (decimal?)Session["FromPrice"];
            decimal? toPrice = (decimal?)Session["ToPrice"];

            if (selectedislands == null)
                Session["SelectedIslands"] = null;
            else
            {
                Session["SelectedIslands"] = selectedislands;
                var townsByIslandIds = allTowns.Where(t => selectedislands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsByTownIds = allLocations.Where(l => townsByIslandIds.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timeByIslandIds = allTimeTables.Where(t => locationsByTownIds.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursByIslandIds = allTimes.Where(t => timeByIslandIds.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                toursBySearch = toursByIslandIds;
            }
            if (selectedCategories != null)
            {
                var toursByCategIds = allTours.Where(t => selectedCategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                if (toursBySearch == null)
                    toursBySearch = toursByCategIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByCategIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedAttractions != null)
            {
                var locationAttrs = db.LocationAttraction.Where(l => selectedAttractions.Contains(l.AttractionTypeId)).ToList();
                var locationByAttrsIds = db.LocationAttraction.Where(l => selectedAttractions.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timeByAttrIds = allTimeTables.Where(t => locationByAttrsIds.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourByAttrIds = allTimes.Where(t => timeByAttrIds.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                if (toursBySearch == null)
                    toursBySearch = tourByAttrIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => tourByAttrIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }

            if (selectedMonths != null)
            {
                var toursByMonthIds = allDates.Where(d => selectedMonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                if (toursBySearch == null)
                    toursBySearch = toursByMonthIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByMonthIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (fromPrice != null && toPrice != null)
            {
                var toursByPriceIds = AllTours.Where(t => t.AdultPrice <= toPrice && t.AdultPrice >= fromPrice).ToList().Select(tt => tt.Id).ToArray();
                if (toursBySearch == null)
                    toursBySearch = toursByPriceIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByPriceIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }

            if (toursBySearch != null)
                Session["SearchTours"] = allTours.Where(t => toursBySearch.Contains(t.Id)).ToList();

            ViewModelSearch model = new ViewModelSearch
            {
                Tours = (List<ViewModelTour>)Session["SearchTours"],
                Wishlists = (List<WishList>)Session["Wishlists"]
            };

            return PartialView("SearchTours", model);
        }

        [HttpPost]
        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterCategories(IEnumerable<int> selectedcategories)
        {
            var allTowns = db.Town.ToList();
            var allLocations = db.Location.ToList();
            var allTimeTables = db.TourTimeTable.ToList();
            var allDateTimes = db.TourDateTime.ToList();
            var allTimes = db.TourTime.ToList();
            var allDates = db.TourDate.ToList();
            List<ViewModelTour> allTours= (List<ViewModelTour>)Session["AllTours"];

            Session["SearchTours"] = allTours;
            IEnumerable<int> toursBySearch = null;
            IEnumerable<int> selectedAttractions = (IEnumerable<int>)Session["SelectedAttractions"];
            IEnumerable<int> selectedMonths = (IEnumerable<int>)Session["SelectedMonths"];
            IEnumerable<int> selectedIslands = (IEnumerable<int>)Session["SelectedIslands"];
            decimal? fromPrice = (decimal?)Session["FromPrice"];
            decimal? toPrice = (decimal?)Session["ToPrice"];

            if (selectedcategories == null)
                Session["SelectedCategories"] = null;
            else
            {
                Session["SelectedCategories"] = selectedcategories;
                var toursByCategIds = allTours.Where(t => selectedcategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                toursBySearch = toursByCategIds;
            }
            if (selectedAttractions != null)
            {
                var locationAttrs = db.LocationAttraction.Where(l => selectedAttractions.Contains(l.AttractionTypeId)).ToList();
                var locationByAttrsIds = db.LocationAttraction.Where(l => selectedAttractions.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timeByAttrIds = allTimeTables.Where(t => locationByAttrsIds.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourByAttrIds = allTimes.Where(t => timeByAttrIds.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                if (toursBySearch == null)
                    toursBySearch = tourByAttrIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => tourByAttrIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedIslands != null)
            {
                var townsByIslandIds = allTowns.Where(t => selectedIslands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsByTownIds = allLocations.Where(l => townsByIslandIds.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timeByIslandIds = allTimeTables.Where(t => locationsByTownIds.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursByIslandIds = allTimes.Where(t => timeByIslandIds.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                if (toursBySearch == null)
                    toursBySearch = toursByIslandIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByIslandIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedMonths != null)
            {
                var toursByMonthIds = allDates.Where(d => selectedMonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                if (toursBySearch == null)
                    toursBySearch = toursByMonthIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByMonthIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (fromPrice != null && toPrice != null)
            {
                var toursByPriceIds = allTours.Where(t => t.AdultPrice <= toPrice && t.AdultPrice >= fromPrice).ToList().Select(tt => tt.Id).ToArray();
                if (toursBySearch == null)
                    toursBySearch = toursByPriceIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByPriceIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }

            if (toursBySearch != null)
                Session["SearchTours"] = allTours.Where(t => toursBySearch.Contains(t.Id)).ToList();

            ViewModelSearch model = new ViewModelSearch 
            { 
                Tours = (List<ViewModelTour>)Session["SearchTours"], 
                Wishlists = (List<WishList>)Session["Wishlists"] 
            };

            return PartialView("SearchTours", model);
        }

        [HttpPost]
        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterAttr(IEnumerable<int> selectedattr)
        {
            var allTowns = db.Town.ToList();
            var allLocations = db.Location.ToList();
            var allTimeTables = db.TourTimeTable.ToList();
            var allDateTimes = db.TourDateTime.ToList();
            var allTimes = db.TourTime.ToList();
            var allDates = db.TourDate.ToList();
            List<ViewModelTour> allTours = (List<ViewModelTour>)Session["AllTours"];

            Session["SearchTours"] = allTours;
            IEnumerable<int> toursBySearch = null;
            IEnumerable<int> selectedIslands = (IEnumerable<int>)Session["SelectedIslands"];
            IEnumerable<int> selectedMonths = (IEnumerable<int>)Session["SelectedMonths"];
            IEnumerable<int> selectedCategories = (IEnumerable<int>)Session["SelectedCategories"];
            decimal? fromPrice = (decimal?)Session["FromPrice"];
            decimal? toPrice = (decimal?)Session["ToPrice"];

            if (selectedattr == null)
                Session["SelectedAttractions"] = null;
            else
            {
                Session["SelectedAttractions"] = selectedattr;

                var locationAttrs = db.LocationAttraction.Where(l => selectedattr.Contains(l.AttractionTypeId)).ToList();
                var locationByAttrsIds = db.LocationAttraction.Where(l => selectedattr.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timeByAttrIds = allTimeTables.Where(t => locationByAttrsIds.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourByAttrIds = allTimes.Where(t => timeByAttrIds.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                toursBySearch = tourByAttrIds;
            }

            if (selectedIslands != null)
            {
                var townsByIslandIds = allTowns.Where(t => selectedIslands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsByTownIds = allLocations.Where(l => townsByIslandIds.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timeByIslandIds = allTimeTables.Where(t => locationsByTownIds.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursByIslandIds = allTimes.Where(t => timeByIslandIds.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                if (toursBySearch == null)
                    toursBySearch = toursByIslandIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByIslandIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedCategories != null)
            {
                var toursByCategIds = allTours.Where(t => selectedCategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                if (toursBySearch == null)
                    toursBySearch = toursByCategIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByCategIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedMonths != null)
            {
                var toursByMonthIds = allDates.Where(d => selectedMonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                if (toursBySearch == null)
                    toursBySearch = toursByMonthIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByMonthIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (fromPrice != null && toPrice != null)
            {
                var toursByPriceIds = allTours.Where(t => t.AdultPrice <= toPrice && t.AdultPrice >= fromPrice).ToList().Select(tt => tt.Id).ToArray();
                if (toursBySearch == null)
                    toursBySearch = toursByPriceIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByPriceIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }

            if (toursBySearch != null)
                Session["SearchTours"] = allTours.Where(t => toursBySearch.Contains(t.Id)).ToList();

            ViewModelSearch model = new ViewModelSearch
            {
                Tours = (List<ViewModelTour>)Session["SearchTours"],
                Wishlists = (List<WishList>)Session["Wishlists"]
            };

            return PartialView("SearchTours", model);
        }

        [HttpPost]
        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterMonths(IEnumerable<int> selectedmonths)
        {
            var allTowns = db.Town.ToList();
            var allLocations = db.Location.ToList();
            var allTimeTables = db.TourTimeTable.ToList();
            var allDateTimes = db.TourDateTime.ToList();
            var allTimes = db.TourTime.ToList();
            var allDates = db.TourDate.ToList();
            List<ViewModelTour> allTours = (List<ViewModelTour>)Session["AllTours"];

            Session["SearchTours"] = allTours;
            IEnumerable<int> toursBySearch = null;
            IEnumerable<int> selectedIslands = (IEnumerable<int>)Session["SelectedIslands"];
            IEnumerable<int> selectedAttractions = (IEnumerable<int>)Session["SelectedAttractions"];
            IEnumerable<int> selectedCategories = (IEnumerable<int>)Session["SelectedCategories"];
            decimal? fromPrice = (decimal?)Session["FromPrice"];
            decimal? toPrice = (decimal?)Session["ToPrice"];

            if (selectedmonths == null)
                Session["SelectedMonths"] = null;
            else
            {
                Session["SelectedMonths"] = selectedmonths;

                var toursByMonthIds = allDates.Where(d => selectedmonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                toursBySearch = toursByMonthIds;
            }

            if (selectedIslands != null)
            {
                var townsByIslandIds = allTowns.Where(t => selectedIslands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsByTownIds = allLocations.Where(l => townsByIslandIds.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timeByIslandIds = allTimeTables.Where(t => locationsByTownIds.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursByIslandIds = allTimes.Where(t => timeByIslandIds.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                if (toursBySearch == null)
                    toursBySearch = toursByIslandIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByIslandIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedCategories != null)
            {
                var toursByCategIds = allTours.Where(t => selectedCategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                if (toursBySearch == null)
                    toursBySearch = toursByCategIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByCategIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedAttractions != null)
            {
                var locationAttrs = db.LocationAttraction.Where(l => selectedAttractions.Contains(l.AttractionTypeId)).ToList();
                var locationByAttrsIds = db.LocationAttraction.Where(l => selectedAttractions.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timeByAttrIds = allTimeTables.Where(t => locationByAttrsIds.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourByAttrIds = allTimes.Where(t => timeByAttrIds.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                if (toursBySearch == null)
                    toursBySearch = tourByAttrIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => tourByAttrIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }

            if (fromPrice != null && toPrice != null)
            {
                var toursByPriceIds = allTours.Where(t => t.AdultPrice <= toPrice && t.AdultPrice >= fromPrice).ToList().Select(tt => tt.Id).ToArray();
                if (toursBySearch == null)
                    toursBySearch = toursByPriceIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByPriceIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }

            if (toursBySearch != null)
                Session["SearchTours"] = allTours.Where(t => toursBySearch.Contains(t.Id)).ToList();

            ViewModelSearch model = new ViewModelSearch
            {
                Tours = (List<ViewModelTour>)Session["SearchTours"],
                Wishlists = (List<WishList>)Session["Wishlists"]
            };

            return PartialView("SearchTours", model);
        }

        [HttpPost]
        //[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult SearchFilterPrice(IEnumerable<int> selectedprice)
        {
            var allTowns = db.Town.ToList();
            var allLocations = db.Location.ToList();
            var allTimeTables = db.TourTimeTable.ToList();
            var allDateTimes = db.TourDateTime.ToList();
            var allTimes = db.TourTime.ToList();
            var allDates = db.TourDate.ToList();
            List<ViewModelTour> allTours = (List<ViewModelTour>)Session["AllTours"];

            Session["SearchTours"] = allTours;
            IEnumerable<int> toursBySearch = null;
            IEnumerable<int> selectedIslands = (IEnumerable<int>)Session["SelectedIslands"];
            IEnumerable<int> selectedAttractions = (IEnumerable<int>)Session["SelectedAttractions"];
            IEnumerable<int> selectedCategories = (IEnumerable<int>)Session["SelectedCategories"];
            IEnumerable<int> selectedMonths = (IEnumerable<int>)Session["SelectedMonths"];

            if (selectedprice == null || selectedprice.ElementAt(0) == 4)
            {
                Session["FromPrice"] = null;
                Session["ToPrice"] = null;
            }
            else
            {
                List<decimal[]> Price1 = (List<decimal[]>)Session["PriceList"];
                decimal? fromPrice = Price1.ElementAt(selectedprice.ElementAt(0))[0];
                List<decimal[]> Price2 = (List<decimal[]>)Session["PriceList"];
                decimal? toPrice = Price1.ElementAt(selectedprice.ElementAt(0))[1];

                Session["FromPrice"] = fromPrice;
                Session["ToPrice"] = toPrice;

                var toursByPriceIds = allTours.Where(t => t.AdultPrice <= toPrice && t.AdultPrice >= fromPrice).ToList().Select(tt => tt.Id).ToArray();
                toursBySearch = toursByPriceIds;
            }
            if (selectedMonths != null)
            {
                var toursByMonthIds = allDates.Where(d => selectedMonths.Contains(d.DateOfTour.Date.Month)).ToList().Select(t => t.TourId).ToArray();
                if (toursBySearch == null)
                    toursBySearch = toursByMonthIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByMonthIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedIslands != null)
            {
                var townsByIslandIds = allTowns.Where(t => selectedIslands.Contains(t.IslandId)).ToList().Select(ti => ti.Id).ToArray();
                var locationsByTownIds = allLocations.Where(l => townsByIslandIds.Contains(l.TownId)).ToList().Select(loc => loc.Id).ToArray();
                var timeByIslandIds = allTimeTables.Where(t => locationsByTownIds.Contains(t.LocationId)).ToList().Select(t => t.TourTimeId).ToArray();
                var toursByIslandIds = allTimes.Where(t => timeByIslandIds.Contains(t.Id)).ToList().Select(td => td.TourId).ToArray();

                if (toursBySearch == null)
                    toursBySearch = toursByIslandIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByIslandIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedCategories != null)
            {
                var toursByCategIds = allTours.Where(t => selectedCategories.Contains(t.CategoryId)).ToList().Select(tt => tt.Id).ToArray();

                if (toursBySearch == null)
                    toursBySearch = toursByCategIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => toursByCategIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }
            if (selectedAttractions != null)
            {
                var locationAttrs = db.LocationAttraction.Where(l => selectedAttractions.Contains(l.AttractionTypeId)).ToList();
                var locationByAttrsIds = db.LocationAttraction.Where(l => selectedAttractions.Contains(l.AttractionTypeId)).ToList().Select(loc => loc.LocationId).ToArray();
                var timeByAttrIds = allTimeTables.Where(t => locationByAttrsIds.Contains(t.LocationId)).ToList().Select(dt => dt.TourTimeId).ToArray();
                var tourByAttrIds = allTimes.Where(t => timeByAttrIds.Contains(t.Id)).ToList().Select(ta => ta.TourId).ToArray();

                if (toursBySearch == null)
                    toursBySearch = tourByAttrIds;
                else
                {
                    var searchResult = toursBySearch.Where(t => tourByAttrIds.Contains(t)).ToArray();
                    toursBySearch = searchResult;
                }
            }

            if (toursBySearch != null)
                Session["SearchTours"] = allTours.Where(t => toursBySearch.Contains(t.Id)).ToList();

            ViewModelSearch model = new ViewModelSearch
            {
                Tours = (List<ViewModelTour>)Session["SearchTours"],
                Wishlists = (List<WishList>)Session["Wishlists"]
            };

            return PartialView("SearchTours", model);
        }
    }
}