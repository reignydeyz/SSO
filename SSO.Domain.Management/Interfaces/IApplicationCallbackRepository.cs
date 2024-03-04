using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IApplicationCallbackRepository : IRepository<ApplicationCallback>, IRangeRepository<ApplicationCallback>
    {
    }
}
