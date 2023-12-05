using IngressBkpAutomation.Constants;
using IngressBkpAutomation.Utilities;
using MySql.Data.MySqlClient;

namespace IngressBkpAutomation.Models
{
    public static class IngressSetupDbContextExtension
    {
        public static void EnsureDatabaseSeeded(this IngressSetupDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Add(new User
                {
                    UserID = "admin@aaa.com",
                    Names = "Administrator",
                    Password = Decryptor.Encrypt("123.123"),
                    Email = "itsupport@aaagrowers.co.ke",
                    DateCreated = DateTime.UtcNow,
                    Role = StrValues.AdminRole,
                    Status = true,
                });
            }

            if (!context.SysSetup.Any())
            {
                context.Add(new SysSetup
                {
                    OrgName = "AAA GROWERS",
                    SiteName = "HO",
                    SmtpServer = "mail.aaagrowers.co.ke",
                    SmtpUserName = "wilson.omwitsa@aaagrowers.co.ke",
                    SmtpPassword = Decryptor.Encrypt("W0mw!@9les!"),
                    SmtpPort = 587, // 587, 465
                    SocketOption = "TLS", // sslonconnect, none
                    MysqlUserName = "root",
                    MysqlPassword = Decryptor.Encrypt("Aaa@2023"),
                    MysqlServer = "localhost",
                    SiteIngressDb = "ingress",
                    HoIngressDb = "ingress_simba",
                    IngressBackMonths = 2,
                    DateCreated = DateTime.UtcNow,
                    Closed = false,
                });
            }

            context.SaveChanges();
        }
    }
}
