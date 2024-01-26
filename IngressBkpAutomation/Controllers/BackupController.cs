using AspNetCoreHero.ToastNotification.Abstractions;
using IngressBkpAutomation.IProvider;
using IngressBkpAutomation.Models;
using IngressBkpAutomation.Utilities;
using IngressBkpAutomation.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.IO.Compression;
using System.Security.Claims;

namespace IngressBkpAutomation.Controllers
{
    public class BackupController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IngressSetupDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailProvider _emailProvider;
        private Utility _utility = new Utility();
        public BackupController(IngressSetupDbContext context, INotyfService notyf, IWebHostEnvironment env, IEmailProvider emailProvider)
        {
            _env = env;
            _notyf = notyf;
            _context = context;
            _emailProvider = emailProvider;
        }

        [Authorize]
        public IActionResult Index()
        {
            var setup = _context.SysSetup.FirstOrDefault();
            ViewBag.btnLable = setup.OnMpls ? "Backup and Update" : "Backup";
            ViewBag.onMpls = setup.OnMpls;
            return View();
        }

        [Authorize]
        public async Task<IActionResult> BackupIngress()
        {
            try
            {
                // Remove duplicates
                var setup = _context.SysSetup.FirstOrDefault();
                setup.BackupLoc = string.IsNullOrEmpty(setup.BackupLoc) ? Path.Combine(_env.WebRootPath, "MysqlBackup") : setup.BackupLoc;
                string newPath = Path.Combine(setup.BackupLoc, "Backup");
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);
                string filePath = Path.Combine(newPath, "backup.sql");

                var backupResp = await _utility.BackupIngress(filePath, setup);
                if (!backupResp.Success)
                {
                    _notyf.Error(backupResp.Message);
                    return RedirectToAction("Index");
                }

                var zipResp = await _utility.ZipBackup(_context, _env);
                if (!zipResp.Success)
                {
                    _notyf.Error(zipResp.Message);
                    return RedirectToAction("Index");
                }

                if (setup.OnMpls)
                {
                    var importResp = await ImportBackup(filePath, setup);
                    if (!importResp.Success)
                    {
                        _notyf.Error(importResp.Message);
                        return RedirectToAction("Index");
                    }

                    var emailResp = await NotifyBackupUpdate();
                    if (!emailResp.Success)
                    {
                        _notyf.Error(emailResp.Message);
                        return RedirectToAction("Index");
                    }
                }
                
                _notyf.Success("Ingress Backed up successfully");
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                _notyf.Error("Sorry, An error occurred");
                return RedirectToAction("Index");
            }
        }

        private async Task<ReturnData<string>> ImportBackup(string filePath, SysSetup setup)
        {
            try
            {
                string constring = $"server={setup.HoMysqlServer};user={setup.HoMysqlUserName};pwd={Decryptor.Decrypt(setup.HoMysqlPassword)};database={setup.HoIngressDb};";
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ImportFromFile(filePath);
                            conn.Close();
                        }
                    }
                }

                return new ReturnData<string>
                {
                    Success = true,
                };
            }
            catch (Exception)
            {
                return new ReturnData<string>
                {
                    Success = false,
                    Message = "Sorry, An error occurred"
                };
            }
        }


        [Authorize]
        public async Task<IActionResult> SendIngressbackup()
        {
            try
            {
                var emailResp = await EmailBackup();
                if (!emailResp.Success)
                {
                    _notyf.Error(emailResp.Message);
                    return RedirectToAction("Index");
                }

                _notyf.Success("Backup send successfully");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _notyf.Error("Sorry, An error occurred");
                return RedirectToAction("Index");
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
                Name = "Attendance Data",
                Address = setting.ReceiverEmail
            };

            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var receiver2 = new EmailAddress
            {
                Name = "Attendance Data",
                Address = userEmail
            };

            var emailMessage = new EmailMessage
            {
                FromAddresses = new List<EmailAddress> { sender },
                ToAddresses = new List<EmailAddress> { receiver1, receiver2 },
                Subject = $"{setting.SiteName} Attendance Data",
                //InstitutionLogo = logoImageUrl,
                Attachments = new List<string>(),
                Body = _utility.GenerateMailBody(sender, MailOparation.Backup),
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

        private async Task<ReturnData<bool>> NotifyBackupUpdate()
        {
            var setting = _context.SysSetup.FirstOrDefault();
            var sender = new EmailAddress
            {
                Name = setting.SiteName,
                Address = setting.SmtpUserName
            };

            var receiver1 = new EmailAddress
            {
                Name = "Attendance Data",
                Address = setting.ReceiverEmail
            };

            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var receiver2 = new EmailAddress
            {
                Name = "Attendance Data",
                Address = userEmail
            };

            var emailMessage = new EmailMessage
            {
                FromAddresses = new List<EmailAddress> { sender },
                ToAddresses = new List<EmailAddress> { receiver1, receiver2 },
                Subject = $"{setting.SiteName} Attendance Data",
                //InstitutionLogo = logoImageUrl,
                Attachments = new List<string>(),
                Body = _utility.GenerateMailBody(sender, MailOparation.Notify),
            };

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
