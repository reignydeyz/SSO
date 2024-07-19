﻿using SSO.Domain.Interfaces;
using SSO.Domain.Models;

namespace SSO.Domain.Management.Interfaces
{
    public interface IRealmUserRepository : IRepository<RealmUser>, IRangeRepository<RealmUser>
    {
    }
}
