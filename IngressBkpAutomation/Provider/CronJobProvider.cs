using AspNetCoreHero.ToastNotification.Abstractions;
using IngressBkpAutomation.IProvider;
using IngressBkpAutomation.Models;
using IngressBkpAutomation.Utilities;
using IngressBkpAutomation.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IngressBkpAutomation.Provider
{
    public class CronJobProvider : ICronJobProvider
    {
        private readonly IngressSetupDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailProvider _emailProvider;
        private Utility _utility = new Utility();
        public CronJobProvider(IngressSetupDbContext context, IWebHostEnvironment env, IEmailProvider emailProvider)
        {
            _env = env;
            _context = context;
            _emailProvider = emailProvider;
        }

        public async Task BackupAttendance()
        {
            var setup = _context.SysSetup.FirstOrDefault();
            setup.BackupLoc = string.IsNullOrEmpty(setup.BackupLoc) ? Path.Combine(_env.WebRootPath, "MysqlBackup") : setup.BackupLoc;
            string newPath = Path.Combine(setup.BackupLoc, "Backup");
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);
            string filePath = Path.Combine(newPath, "backup.sql");

            var backupResp = await _utility.BackupIngress(filePath, setup);
            if (backupResp.Success)
            {
                var zipResp = await _utility.ZipBackup(_context, _env);
                if (zipResp.Success)
                    await EmailBackup();
            }
        }

        private async Task<ReturnData<bool>> EmailBackup()
        {
            var setting = _context.SysSetup.FirstOrDefault();
            setting.BackupLoc = string.IsNullOrEmpty(setting.BackupLoc) ? Path.Combine(_env.WebRootPath, "MysqlBackup") : setting.BackupLoc;
            //var varificationLink = $"{Request.Host}/account/confirmEmail?username={userDetails.Data.UserId}";
            //var logoImageUrl = Path.Combine(_env.WebRootPath, setting.LogoImageUrl);
            var sender = new EmailAddress
            {
                Name = setting.SiteName,
                Address = setting.SmtpUserName
            };

            var receiver1 = new EmailAddress
            {
                Name = "System Genereated Attendance",
                Address = setting.ReceiverEmail
            };

            var emailMessage = new EmailMessage
            {
                FromAddresses = new List<EmailAddress> { sender },
                ToAddresses = new List<EmailAddress> { receiver1 },
                Subject = $"{setting.SiteName} System Genereated Attendance",
                //InstitutionLogo = logoImageUrl,
                Attachments = new List<string>(),
                Body = _utility.GenerateMailBody(sender, MailOparation.Notify),
            };

            var filePath = Path.Combine(setting.BackupLoc, setting.LastBackup);
            emailMessage.Attachments.Add(filePath);
            var smtpSettings = new MailSettings
            {
                EmailId = setting.SmtpUserName,
                DisplayName = setting.SiteName,
                Password = setting.SmtpPassword,
                Server = setting.SmtpServer,
                Port = setting.SmtpPort,
                SocketOption = setting.SocketOption
            };
            return await _emailProvider.SendEmailAsync(emailMessage, smtpSettings);
        }

    }
}
