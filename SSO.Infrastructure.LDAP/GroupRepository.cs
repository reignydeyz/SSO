using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SSO.Domain.Interfaces;
using SSO.Domain.Models;
using SSO.Infrastructure.LDAP.Models;
using SSO.Infrastructure.Management;
using System.DirectoryServices;

namespace SSO.Infrastructure.LDAP
{
    public class GroupRepository : GroupRepositoryBase
    {
        readonly IAppDbContext _context;
        readonly DirectoryEntry _dirEntry;

        public GroupRepository(IAppDbContext context, IOptions<LDAPSettings> ldapSettings) : base(context)
        {
            _context = context;

            var ldapConnectionString = $"{(ldapSettings.Value.UseSSL ? "LDAPS" : "LDAP")}://{ldapSettings.Value.Server}:{ldapSettings.Value.Port}/{ldapSettings.Value.SearchBase}";
            _dirEntry = new DirectoryEntry(ldapConnectionString, ldapSettings.Value.Username, ldapSettings.Value.Password);
        }

        public override async Task<Group> Add(Group param, bool? saveChanges = true, object? args = null)
        {
            var newGroup = _dirEntry.Children.Add($"CN={param.Name}", "group");

            newGroup.Properties["sAMAccountName"].Value = param.Name;
            newGroup.Properties["description"].Value = param.Description ?? string.Empty;
            newGroup.CommitChanges();

            _context.Add(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return param;
        }

        public override async Task Delete(Group param, bool? saveChanges = true)
        {
            _context.Remove(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public override async Task<Group> Update(Group param, bool? saveChanges = true, object? args = null)
        {
            var rec = await _context.Groups.FirstAsync(x => x.GroupId == param.GroupId);

            rec.Name = param.Name;
            rec.Description = param.Description;
            rec.ModifiedBy = param.ModifiedBy;
            rec.DateModified = DateTime.Now;

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();

            return rec;
        }
    }
}
