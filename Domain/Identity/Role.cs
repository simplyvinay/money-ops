﻿using System.Collections.Generic;
using MoneyOps.Infrastructure;
using MoneyOps.Infrastructure.Auth.Authorization;

namespace MoneyOps.Domain.Identity
{
    public class Role : Entity
    {
        private string _permissionsInRole;

        public Role(
            string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public ICollection<UserRole> Users { get; } = new HashSet<UserRole>();
        public IEnumerable<Permissions> PermissionsInRole => _permissionsInRole.UnpackPermissions();

        public void AddPermissions(
            ICollection<Permissions> permissions)
        {
            _permissionsInRole = permissions.PackPermissions();
        }

        public void AddUser(
            User user)
        {
            Users.Add(
                new UserRole(
                    user,
                    this));
        }
    }
}