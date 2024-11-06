
using Fanex.Aff12B.Common.Core.Models;

namespace Fanex.Aff12B.Identity.Core.Services
{
    public interface IUserManagementService
    {
        User GetValidUser(string username, string password);
    }
}
