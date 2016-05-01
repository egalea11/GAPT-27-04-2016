using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GAPT.Models;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Web.ApplicationServices;

namespace GAPT.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ToursDbContext db = new ToursDbContext();
        private ApplicationDbContext appdb = new ApplicationDbContext();
        List<ViewModelTour> ViewAllTours = new List<ViewModelTour>();
        public List<Tour> AllTours = new List<Tour>();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(User.Identity.GetUserId<int>()),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(User.Identity.GetUserId<int>()),
                Logins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<int>()),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<int>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<int>(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId<int>(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        //[HttpPost]
        public PartialViewResult HomeCategDropDown()
        {
            ViewModelCategory model = new ViewModelCategory();
            model.categories = db.Category.ToList();
            return PartialView(model);
        }

        public ActionResult MyAccount()
        {
            GetCategAttrTours();
            var error = TempData["ErrorMessage"] as string;
            if (error != null)
            {
                ChangePasswordViewModel model = new ChangePasswordViewModel() { ErrorMessage = "The Old Password entered is incorrect" };
                ViewData["WrongPassword"] = model;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("MyAccount", new { Message = ManageMessageId.ChangePasswordSuccess });
                //return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            TempData["ErrorMessage"] = "The Old Password entered is incorrect";
            return RedirectToAction("MyAccount", new { Message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<int>());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId<int>(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
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

                if (User.Identity.IsAuthenticated)
                {
                    var currentusername = User.Identity.Name;
                    var userid = appdb.Users.Where(u => u.UserName == currentusername).FirstOrDefault().Id;
                    var curruserwishlist = db.WishList.Where(w => w.UserId == userid && w.Expired == false).ToList();
                    GlobalTours.wishlist = curruserwishlist;
                }
                else
                {
                    GlobalTours.wishlist = null;
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
                GlobalTours.alltours = ViewAllTours;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message.ToString());
            }
            //return null;
        }

        public ActionResult MyWishlist()
        {
            var wishlisttourids = GlobalTours.wishlist.Select(w => w.TourId).ToArray();
            var tours = GlobalTours.alltours.Where(t => wishlisttourids.Contains(t.Id)).ToList();
            ViewModelWishlist wishlist = new ViewModelWishlist() { wishlisttours = tours };
            return PartialView(wishlist);
        }
        public ActionResult MyOrders()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // User not authenticated
                // Response 401 Unauthenticated
                Response.StatusCode = 401;
                Response.End();
            }
            var currUserName = User.Identity.Name;
            var userId = appdb.Users.Where(u => u.UserName == currUserName).FirstOrDefault().Id;
            var orders = db.Order.Where(o => o.UserId == userId).ToList();

            OrderHistoryModel model = new OrderHistoryModel();
            model.Orders = new List<OrderDetails>();

            foreach (var o in orders)
            {
                var tourDateTime = db.TourDateTime.Where(t => t.Id == o.TourDateTimeId).FirstOrDefault();
                var tourDateId = tourDateTime.TourDateId;
                var tourTimeId = tourDateTime.TourTimeId;
                var tourDate = db.TourDate.Where(t => t.Id == tourDateId).FirstOrDefault();
                var tourTime = db.TourTime.Where(t => t.Id == tourTimeId).FirstOrDefault();
                var tourLocationId = db.TourTimeTable.Where(t => t.TourTimeId == tourTime.Id && t.StartTime == tourTime.StartTime).FirstOrDefault().LocationId;
                var tourLocationName = db.Location.Where(l => l.Id == tourLocationId).FirstOrDefault().Name;
                var tour = db.Tour.Where(t => t.Id == tourTime.TourId).FirstOrDefault();

                OrderDetails order = new OrderDetails() 
                {
                    AdultQuantity = o.AdultQuantity,
                    ChildQuantity = o.ChildQuantity,
                    TotalPrice = o.TotalPrice,
                    DateTimeCreated = o.DateTimeCreated,
                    UserId = o.UserId,
                    TourDateTimeId = o.TourDateTimeId,
                    StartingLocation = tourLocationName,
                    StringTourTime = tourTime.StartTime + "-" + tourTime.EndTime,
                    DateTimeTourDate = tourDate.DateOfTour,
                    TourName = tour.Name,
                    TourId = tour.Id,
                    TotalChildPrice = o.ChildQuantity * tour.ChildPrice,
                    TotalAdultPrice = o.AdultQuantity * tour.AdultPrice,
                };
                model.Orders.Add(order);
            }

            return PartialView(model);
        }

        public ActionResult UserPortal()
        {
            return View();
        }

        public ActionResult ViewCustomerInfo()
        {
            return View();
        }
 
        public ActionResult CustomerInfo()
        {
            return View();
        }

        public ActionResult Wishlist()
        {
            GetCategAttrTours();
            return View();
        }

        public ActionResult ChangePersonalDetails()
        {
            //var user = UserManager.C
            var user = UserManager.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            string[] phone1 = user.PhoneNumber.Split(' ');
            string numberprefix = phone1[0] + ": " + phone1[1];

            ChangePersonalDetailsViewModel model = new ChangePersonalDetailsViewModel() 
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = phone1[2],
                NumberPrefix = numberprefix,
                UserName = user.UserName,
                Country = user.Country,
                BirthDay = user.BirthDate.Day,
                BirthMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(user.BirthDate.Month),
                BirthYear = user.BirthDate.Year
            };
            return PartialView(model);
        }

        public async Task<ActionResult> RemoveWishlist(IEnumerable<int> id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // User not authenticated
                // Response 401 Unauthenticated
                Response.StatusCode = 401;
                Response.End();
            }

            if (id == null || id.Count<int>() == 0)
            {
                // Argument not passed
                // Reponse 400 Bad Request
                Response.StatusCode = 400;
                Response.End();
            }

            var wishlists = db.WishList.ToList();
            var currentusername = User.Identity.Name;
            var userid = appdb.Users.Where(u => u.UserName == currentusername).FirstOrDefault().Id;
            var curruserwishitem = wishlists.Where(w => w.UserId == userid && w.TourId == id.First() && w.Expired == false).ToList();

            if (curruserwishitem.Count != 0)
            {
                db.WishList.Remove(curruserwishitem.FirstOrDefault());
                await db.SaveChangesAsync();
            }

            GlobalTours.wishlist = db.WishList.Where(w => w.UserId == userid && w.Expired == false).ToList();
            var wishlisttourids = GlobalTours.wishlist.Select(w => w.TourId).ToArray();
            var tours = GlobalTours.alltours.Where(t => wishlisttourids.Contains(t.Id)).ToList();
            ViewModelWishlist wishlist = new ViewModelWishlist() { wishlisttours = tours };
            return PartialView("MyWishlist", wishlist);
            //return Json(true);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
    public static class GlobalTours
    {
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
    }
}