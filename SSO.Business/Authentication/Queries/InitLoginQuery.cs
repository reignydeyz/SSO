namespace SSO.Business.Authentication.Queries
{
    public class InitLoginQuery : AuthDto
    {
        public string? CallbackUrl { get; set; }
    }
}
