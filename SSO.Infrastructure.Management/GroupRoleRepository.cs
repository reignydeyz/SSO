﻿using Microsoft.EntityFrameworkCore;
using SSO.Domain.Management.Interfaces;
using SSO.Domain.Models;
using System.Data;
using System.Linq.Expressions;

namespace SSO.Infrastructure.Management
{
    public class GroupRoleRepository : IGroupRoleRepository
    {
        readonly AppDbContext _context;

        public GroupRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRange(IEnumerable<GroupRole> param, bool? saveChanges = true, object? args = null)
        {
            _context.AddRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GroupRole>> GroupRoles(Guid groupId, Guid applicationId)
        {
            var res = from r in _context.Roles.Where(x => x.ApplicationId == applicationId)
                    join gr in _context.GroupRoles.Where(x => x.GroupId == groupId) on r.Id equals gr.RoleId
                    select gr;

            return res.AsNoTracking();
        }

        public async Task RemoveRange(IEnumerable<GroupRole> param, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(param);

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(Expression<Func<GroupRole, bool>> predicate, bool? saveChanges = true, object? args = null)
        {
            _context.RemoveRange(_context.GroupRoles.Where(predicate));

            if (saveChanges!.Value)
                await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationRole>> Roles(Guid groupId, Guid applicationId)
        {
            var roles = from r in _context.Roles.Where(x => x.ApplicationId == applicationId)
                        join gr in _context.GroupRoles.Where(x => x.GroupId == groupId) on r.Id equals gr.RoleId
                        select r;

            return roles.AsNoTracking();
        }
    }
}
