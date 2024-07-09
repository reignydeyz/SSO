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
        private readonly IAppDbContext _context;
        private readonly DirectoryEntry _dirEntry;
        private readonly DirectorySearcher _dirSearcher;
        private bool _disposed;

        public GroupUserRepository(IAppDbContext context, IOptions<LDAPSettings> ldapSettings) : base(context)
        {
            _context = context;

            var ldapConnectionString = $"{(ldapSettings.Value.UseSSL ? "LDAPS" : "LDAP")}://{ldapSettings.Value.Server}:{ldapSettings.Value.Port}/{ldapSettings.Value.SearchBase}";
            _dirEntry = new DirectoryEntry(ldapConnectionString, ldapSettings.Value.Username, ldapSettings.Value.Password);
            _dirSearcher = new DirectorySearcher(_dirEntry);
        }

        public override Task<GroupUser> Add(GroupUser param, bool? saveChanges = true, object? args = null)
        {
            var (userEntry, groupEntry) = FindUserAndGroupEntries(param);

            if (userEntry != null && groupEntry != null)
            {
                AddUserToGroup(userEntry, groupEntry);
                groupEntry.CommitChanges();
            }

            _context.Add(param);
            if (saveChanges!.Value)
                return _context.SaveChangesAsync().ContinueWith(_ => param);

            return Task.FromResult(param);
        }

        public override Task Delete(GroupUser param, bool? saveChanges = true)
        {
            var (userEntry, groupEntry) = FindUserAndGroupEntries(param);

            if (userEntry != null && groupEntry != null)
            {
                RemoveUserFromGroup(userEntry, groupEntry);
                groupEntry.CommitChanges();
            }

            _context.Remove(param);

            if (saveChanges!.Value)
                return _context.SaveChangesAsync();

            return Task.CompletedTask;
        }

        private (DirectoryEntry?, DirectoryEntry?) FindUserAndGroupEntries(GroupUser param)
        {
            var group = _context.Groups.First(x => x.GroupId == param.GroupId).Name;
            var userName = _context.Users.First(x => x.Id == param.UserId).UserName;

            var userEntry = FindDirectoryEntryByFilter($"(&(objectClass=user)(sAMAccountName={userName}))");
            var groupEntry = FindDirectoryEntryByFilter($"(&(objectClass=group)(sAMAccountName={group}))");

            return (userEntry, groupEntry);
        }

        private DirectoryEntry? FindDirectoryEntryByFilter(string filter)
        {
            _dirSearcher.Filter = filter;
            var searchResult = _dirSearcher.FindOne();
            return searchResult?.GetDirectoryEntry();
        }

        private void AddUserToGroup(DirectoryEntry userEntry, DirectoryEntry groupEntry)
        {
            groupEntry.Properties["member"].Add(userEntry.Properties["distinguishedName"].Value);
        }

        private void RemoveUserFromGroup(DirectoryEntry userEntry, DirectoryEntry groupEntry)
        {
            groupEntry.Properties["member"].Remove(userEntry.Properties["distinguishedName"].Value);
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
