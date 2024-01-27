using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Hangfire;
using Hangfire.MySql;
using IngressBkpAutomation.IProvider;
using IngressBkpAutomation.Models;
using IngressBkpAutomation.Provider;
using IngressBkpAutomation.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MySqlDatabase");

//builder.Services.AddDbContext<IngressSetupDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<IngressSetupDbContext>(option => option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddHangfire(x => x.UseStorage(new MySqlStorage(connectionString, new MySqlStorageOptions { TablesPrefix = "Hangfire_" })));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<ICronJobProvider, CronJobProvider>();
builder.Services.AddTransient<IEmailProvider, EmailProvider>();
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopCenter;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);//You can set Time   
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/home/login";
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/home/denied";
            //options.Cookie.Name = ".AspNetCore.Cookies";
            //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHangfireDashboard();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

Utility.UpdateDatabase(app);
//using (var serviceScope = app.Services.CreateScope())
//{
//    var services = serviceScope.ServiceProvider;
//    var recurringJob = services.GetRequiredService<IRecurringJobManager>();
//    var cronJob = services.GetRequiredService<ICronJobProvider>();
//    var context = services.GetRequiredService<IngressSetupDbContext>();
//    var setup = context.SysSetup.FirstOrDefault();

//    int backup1Time = (int)setup.AutoBackup1At.Value.Subtract(TimeSpan.FromHours(3)).TotalHours;
//    int backup2Time = (int)setup.AutoBackup2At.Value.Subtract(TimeSpan.FromHours(3)).TotalHours;
//    //recurringJob.AddOrUpdate("IngressBackup", () => cronJob.AddReccuringJob(), Cron.Minutely);

//    //Cron.Daily();  -  "0 0 * * *"  Every night at 12:00 AM (default UTC time)
//    //recurringJob.AddOrUpdate("IngressBackup", () => cronJob.AddReccuringJob(), Cron.Daily(6, 30));  // "30 9 * * *"
//    recurringJob.AddOrUpdate("AttendanceBackup1", () => cronJob.BackupAttendance(), Cron.Daily(backup1Time, 00));
//    recurringJob.AddOrUpdate("AttendanceBackup2", () => cronJob.BackupAttendance(), Cron.Daily(backup2Time, 00));
//}
app.Run();
