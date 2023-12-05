namespace IngressBkpAutomation.Models
{
    public class SysSetup
    {
        public int Id { get; set; }
        public string? OrgName { get; set; }
        public string? SiteName { get; set; }
        public string? SmtpUserName { get; set; }
        public string? SmtpPassword { get; set; }
        public string? SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string? SocketOption { get; set; }
        public string? MysqlUserName { get; set; }
        public string? MysqlPassword { get; set; }
        public string? MysqlServer { get; set; }
        public string? SiteIngressDb { get; set; }
        public string? HoIngressDb { get; set; }
        public int IngressBackMonths { get; set; }
        public string? Contact { get; set; }
        public bool Closed { get; set; }
        public string? Personnel { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}