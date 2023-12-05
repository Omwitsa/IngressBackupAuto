﻿using AspNetCoreHero.ToastNotification.Abstractions;
using IngressBkpAutomation.IProvider;
using IngressBkpAutomation.Models;
using IngressBkpAutomation.Utilities;
using IngressBkpAutomation.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.IO.Compression;

namespace IngressBkpAutomation.Controllers
{
    public class BackupController : Controller
    {
        private readonly INotyfService _notyf;
        private readonly IngressSetupDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailProvider _emailProvider;
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
            return View();
        }

        [Authorize]
        public async Task<IActionResult> BackupIngress()
        {
            try
            {
                // Remove duplicates

                string folderName = @"MySQL\Backup";
                string newPath = Path.Combine(_env.WebRootPath, folderName);
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);
                string filePath = Path.Combine(newPath, "backup.sql");

                var backupResp = await BackupIngress(filePath);
                if (!backupResp.Success)
                {
                    _notyf.Error(backupResp.Message);
                    return RedirectToAction("Index");
                }

                var zipResp = await ZipBackup();
                if (!zipResp.Success)
                {
                    _notyf.Error(zipResp.Message);
                    return RedirectToAction("Index");
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

        private async Task<ReturnData<string>> BackupIngress(string filePath)
        {
            try
            {
                var setup = _context.SysSetup.FirstOrDefault();
                string constring = $"server={setup.MysqlServer};user={setup.MysqlUserName};pwd={Decryptor.Decrypt(setup.MysqlPassword)};database={setup.SiteIngressDb};";
                var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var backupMonthStartDate = month.AddMonths(-setup.IngressBackMonths);
                var dic = new Dictionary<string, string>();
                dic["attendance"] = $"SELECT * FROM `attendance` WHERE DATE_FORMAT(date,'%Y/%m/%d') >= '{backupMonthStartDate}';";
                dic["leavetype"] = "SELECT * FROM `leavetype`;";
                dic["schedule"] = "SELECT * FROM `schedule`;";
                dic["user"] = "SELECT * FROM `user`;";
                dic["user_group"] = "SELECT * FROM `user_group`;";
                dic["user_info"] = "SELECT * FROM `user_info`;";

                using (MySqlConnection connection = new MySqlConnection(constring))
                {
                    using (MySqlCommand command = new MySqlCommand())
                    {
                        using (MySqlBackup backup = new MySqlBackup(command))
                        {
                            command.Connection = connection;
                            connection.Open();

                            List<string> lstHeaders = new List<string>();
                            lstHeaders.Add("USE `" + setup.HoIngressDb + "`;");
                            backup.ExportInfo.SetDocumentHeaders(lstHeaders);
                            backup.ExportInfo.TablesToBeExportedDic = dic;
                            backup.ExportInfo.EnableComment = false;
                            backup.ExportInfo.ExportFunctions = false;
                            backup.ExportInfo.ExportViews = false;
                            backup.ExportInfo.ExportTriggers = false;
                            backup.ExportInfo.ExportEvents = false;
                            backup.ExportInfo.ExportProcedures = false;
                            backup.ExportInfo.ExportRoutinesWithoutDefiner = false;

                            backup.ExportToFile(filePath);
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
                var emailResp = await SendEmail();
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

        private async Task<ReturnData<bool>> SendEmail()
        {
            var setting = _context.SysSetup.FirstOrDefault();
            //var varificationLink = $"{Request.Host}/account/confirmEmail?username={userDetails.Data.UserId}";
            //var logoImageUrl = Path.Combine(_env.WebRootPath, setting.LogoImageUrl);
            var institutionEmail = new EmailAddress
            {
                Name = setting.SiteName,
                Address = setting.SmtpUserName
            };

            var receiverEmail = new EmailAddress
            {
                Name = "Ingress Backup",
                Address = "wilson.omwitsa@aaagrowers.co.ke"
            };

            var emailMessage = new EmailMessage
            {
                FromAddresses = new List<EmailAddress> { institutionEmail },
                ToAddresses = new List<EmailAddress> { receiverEmail },
                Subject = $"{setting.SiteName} Ingress Backup",
                //InstitutionLogo = logoImageUrl,
                Attachments = new List<string>(),
                Body = GenerateMailBody(institutionEmail),
            };

            var filePath = Path.Combine(_env.WebRootPath, "MySQL", setting.LastBackup);
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

        private async Task<ReturnData<string>> ZipBackup()
        {
            try
            {
                string sourcePath = Path.Combine(_env.WebRootPath, "MySQL", "Backup");
                var backupName = $"backup.sql{DateTime.Today}.zip";
                string destinationPath = Path.Combine(_env.WebRootPath, "MySQL", backupName);
                if (!Directory.Exists(sourcePath))
                    Directory.CreateDirectory(sourcePath);

                FileInfo file = new FileInfo(destinationPath);
                if (file.Exists)
                    file.Delete();

                ZipFile.CreateFromDirectory(
                    sourcePath,
                    destinationPath
                );

                var setup = _context.SysSetup.FirstOrDefault();
                setup.LastBackup = backupName;
                _context.SaveChanges();
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

        private string GenerateMailBody(EmailAddress institutionEmail)
        {
            var message = "<div style='margin: 2em 5em 2em 5em; background-color: #f2f2f2'>" +
                            "<table style='width: 100 %; margin: 5% 10% 5% 10%;'><br>" +
                                "<tr><td> " + institutionEmail.Name + " Backup(" + DateTime.UtcNow.AddHours(3) + ") <br> <br></td></tr>" +
                            " </table>" +
                        "</div>";

            return "";
        }

    }
}
