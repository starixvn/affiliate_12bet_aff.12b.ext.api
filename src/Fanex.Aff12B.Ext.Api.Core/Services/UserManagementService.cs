using Fanex.Aff12B.Ext.Api.Core.Extensions;
using Fanex.Aff12B.Ext.Api.Core.Models;
using Microsoft.Extensions.Options;

namespace Fanex.Aff12B.Ext.Api.Core.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly List<User> _users;

        public UserManagementService(IOptions<List<User>> userListAccessor)
        {
            _users = userListAccessor.Value;
        }

        public bool IsValidUser(string username, string password)
            => _users.Exists(x => x.UserName.Equals(username)
            && x.Password.Equals(password.Sha256()));
    }
}
