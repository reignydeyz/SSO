using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IGroupRoleRepository
    {
        Task Add(GroupUser param, bool? saveChanges = true, object? args = null);

        Task Delete(GroupUser param, bool? saveChanges = true, object? args = null);
    }
}
