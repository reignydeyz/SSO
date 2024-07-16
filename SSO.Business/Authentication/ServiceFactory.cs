using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SSO.Business.Authentication
{
    public class ServiceFactory
    {
        readonly IHttpContextAccessor _httpContext;
        readonly IServiceProvider _serviceProvider;
        readonly IConfiguration _config;

        public ServiceFactory(IHttpContextAccessor httpContext, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _httpContext = httpContext;
            _serviceProvider = serviceProvider;
            _config = configuration;
        }
    }
}
