using IngressBkpAutomation.Models;
using IngressBkpAutomation.ViewModel;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.IO.Compression;

namespace IngressBkpAutomation.Utilities
{
    public class Utility
    {
        public static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<IngressSetupDbContext>();
                context.Database.Migrate();
                context.EnsureDatabaseSeeded();
                // context.Database.EnsureCreated();
            }
        }

        public async Task<ReturnData<string>> BackupIngress(string filePath, SysSetup setup)
        {
            try
            {
                string constring = $"server={setup.MysqlServer};user={setup.MysqlUserName};pwd={Decryptor.Decrypt(setup.MysqlPassword)};database={setup.SiteIngressDb};";
                var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var backupMonthStartDate = month.AddMonths(-setup.IngressBackMonths);
                var dic = new Dictionary<string, string>();
                dic["attendance"] = $"SELECT * FROM `attendance` WHERE date >= (STR_TO_DATE('{backupMonthStartDate}', '%d/%m/%Y'));";
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
                            command.CommandTimeout = 0;
                            connection.Open();

                            List<string> lstHeaders = new List<string>();
                            lstHeaders.Add("USE `" + setup.HoIngressDb + "`;");
                            backup.ExportInfo.SetDocumentHeaders(lstHeaders);
                            backup.ExportInfo.TablesToBeExportedDic = dic;
                            backup.ExportInfo.EnableComment = true;
                            backup.ExportInfo.ExportFunctions = false;
                            backup.ExportInfo.ExportViews = false;
                            backup.ExportInfo.ExportTriggers = false;
                            backup.ExportInfo.ExportEvents = false;
                            backup.ExportInfo.ExportProcedures = false;
                            backup.ExportInfo.ExportRoutinesWithoutDefiner = false;
                            backup.ExportInfo.ResetAutoIncrement = true;
                            backup.ExportInfo.MaxSqlLength = 1024 * 1024;

                            List<string> lstFooters = new List<string>();
                            lstFooters.Add("");
                            backup.ExportInfo.SetDocumentFooters(lstFooters);

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

        public async Task<ReturnData<string>> ZipBackup(IngressSetupDbContext context, IWebHostEnvironment env)
        {
            try
            {
                var setup = context.SysSetup.FirstOrDefault();
                setup.BackupLoc = string.IsNullOrEmpty(setup.BackupLoc) ? Path.Combine(env.WebRootPath, "MysqlBackup") : setup.BackupLoc;
                string sourcePath = Path.Combine(setup.BackupLoc, "Backup");
                var date = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}";
                var backupName = $"backup.sql__{date}__.zip";
                string destinationPath = Path.Combine(setup.BackupLoc, backupName);
                if (!Directory.Exists(sourcePath))
                    Directory.CreateDirectory(sourcePath);

                FileInfo file = new FileInfo(destinationPath);
                if (file.Exists)
                    file.Delete();

                ZipFile.CreateFromDirectory(
                    sourcePath,
                    destinationPath
                );

                setup.LastBackup = backupName;
                context.SaveChanges();
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

        public string GenerateMailBody(EmailAddress sender, MailOparation mailOparation)
        {
            var message = "";
            if (mailOparation == MailOparation.Notify)
                message = "<div style='margin: 2em 5em 2em 5em; background-color: #f2f2f2'>" +
                           "<table style='width: 100 %; margin: 5% 10% 5% 10%;'><br>" +
                                "<tr><td> This is a system generated notification on " + sender.Name + " attendance data update as at (" + DateTime.UtcNow.AddHours(3) + ") <br> <br></td></tr>" +
                           " </table>" +
                       "</div>";

            return message;
        }
    }
}
