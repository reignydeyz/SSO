using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Management;
using System.DirectoryServices;

namespace SSO.Infrastructure.LDAP
{
    public class GroupRepository : GroupRepositoryBase, IDisposable
    {
        readonly IAppDbContext _context;
        readonly DirectoryEntry _dirEntry;
        readonly DirectorySearcher _dirSearcher;

        bool _disposed;

        public GroupRepository(IAppDbContext context, IOptions<LDAPSettings> ldapSettings) : base(context)
        {
            _context = context;

            var ldapConnectionString = $"{(ldapSettings.Value.UseSSL ? "LDAPS" : "LDAP")}://{ldapSettings.Value.Server}:{ldapSettings.Value.Port}/{ldapSettings.Value.SearchBase}";
            _dirEntry = new DirectoryEntry(ldapConnectionString, ldapSettings.Value.Username, ldapSettings.Value.Password);
            _dirSearcher = new DirectorySearcher(_dirEntry);
        }

        public override async Task<Group> Add(Group param, bool? saveChanges = true, object? args = null)
        {
            var newGroup = _dirEntry.Children.Add($"CN={param.Name}", "group");

            newGroup.Properties["sAMAccountName"].Value = param.Name;
            newGroup.Properties["description"].Value = param.Description ?? string.Empty;
            newGroup.CommitChanges();

            await _context.AddAsync(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public override async Task Delete(Group param, bool? saveChanges = true)
        {
            _dirSearcher.Filter = $"(&(objectClass=group)(sAMAccountName={param.Name}))";
            _dirSearcher.SearchScope = SearchScope.Subtree;

            var result = _dirSearcher.FindOne();

            if (result != null)
            {
                var userEntry = result.GetDirectoryEntry();
                userEntry.DeleteTree();
                _dirEntry.CommitChanges();
            }

            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override async Task<Group> Update(Group param, bool? saveChanges = true, object? args = null)
        {
            var rec = await _context.Groups.FirstAsync(x => x.GroupId == param.GroupId);

            _dirSearcher.Filter = $"(&(objectClass=group)(sAMAccountName={rec.Name}))";
            _dirSearcher.SearchScope = SearchScope.Subtree;

            var result = _dirSearcher.FindOne();

            if (result != null)
            {
                var groupEntry = result.GetDirectoryEntry();

                groupEntry.Rename($"CN={param.Name}");
                groupEntry.Properties["sAMAccountName"].Value = param.Name;
                groupEntry.Properties["description"].Value = param.Description ?? string.Empty;
                groupEntry.CommitChanges();
            }

            rec.Name = param.Name;
            rec.Description = param.Description;
            rec.ModifiedBy = param.ModifiedBy;
            rec.DateModified = DateTime.Now;

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return rec;
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
