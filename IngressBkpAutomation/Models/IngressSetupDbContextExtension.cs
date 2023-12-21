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
                    SmtpUserName = "ingress.bkpauto@aaagrowers.co.ke",
                    SmtpPassword = Decryptor.Encrypt("Ib@$#@9les!"),
                    SmtpPort = 587, // 587, 465
                    SocketOption = "TLS", // TLS, SSL, NONE
                    ReceiverEmail = "ingressbkpauto@aaagrowers.co.ke",
                    MysqlUserName = "root",
                    MysqlPassword = Decryptor.Encrypt("Ingress2023"),
                    MysqlServer = "localhost",
                    SiteIngressDb = "ingress",
                    HoMysqlUserName = "root",
                    HoMysqlPassword = Decryptor.Encrypt("Ingress2023"),
                    HoMysqlServer = "localhost",
                    HoIngressDb = "ingress_simba",
                    IngressBackMonths = 2,
                    Closed = false,
                    OnMpls = true
                });
            }

            context.SaveChanges();
        }
    }
}
