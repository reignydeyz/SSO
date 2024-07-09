using Microsoft.Extensions.Options;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Management;
using System.DirectoryServices;

namespace SSO.Infrastructure.LDAP
{
    public class GroupUserRepository : GroupUserRepositoryBase, IDisposable
    {
        readonly IAppDbContext _context;
        readonly DirectoryEntry _dirEntry;
        readonly DirectorySearcher _dirSearcher;

        bool _disposed;

        public GroupUserRepository(IAppDbContext context, IOptions<LDAPSettings> ldapSettings) : base(context)
        {
            _context = context;

            var ldapConnectionString = $"{(ldapSettings.Value.UseSSL ? "LDAPS" : "LDAP")}://{ldapSettings.Value.Server}:{ldapSettings.Value.Port}/{ldapSettings.Value.SearchBase}";
            _dirEntry = new DirectoryEntry(ldapConnectionString, ldapSettings.Value.Username, ldapSettings.Value.Password);
            _dirSearcher = new DirectorySearcher(_dirEntry);
        }

        public override async Task<GroupUser> Add(GroupUser param, bool? saveChanges = true, object? args = null)
        {
            var group = _context.Groups.First(x => x.GroupId == param.GroupId).Name;
            var userName = _context.Users.First(x => x.Id == param.UserId);

            _dirSearcher.Filter = $"(&(objectClass=user)(sAMAccountName={userName}))";

            var userResult = _dirSearcher.FindOne();

            if (userResult != null)
            {
                var userEntry = userResult.GetDirectoryEntry();

                _dirSearcher.Filter = $"(&(objectClass=group)(sAMAccountName={group}))";
                var groupResult = _dirSearcher.FindOne();

                if (groupResult != null) 
                {
                    var groupEntry = groupResult.GetDirectoryEntry();
                    groupEntry.Properties["member"].Add(userEntry.Properties["distinguishedName"].Value);
                    groupEntry.CommitChanges(); 
                }
            }

            await _context.AddAsync(param);
            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
            return param;
        }

        public override async Task Delete(GroupUser param, bool? saveChanges = true)
        {
            var group = _context.Groups.First(x => x.GroupId == param.GroupId).Name;
            var userName = _context.Users.First(x => x.Id == param.UserId);

            _dirSearcher.Filter = $"(&(objectClass=user)(sAMAccountName={userName}))";

            var userResult = _dirSearcher.FindOne();

            if (userResult != null)
            {
                var userEntry = userResult.GetDirectoryEntry();

                _dirSearcher.Filter = $"(&(objectClass=group)(sAMAccountName={group}))";
                var groupResult = _dirSearcher.FindOne();

                if (groupResult != null)
                {
                    var groupEntry = groupResult.GetDirectoryEntry();

                    if (groupEntry.Properties["member"].Contains(userEntry.Properties["distinguishedName"].Value))
                    {
                        groupEntry.Properties["member"].Remove(userEntry.Properties["distinguishedName"].Value);
                        groupEntry.CommitChanges();
                    }
                }
            }

            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dirSearcher?.Dispose();
                    _dirEntry?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
