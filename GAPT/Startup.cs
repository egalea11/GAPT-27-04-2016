using GAPT.Models;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(GAPT.Startup))]
namespace GAPT
{
    public partial class Startup
    {

        SmsReminder smsReminder = new SmsReminder();
        SmsService sms = new SmsService();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage("ConnectionString");
            JobStorage.Current = new SqlServerStorage("ConnectionString");
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //RecurringJob.AddOrUpdate(() => smsReminder.Reminder(), Cron.Daily);  runs once per day
            //RecurringJob.AddOrUpdate(() => smsReminder.Reminder(), "*/1 * * * *"); //runs every minute (for testing purposes)

            

        }
        /*
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
           // var UserManager = new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User   
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool  
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                 

                var user = new ApplicationUser();
                user.UserName = "admingmail@gmail.com";
                user.Email = "admingemail@gmail.com";

                string userPWD = "Iamadmin95!";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin  
                if (chkUser.Succeeded)
                {
                    Console.WriteLine("SUCCEEDED");
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }

        }

    */


    }
}
