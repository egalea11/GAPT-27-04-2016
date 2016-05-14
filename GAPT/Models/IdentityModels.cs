using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GAPT.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole,
    CustomUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public DateTime BirthDate { get; set; }
        //public int BirthDay { get; set; }
        //public string BirthMonth { get; set; }
        //public int BirthYear { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public string Country { get; set; }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }



    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole,
    int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("ConnectionString") //DefaultConnection , throwIfV1Schema: false
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ToursDbContext: DbContext
    {
        public ToursDbContext()
            : base("ConnectionString") //DefaultConnection , throwIfV1Schema: false
        {
        }

        public static ToursDbContext Create()
        {
            return new ToursDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Tour> Tour { get; set; }
        public DbSet<TourAttendees> TourAttendees { get; set; }
        public DbSet<TourDate> TourDate { get; set; }
        public DbSet<TourDateTime> TourDateTime { get; set; }
        public DbSet<TourTime> TourTime { get; set; }
        public DbSet<TourTimeTable> TourTimeTable { get; set; }
        public DbSet<Town> Town { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<TempOrder> TempOrder { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<WishList> WishList { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<LocationAttraction> LocationAttraction { get; set; }
        public DbSet<Island> Island { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<AttractionType> AttractionType { get; set; }
    }
}