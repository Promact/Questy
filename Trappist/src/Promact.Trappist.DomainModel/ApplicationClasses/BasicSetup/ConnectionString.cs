namespace Promact.Trappist.DomainModel.ApplicationClasses.BasicSetup
{
    public class ConnectionString
    {
        public string Value { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string IsTrustedConnection { get; set; }
    }
}