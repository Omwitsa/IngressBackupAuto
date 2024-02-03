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
using System.Diagnostics;

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
using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var jobManager = services.GetRequiredService<IRecurringJobManager>();
    var cronJob = services.GetRequiredService<ICronJobProvider>();
    var context = services.GetRequiredService<IngressSetupDbContext>();
    var setup = context.SysSetup.FirstOrDefault();

    var backup1Time = setup.AutoBackup1At.GetValueOrDefault();
    var backup2Time = setup.AutoBackup2At.GetValueOrDefault();

    jobManager.AddOrUpdate("AttendanceBackup1", () => cronJob.BackupAttendance(), Cron.Daily(backup1Time.Hours, backup1Time.Minutes), TimeZoneInfo.Local);
    jobManager.AddOrUpdate("AttendanceBackup2", () => cronJob.BackupAttendance(), Cron.Daily(backup2Time.Hours, backup2Time.Minutes), TimeZoneInfo.Local);
}
app.Run();
