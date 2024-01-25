using System.ComponentModel.DataAnnotations;

namespace IngressBkpAutomation.Models
{
    public class SysSetup
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? OrgName { get; set; }
        [StringLength(50)]
        public string? SiteName { get; set; }
        [StringLength(50)]
        public string? SmtpUserName { get; set; }
        [StringLength(100)]
        public string? SmtpPassword { get; set; }
        [StringLength(50)]
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        [StringLength(20)]
        public string? SocketOption { get; set; }
        [StringLength(50)]
        public string? ReceiverEmail { get; set; }
        [StringLength(50)]
        public string? MysqlUserName { get; set; }
        [StringLength(100)]
        public string? MysqlPassword { get; set; }
        [StringLength(50)]
        public string? MysqlServer { get; set; }
        [StringLength(20)]
        public string? SiteIngressDb { get; set; }
        [StringLength(50)]
        public string? HoMysqlUserName { get; set; }
        [StringLength(100)]
        public string? HoMysqlPassword { get; set; }
        [StringLength(50)]
        public string? HoMysqlServer { get; set; }
        [StringLength(20)]
        public string? HoIngressDb { get; set; }
        public int IngressBackMonths { get; set; }
        [StringLength(50)]
        public string? Contact { get; set; }
        [StringLength(100)]
        public string? LastBackup { get; set; }
        [StringLength(250)]
        public string? BackupLoc { get; set; }
        public bool Closed { get; set; }
        public bool OnMpls { get; set; }
        [StringLength(50)]
        public string? Personnel { get; set; }
        public TimeSpan? AutoBackup1At { get; set; }
        public TimeSpan? AutoBackup2At { get; set; }
    }
}