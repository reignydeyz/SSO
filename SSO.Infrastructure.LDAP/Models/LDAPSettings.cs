namespace SSO.Infrastructure.LDAP.Models
{
    public class LDAPSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SearchBase { get; set; }
        public string SearchFilter { get; set; }
        public bool UseSSL { get; set; }
    }
}
