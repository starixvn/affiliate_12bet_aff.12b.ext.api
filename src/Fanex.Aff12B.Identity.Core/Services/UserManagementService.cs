using Fanex.Aff12B.Common.Core.Models;
using Fanex.Aff12B.Identity.Core.Extensions;
using Microsoft.Extensions.Options;

namespace Fanex.Aff12B.Identity.Core.Services
{
    public class UserManagementService(IOptions<List<User>> userListAccessor) : IUserManagementService
    {
        public User GetValidUser(string username, string password)
            => userListAccessor.Value.Find(x => x.Username.Equals(username)
            && x.Password.Equals(password.Sha256()));
    }
}
