using IngressBkpAutomation.ViewModel;

namespace IngressBkpAutomation.Constants
{
    public class StrValues
    {
        static public string UserId = "UserId";
        static public string AdminRole = "Admin";
        static public string UserRole = "User";
        static public string TLS = "TLS";
        static public string SSL = "SSL";
        static public string NONE = "NONE";
    }

    public class ArrValues
    {
        public static string[] Roles = new string[] { StrValues.UserRole, StrValues.AdminRole };
        public static string[] SocketOptions = new string[] { StrValues.TLS, StrValues.SSL, StrValues.NONE };
    }
}
