namespace SSO.Business.Applications
{
    public class ApplicationDetailDto : ApplicationDto
    {
        public int TokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public short MaxAccessFailedCount { get; set; } = 0;
    }
}
