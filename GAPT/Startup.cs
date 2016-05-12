using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;


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
            //RecurringJob.AddOrUpdate(() => smsReminder.Reminder(), "*/10 * * * *"); //runs every minute (for testing purposes)



        }

    }
}

